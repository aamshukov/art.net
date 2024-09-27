//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using UILab.Art.Framework.Core.Content.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;
using UILab.Art.Framework.Core.Text;
using UILab.Art.Framework.Core.Text.Search;
using UILab.Art.Framework.Document.Abstractions;
using UILab.Art.Framework.Document.History;

namespace UILab.Art.Framework.Document;

public class Document : EntityType<string>, IDocument, IDisposable, IAsyncDisposable
{
    private readonly object syncRoot = new();

    private const size DefaultBufferSize = 1024 * 4;

    private const size DefaultTabSize = 4;

    public bool Disposed { get; private set; }

    /// <summary>
    /// Origin of the document.
    /// </summary>
    public string Source { get; init; }

    /// <summary>
    /// The famous piece table :) !.
    /// </summary>
    private PieceTable PieceTable { get; init; }

    /// <summary>
    /// Length of every buffer in the working content, might be in sync with original content.
    /// </summary>
    private size BufferSize { get; init; }

    public size TabSize { get; init; }

    /// <summary>
    /// Gets encoding.
    /// </summary>
    public TextEncoding Encoding { get; init; }

    /// <summary>
    /// Readonly original buffer, pieces of codepoints.
    /// </summary>
    private IContent<codepoint> OriginalContent { get; init; }

    /// <summary>
    /// Working (add) buffer, pieces of codepoints.
    /// </summary>
    private IContent<codepoint> WorkingContent { get; init; }

    /// <summary>
    /// Undo/Redo history.
    /// </summary>
    private IDocumentHistory History { get; init; }

    public bool Dirty { get; set; }

    public DocumentCursor Cursor { get; init; }

    public Document(string id,
                    string source,
                    IContent<codepoint> content,
                    IDocumentHistory history,
                    TextEncoding encoding = TextEncoding.Ascii,
                    size bufferSize = DefaultBufferSize,
                    size tabSize = DefaultTabSize,
                    string? version = default) : base(id, version)
    {
        Assert.NonEmptyString(id, nameof(id));
        Assert.NonEmptyString(source, nameof(source));
        Assert.NonNullReference(content, nameof(content));
        Assert.NonNullReference(history, nameof(history));
        Assert.Ensure(tabSize >= 0, nameof(tabSize));

        Source = source;
        Disposed = false;
        BufferSize = bufferSize;
        TabSize = tabSize;
        Encoding = encoding;
        History = history;
        Dirty = false;
        Cursor = new();

        OriginalContent = content;
        WorkingContent = new DocumentContent(id: $"{((DocumentContent)content).Id}-{ContentType.Added}",
                                             source: $"{nameof(Document)}:{ContentType.Added}",
                                             bufferSize: BufferSize,
                                             version: version);

        PieceTable = new PieceTable(OriginalContent, WorkingContent, TabSize);

        Initialize();
    }

    private void Initialize()
    {
        if(OriginalContent.Length() > 0)
            PieceTable.AddPiece(PieceTable.CreatePiece(id: 1, start: 0, length: OriginalContent.Length(), contentType: ContentType.Original));
    }

    public size Length() => PieceTable.Length();

    public count GetLinesCount()
    {
        return 0;
    }

    public index GetLineFromLocation(location location)
    {
        return 0;
    }

    public location GetLocationFromLine(index line)
    {
        return 0;
    }

    [SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<Pending>")]
    public codepoint GetCodepoint(location location)
    {
        Assert.Ensure(location >= 0, nameof(location));

        codepoint result = Codepoint.BadCodepoint;

        Piece piece = PieceTable.FindExactPiece(location, out location pieceStartLocation);

        if(!ReferenceEquals(piece, Piece.Sentinel))
        //if(piece != Piece.Sentinel)
        {
            var content = piece.ContentType == ContentType.Added ? WorkingContent : OriginalContent;
            var offset = piece.Span.Start + (location - pieceStartLocation);

            result = content.GetCodepoint(offset, defaultValue: Codepoint.BadCodepoint);
        }

        return result;
    }

    public List<Piece> GetLine(index lineNumber)
    {
        Assert.Ensure(lineNumber >= 0, nameof(lineNumber));
        return new();
    }

    public List<Piece> GetPieces(location location, size length = size.MaxValue)
    {
        Assert.Ensure(location >= 0, nameof(location));
        Assert.Ensure(length >= 0, nameof(length));

        var pieces = PieceTable.GetPieces(location, length);
        return pieces;
    }

    [SuppressMessage("Major Bug", "S2201:Methods without side effects should not have their return values ignored", Justification = "<Pending>")]
    public string GetString(location location, size length = size.MaxValue)
    {
        Assert.Ensure(location >= 0, nameof(location));
        Assert.Ensure(length >= 0, nameof(length));

        DomainHelper.NormalizeRange(ref location, ref length, Length());

        var sb = new StringBuilder();

        var pieces = GetPieces(location, length);

        foreach(var piece in pieces)
        {
            var content = piece.ContentType == ContentType.Added ? WorkingContent : OriginalContent;

            foreach(ReadOnlyMemory<codepoint> codepoints in content.GetContent(piece.Span.Start, piece.Span.Length))
            {
                #pragma warning disable CA1806 // CA1806: Do not ignore method results
                MemoryMarshal.ToEnumerable<codepoint>(codepoints).Aggregate(sb, (result, codepoint) => sb.Append(Char.ConvertFromUtf32((int)codepoint)));
                #pragma warning restore CA1806
            }
        }

        return sb.ToString();
    }

    public List<Piece> GetContent()
    {
        return GetPieces(0);
    }

    public List<index> Find(ReadOnlyMemory<codepoint> pattern,
                            index start = 0,
                            size length = size.MaxValue,
                            count count = count.MaxValue,
                            TextEncoding encoding = TextEncoding.Ascii,
                            BoyerMoore.ZAlgorithm algorithm = BoyerMoore.ZAlgorithm.Gusfield)
    {
        // see UILab.Art.Framework.Core.Text.Search.BoyerMoore implementation for details.
        // Code duplication of the UILab.Art.Framework.Core.Text.Search.BoyerMoore search engine.
        Assert.NonNullReference(pattern, nameof(pattern));
        Assert.Ensure(start >= 0, nameof(start));
        Assert.Ensure(length >= 0, nameof(length));
        Assert.Ensure(count >= 0, nameof(count));

        List<index> matches = new();

        size textLength = Length();

        DomainHelper.NormalizeRange(ref start, ref length, textLength);

        if(length == 0 || count == 0 || textLength == 0 || pattern.Length == 0)
            return matches;

        ReadOnlySpan<codepoint> ptr = pattern.Span;

        // preprocessing
        BoyerMoore.CalculateBadCharValues(ptr, Encoding, out index[] badChars);
        BoyerMoore.CalculateGoodSuffixValues(ptr, out index[] upperCaseLprime, out size[] lowerCaseLprime, algorithm);

        // search
        size m = length;
        size n = ptr.Length;

        index k = n - 1; // global index, init with end of pattern
        index i;         // pattern's index
        index h;         // text's index
        index s = -1;    // sentinel - last found mathed index + 1, when search back DO NOT pass it

        while(k < m)
        {
            i = n - 1; // end of pattern
            h = k;

            while(i >= 0 && ptr[i] == GetCodepoint(h)) // match right to left
            {
                i = i - 1;
                h = h - 1;

                if(h < s) // check sentinel
                    break;
            }

            if(i < 0) // match
            {
                s = k + 1; // set sentinel

                matches.Add(k - n + 1); // ... ending at position k

                if(matches.Count == count)
                    break;

                size lprime2 = n > 1 ? lowerCaseLprime[2 - 1] : 0; // l'(2)
                k = k + n - lprime2;
            }
            else // mismatch
            {
                // shift P (increase k) by the maximum amount
                // deterrnined by the bad character rule and the good suffìx rule
                size shift = 1;

                if(i == n - 1)
                {
                    // one special case remains, when the first
                    // comparison (in pattern, P[n]) is mismatch then
                    // shift one place to the right.
                }
                else
                {
                    // bad char shift operates with mismatched char
                    size badCharShift = i - badChars[GetCodepoint(h)];
                    shift = Math.Max(shift, badCharShift);

                    // good suffix shift operates with the last matched character
                    size goodSuffixShift = upperCaseLprime[i + 1] > 0
                                                ? (n - upperCaseLprime[i + 1] - 1)  // -1 beacuse L' is index
                                                : (n - lowerCaseLprime[i] - 1);     // l' is size, still need to substract
                    shift = Math.Max(shift, goodSuffixShift);
                }

                k += shift;
            }
        }

        return matches;
    }

    public List<index> Replace(location location, ReadOnlyMemory<codepoint> codepoints, count count = count.MaxValue)
    {
        Assert.Ensure(location >= 0, nameof(location));
        Assert.Ensure(count >= 0, nameof(count));
        Assert.NonNullReference(codepoints, nameof(codepoints));
        return new();
    }

    /// <summary>
    /// Inserts codepoints.
    /// </summary>
    /// <param name="codepoints"></param>
    /// <returns>Number of inserted codepoints.</returns>
    public count Insert(location location, ReadOnlyMemory<codepoint> codepoints)
    {
        Assert.Ensure(location >= 0, nameof(location));
        Assert.NonNullReference(codepoints, nameof(codepoints));

        if(codepoints.Length == 0)
        {
            return 0;
        }

        PieceTable.Insert(location,
                          codepoints,
                          WorkingContent.Length(),
                          out id injectionPoint,
                          out List<Piece> addPieces,
                          out List<Piece> removePieces);

        DocumentHistoryLiveEntry historyEntry = new(History.NextGroup(),
                                                    injectionPoint,
                                                    addPieces,
                                                    removePieces,
                                                    DocumentEditActionType.Insert,
                                                    DocumentHistoryEntryOperation.Undo);
        History.Add(historyEntry);

        count count = WorkingContent.AppendContent(codepoints);
        Assert.Ensure(count == codepoints.Length, $"{nameof(count)}:{codepoints.Length}");

        Dirty = true;

        return count;
    }

    public void Delete(location location, size length = size.MaxValue)
    {
        Assert.Ensure(location >= 0, nameof(location));
        Assert.Ensure(length >= 0, nameof(length));

        if(length > 0)
        {
            PieceTable.Delete(location,
                              length,
                              out id injectionPoint,
                              out List<Piece> addPieces,
                              out List<Piece> removePieces);

            DocumentHistoryLiveEntry historyEntry = new(History.NextGroup(),
                                                        injectionPoint,
                                                        addPieces,
                                                        removePieces,
                                                        DocumentEditActionType.Delete,
                                                        DocumentHistoryEntryOperation.Undo);
            History.Add(historyEntry);

            Dirty = true;
        }
    }

    public void Clear()
    {
    }

    [SuppressMessage("Critical Code Smell", "S1186:Methods should not be empty", Justification = "<Pending>")]
    public void ApplyHistory(IEnumerable<DocumentHistoryEntry> snapshot)
    {
    }

    public bool CanUndo()
    {
        return History.CanUndo();
    }

    [SuppressMessage("Minor Code Smell", "S3267:Loops should be simplified with \"LINQ\" expressions", Justification = "<Pending>")]
    public void Undo()
    {
        foreach(DocumentHistoryLiveEntry entry in History.Undo().OfType<DocumentHistoryLiveEntry>())
        {
            // restore to the prev state
            PieceTable.Restore(entry.InjectionPoint, entry.AddPieces, entry.RemovePieces);

            // switch add/restore entries for redo
            (entry.RemovePieces, entry.AddPieces) = (entry.AddPieces, entry.RemovePieces);
        }
    }

    public void ResetUndo()
    {
        History.ResetUndo();
    }

    public bool CanRedo()
    {
        return History.CanRedo();
    }

    [SuppressMessage("Minor Code Smell", "S3267:Loops should be simplified with \"LINQ\" expressions", Justification = "<Pending>")]
    public void Redo()
    {
        foreach(DocumentHistoryLiveEntry entry in History.Redo().OfType<DocumentHistoryLiveEntry>())
        {
            // restore to the prev state
            PieceTable.Restore(entry.InjectionPoint, entry.AddPieces, entry.RemovePieces);

            // switch add/restore entries for undo
            (entry.RemovePieces, entry.AddPieces) = (entry.AddPieces, entry.RemovePieces);
        }
    }

    public void ResetRedo()
    {
        History.ResetRedo();
    }

    [SuppressMessage("Minor Code Smell", "S3267:Loops should be simplified with \"LINQ\" expressions", Justification = "<Pending>")]
    private void UndoRedo(DocumentHistoryEntryOperation operation)
    {
        foreach(DocumentHistoryLiveEntry entry in History.Undo().OfType<DocumentHistoryLiveEntry>())
        {
            // restore to the prev state
            PieceTable.Restore(entry.InjectionPoint, entry.AddPieces, entry.RemovePieces);

            // switch add/restore entries for redo
            (entry.RemovePieces, entry.AddPieces) = (entry.AddPieces, entry.RemovePieces);
        }
    }

    public List<string> CollectPiecesInfo()
    {
        List<String> piecesInfo = new();

        offset offset = 0;

        foreach(var piece in PieceTable.Pieces)
        {
            piecesInfo.Add($"{piece.Id} -> {GetString(offset, piece.Span.Length)} : {offset} : {piece.Span.Length} : {piece.ContentType}");
            offset += piece.Span.Length;
        }

        return piecesInfo;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;

        yield return Source;
        yield return BufferSize;
        // ignore PieceTable, might be large
        // ignore OriginalContent, might be large
        // ignore WorkingContent, might be large
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    [SuppressMessage("Minor Code Smell", "S2486:Generic exceptions should not be ignored", Justification = "<Pending>")]
    [SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "<Pending>")]
    protected virtual async ValueTask DisposeAsyncCore()
    {
        try
        {
            if(!Disposed)
            {
                // managed resources
                History.Dispose();

                // unmanaged resources
            }
        }
        catch
        {
        }
        finally
        {
            Disposed = true;
        }

        await ValueTask.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    [SuppressMessage("Minor Code Smell", "S2486:Generic exceptions should not be ignored", Justification = "<Pending>")]
    [SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "<Pending>")]
    [SuppressMessage("Major Code Smell", "S1066:Mergeable \"if\" statements should be combined", Justification = "<Pending>")]
    protected virtual void Dispose(bool disposing)
    {
        if(!Disposed)
        {
            lock(syncRoot)
            {
                try
                {
                    if(!Disposed)
                    {
                        // managed resources
                        if(disposing)
                        {
                            History.Dispose();
                        }

                        // unmanaged resources
                    }
                }
                catch
                {
                }
                finally
                {
                    Disposed = true;
                }
            }
        }
    }
}
