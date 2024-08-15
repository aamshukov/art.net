//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics.CodeAnalysis;
using Art.Framework.Core.Domain;
using UILab.Art.Framework.Core.Content.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Text;
using ValueType = UILab.Art.Framework.Core.Domain.ValueType;

namespace Art.Framework.Document.PieceTable;

public sealed class PieceTable : ValueType
{
    private LinkedList<Piece> Pieces { get; init; }

    public count Count => Pieces.Count;

    public size TabSize { get; init; }

    /// <summary>
    /// Readonly original buffer, sequence of codepoints.
    /// </summary>
    private IContent<codepoint> OriginalContent { get; init; }

    /// <summary>
    /// Working (add) buffer, sequence of codepoints.
    /// </summary>
    private IContent<codepoint> WorkingContent { get; init; }

    public PieceTable(IContent<codepoint> originalContent, IContent<codepoint> workingContent, size tabSize = 4)
    {
        Assert.NonNullReference(originalContent, nameof(originalContent));
        Assert.NonNullReference(workingContent, nameof(workingContent));
        Assert.Ensure(tabSize >= 0, nameof(tabSize));

        Pieces = new();

        OriginalContent = originalContent;
        WorkingContent = workingContent;

        TabSize = tabSize;

        Pieces.AddFirst(Piece.Sentinel);
        Pieces.AddLast(Piece.Sentinel);
    }

    public size Length() => Pieces.Aggregate(0, (result, piece) => result + piece.Span.Length);

    public Piece CreatePiece(id id, index start, size length, ContentType contentType = ContentType.Original)
    {
        Assert.Ensure(id >= 0, nameof(id));
        Assert.Ensure(start >= 0, $"{nameof(start)}");
        Assert.Ensure(length >= 0, nameof(length));

        Span span = new(start, length);
        Piece piece = new(id: id, span: span, contentType, version: Version);

        BuilLineMap(piece);

        return piece;
    }

    /// <summary>
    /// Adds a new Piece to the end of the list.
    /// </summary>
    /// <param name="piece"></param>
    public void AddPiece(Piece piece)
    {
        Assert.NonNullReference(piece, nameof(piece));
        Assert.NonNullReference(Pieces.Last != default, nameof(piece));

#pragma warning disable CS8604
        Pieces.AddBefore(Pieces.Last, piece); // add to the end, before tail sentinel
#pragma warning restore CS8604
    }

    public LinkedListNode<Piece> InsertPiece(Piece piece, LinkedListNode<Piece> anchorNode, bool insertAfter)
    {
        Assert.NonNullReference(piece, nameof(piece));

        if (insertAfter)
        {
            return Pieces.AddAfter(anchorNode, piece);
        }
        else
        {
            return Pieces.AddBefore(anchorNode, piece);
        }
    }

    public List<ListNodeProxy> InsertPieces(List<ListNodeProxy> pieces)
    {
        List<ListNodeProxy> swappedPieces = new();
        return swappedPieces;
    }

    /// <summary>
    /// Inserts codepoints.
    /// </summary>
    /// <param name="location"></param>
    /// <param name="codepoints"></param>
    /// <param name="contentOffset"></param>
    /// <returns></returns>
    public ListNodeProxy Insert(location location, ReadOnlyMemory<codepoint> codepoints, offset contentOffset)
    {
        // 0 1 2 3 4 5 6 7 8 9                     0 1 2 3 4 5 6 7 8 9 10
        // a A B C D E F G H                       a A B C D E F G H
        //         b                                       b b
        // a A B C                  0 : 4          a A B C                  0 : 4
        //         b                4 : 1                  b b              4 : 2
        //           D E F G H      5 : 5                      D E F G H    6 : 5
        Assert.Ensure(location >= 0, nameof(location));
        Assert.NonNullReference(codepoints, nameof(codepoints));

        FindPieceResult findPieceResult = FindPiece(location);

        var insertPosition = location - findPieceResult.Location;
        var existingPiece = findPieceResult.Piece;
        var codepointsLength = codepoints.Length;

        ListNodeProxy proxyNode;

        Assert.Ensure(insertPosition >= 0, nameof(insertPosition));

        if (insertPosition == 0 || findPieceResult.AtTheEnd) // if AtTheEnd is true then location way out of the end
        {
            // Case I: boundary case, either at the beginning of the peace or at the end ...
            var newPiece = CreatePiece(id: Count - 1,         // -1 because there two sentinels already
                                       start: contentOffset,  // location in working (added) content
                                       length: codepointsLength,
                                       contentType: ContentType.Added);

            InsertPiece(newPiece, existingPiece, insertAfter: findPieceResult.AtTheEnd);

            // make proxy to keep links
            proxyNode = MakeListNodeProxy(existingPiece);
        }
        else
        {
            // Case II: at the middle of the piece case ... split
            var newLhsPiece = CreatePiece(id: Count - 1,
                                          start: existingPiece.Value.Span.Start,
                                          length: insertPosition,
                                          contentType: existingPiece.Value.ContentType);

            var newMiddlePiece = CreatePiece(id: Count,
                                             start: contentOffset,
                                             length: codepointsLength,
                                             contentType: ContentType.Added);

            var newRhsPiece = CreatePiece(id: Count + 1,
                                          start: existingPiece.Value.Span.Start + insertPosition,
                                          length: existingPiece.Value.Span.Length - insertPosition,
                                          contentType: existingPiece.Value.ContentType);

            // LHS - piece before insertion : MIDDLE - new insertion piece : RHS - piece after insertion
            // insert in reverse order
            InsertPiece(newRhsPiece, existingPiece, insertAfter: true);
            InsertPiece(newMiddlePiece, existingPiece, insertAfter: true);
            InsertPiece(newLhsPiece, existingPiece, insertAfter: true);

            // make proxy to keep links
            proxyNode = MakeListNodeProxy(existingPiece);

            // remove the existing piece
            Pieces.Remove(existingPiece);
        }

        return proxyNode;
    }

    public List<ListNodeProxy> Delete(location location, size length)
    {
        Assert.Ensure(location >= 0, nameof(location));
        Assert.Ensure(length >= 0, nameof(length));

        DomainHelper.NormalizeRange(ref location, ref length, Length());

        List<LinkedListNode<Piece>> pieces = EnumerateCoveringRange(location, length, out location startPieceOffset, out location endPieceOffset);

        LinkedListNode<Piece>? lhsPieceNode = pieces.FirstOrDefault();
        LinkedListNode<Piece>? rhsPieceNode = pieces.LastOrDefault();

        if (lhsPieceNode is not null)
        {
            index newStart = location - startPieceOffset;
            size newLength = Math.Min(lhsPieceNode.Value.Span.Length - newStart, length);

            Assert.Ensure(newStart >= 0, nameof(newStart));
            Assert.Ensure(newLength >= 0, nameof(newLength));

            SplitForDeletion(newStart, newLength, lhsPieceNode.Value, out Piece? lhsPiece, out Piece? rhsPiece);

            if (lhsPiece is not null)
            {
                Pieces.AddBefore(lhsPieceNode, lhsPiece);
            }

            if (rhsPiece is not null)
            {
                Pieces.AddAfter(lhsPieceNode, rhsPiece);
            }
        }

        if (rhsPieceNode is not null && !ReferenceEquals(lhsPieceNode, rhsPieceNode))
        {
            index newStart = 0;
            size newLength = location + length - endPieceOffset;

            Assert.Ensure(newStart >= 0, nameof(newStart));
            Assert.Ensure(newLength >= 0, nameof(newLength));

            SplitForDeletion(newStart, newLength, rhsPieceNode.Value, out Piece? lhsPiece, out Piece? rhsPiece);

            if (lhsPiece is not null)
            {
                Pieces.AddBefore(rhsPieceNode, lhsPiece);
            }

            if (rhsPiece is not null)
            {
                Pieces.AddAfter(rhsPieceNode, rhsPiece);
            }
        }

        // make proxies to keep links
        List<ListNodeProxy> proxyNodes = new();

        foreach (var piece in pieces)
        {
            proxyNodes.Add(MakeListNodeProxy(piece));
        }

        // remove the existing pieces
        foreach (var piece in pieces)
        {
            Pieces.Remove(piece);
        }

        return proxyNodes;
    }

    /// <summary>
    /// Splits an existing piece for the deletion operation.
    /// </summary>
    /// <param name="offset">Relative (local) offset in the piece</param>
    /// <param name="length">Relative (local) length to consider for splitting in the piece.</param>
    /// <param name="piece"></param>
    /// <param name="lhsPiece"></param>
    /// <param name="rhsPiece"></param>
    private void SplitForDeletion(offset offset, size length, Piece piece, out Piece? lhsPiece, out Piece? rhsPiece)
    {
        // Case I: no split - offset == 0 and length == piece.length
        //      0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5
        //      A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
        //      ^                 unchanged                       ^     lhs = null, rhs = null
        // 
        // Case II: split - offset == 0 and length < piece.length
        //      0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5
        //      A B C D E F G H I J K L M N O P Q R S T U V W X Y Z     lhs = null, rhs = NOT null
        //      ^    delete     ^
        //
        // Case III: split - offset > 0 and offset + length == piece.length
        //      0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5
        //      A B C D E F G H I J K L M N O P Q R S T U V W X Y Z     lhs = NOT null, rhs = null
        //                                            ^  delete   ^
        //
        // Case IV: split - offset > 0 and offset + length < piece.length
        //      0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5
        //      A B C D E F G H I J K L M N O P Q R S T U V W X Y Z     lhs = NOT null, rhs = NOT null
        //                  ^      delete       ^
        //
        // Case V: no split - offset > 0 and length == piece.length, similar to Case I.
        //      0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5
        //      A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
        //            ^                                                 lhs = null, rhs = null
        //            ^
        Assert.Ensure(offset >= 0, nameof(offset));
        Assert.Ensure(length >= 0, nameof(length));
        Assert.NonNullReference(piece, nameof(piece));
        Assert.Ensure(length <= piece.Span.Length, nameof(length));

        index spanStart = piece.Span.Start;
        size spanLength = piece.Span.Length;

        if (offset == 0 && length == spanLength || length == 0)
        {
            // Case I:
            lhsPiece = default;
            rhsPiece = default;
        }
        else if (offset == 0 && length < spanLength)
        {
            // Case II:
            lhsPiece = default;
            rhsPiece = CreatePiece(id: Count - 1,
                                   start: spanStart + length,
                                   length: spanLength - length,
                                   contentType: piece.ContentType);
        }
        else if (offset > 0 && offset + length == spanLength)
        {
            // Case III:
            lhsPiece = default;
            rhsPiece = CreatePiece(id: Count - 1,
                                   start: spanStart,
                                   length: spanLength - length,
                                   contentType: piece.ContentType);
        }
        else if (offset > 0 && offset + length < spanLength)
        {
            // Case IV:
            lhsPiece = CreatePiece(id: Count - 1,
                                   start: spanStart,
                                   length: offset,
                                   contentType: piece.ContentType);
            rhsPiece = CreatePiece(id: Count - 1,
                                   start: spanStart + offset + length,
                                   length: spanLength - (offset + length),
                                   contentType: piece.ContentType);
        }
#pragma warning disable S125 // Sections of code should not be commented out
        //else if(offset > 0 && length == spanLength)
        //{
        //    // Case V:
        //    lhsPiece = default;
        //    rhsPiece = default;
        //    Assert.NotImplemented();
        //}
#pragma warning restore S125 // Sections of code should not be commented out
        else
        {
            lhsPiece = default;
            rhsPiece = default;
            Assert.NotImplemented();
        }
    }

    private sealed record FindPieceResult(location Location,           // absolute location of the found piece
                                          LinkedListNode<Piece> Piece, // found piece
                                          bool AtTheEnd);              // indicates if insertion should happen 'after'

    private FindPieceResult FindPiece(location location)
    {
        List<LinkedListNode<Piece>> pieces = EnumerateCoveringRange(location, 1, out location startPieceOffset, out location endPieceOffset, find: true);
        LinkedListNode<Piece>? piece = pieces.FirstOrDefault();

        if (piece is not null)
        {
#pragma warning disable S125 // Sections of code should not be commented out
            return new FindPieceResult(Location: startPieceOffset, Piece: piece, AtTheEnd: false); //location == span.End);
#pragma warning restore S125 // Sections of code should not be commented out
        }
        else
        {
#pragma warning disable CS8602
#pragma warning disable CS8604
            return new FindPieceResult(Location: endPieceOffset, Piece: Pieces.Last.Previous, AtTheEnd: true);
#pragma warning restore CS8604
#pragma warning restore CS8602
        }
    }

    public Piece FindExactPiece(location location,
                                out location pieceStartLocation) // absolute location of the found piece
    {
        pieceStartLocation = -1;

        location curLocation = 0;

        LinkedListNode<Piece>? node = Pieces.First?.Next;

        while (node != default && node != Pieces.Last)
        {
            var span = node.Value.Span;

            if (location >= curLocation && location <= curLocation + span.Length - 1)
            {
                pieceStartLocation = curLocation;
                return node.Value;
            }

            node = node.Next;
            curLocation += span.Length;
        }

        return Piece.Sentinel;
    }

    /// <summary>
    /// Enumerates covering range of pieces.
    /// First and/or last piece(s) might 'stick out'.
    /// </summary>
    /// <param name="location"></param>
    /// <param name="length"></param>
    /// <param name="startPieceOffset">Absolute offset of the first found piece in the document.</param>
    /// <param name="endPieceOffset">Absolute offset of the end found piece in the document.</param>
    /// <returns></returns>
    [SuppressMessage("Major Code Smell", "S907:\"goto\" statement should not be used", Justification = "<Pending>")]
    public List<LinkedListNode<Piece>> EnumerateCoveringRange(location location, size length, out location startPieceOffset, out location endPieceOffset, bool find = false)
    {
        Assert.Ensure(location >= 0, nameof(location));
        Assert.Ensure(length >= 0, nameof(length));

        startPieceOffset = 0;
        endPieceOffset = 0;

        List<LinkedListNode<Piece>> pieces = new();

        if (length == 0)
        {
            goto _exit;
        }

        // 0 1 2 3 4  5  6 7 8  9 0 1 2 3  4 5 6 7 8 9 0 1 2  3 4 5
        // A B C D E  F  G H I  J K L M N  O P Q R S T U V W  X Y Z
        // ^    or    ^
        // ^          ^
        // 0 1 2 3 4  5  6 7 8  9 0 1 2 3  4 5 6 7 8 9 0 1 2  3 4 5
        // A B C D E  F  G H I  J K L M N  O P Q R S T U V W  X Y Z
        // ^   ^
        // 0 1 2 3 4  5  6 7 8  9 0 1 2 3  4 5 6 7 8 9 0 1 2  3 4 5
        // A B C D E  F  G H I  J K L M N  O P Q R S T U V W  X Y Z
        // ^       ^
        // 0 1 2 3 4  5  6 7 8  9 0 1 2 3  4 5 6 7 8 9 0 1 2  3 4 5
        // A B C D E  F  G H I  J K L M N  O P Q R S T U V W  X Y Z
        //   ^     ^
        // 0 1 2 3 4  5  6 7 8  9 0 1 2 3  4 5 6 7 8 9 0 1 2  3 4 5
        // A B C D E  F  G H I  J K L M N  O P Q R S T U V W  X Y Z
        //   ^        ^
        // 0 1 2 3 4  5  6 7 8  9 0 1 2 3  4 5 6 7 8 9 0 1 2  3 4 5
        // A B C D E  F  G H I  J K L M N  O P Q R S T U V W  X Y Z
        //   ^             ^
        // 0 1 2 3 4  5  6 7 8  9 0 1 2 3  4 5 6 7 8 9 0 1 2  3 4 5
        // A B C D E  F  G H I  J K L M N  O P Q R S T U V W  X Y Z
        //   ^               ^
        // 0 1 2 3 4  5  6 7 8  9 0 1 2 3  4 5 6 7 8 9 0 1 2  3 4 5
        // A B C D E  F  G H I  J K L M N  O P Q R S T U V W  X Y Z
        // ^                                                      ^
        // 0 1 2 3 4  5  6 7 8  9 0 1 2 3  4 5 6 7 8 9 0 1 2  3 4 5
        // A B C D E  F  G H I  J K L M N  O P Q R S T U V W  X Y Z
        //   ^                                                    ^
        location start = location;          // inclusive index
        location end = start + length - 1;  // inclusive index

        location current = 0;

        LinkedListNode<Piece>? node = Pieces.First?.Next;

        // find starting span containing the location
        while (node != Pieces.Last && node is not null)
        {
            var span = node.Value.Span;
            var spanLength = span.Length;

            if (current + spanLength - 1 >= location) // start, mimics if(span.Contains(start))
            {
                startPieceOffset = current;
                pieces.Add(node);
                break;
            }

            current += spanLength;
            node = node.Next;
        }

        if (find)
            goto _exit;

        LinkedListNode<Piece>? startPieceNode = pieces.FirstOrDefault();

        if (startPieceNode is not null)
        {
            // find ending containing the location, collecting all spans on the run
            while (node != Pieces.Last && node is not null)
            {
                var span = node.Value.Span;
                var spanLength = span.Length;

                if (current + spanLength - 1 >= end) // end, mimics if(span.Contains(end))
                {
                    endPieceOffset = current;
                    pieces.Add(node);
                    break;
                }

                current += spanLength;

                pieces.Add(node);
                node = node.Next;
            }

            LinkedListNode<Piece>? startPieceNode2 = pieces.Skip(1).FirstOrDefault();

            if (ReferenceEquals(startPieceNode, startPieceNode2))
            {
                pieces.Remove(startPieceNode2);
            }
        }

    _exit:
        Assert.Ensure(startPieceOffset >= 0, nameof(startPieceOffset));
        Assert.Ensure(endPieceOffset >= 0, nameof(endPieceOffset));

        return pieces;
    }

    public List<Piece> GetPieces(location location, size length = size.MaxValue)
    {
        //  location = 2, length = 6
        //  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9
        //  . . . . . . . . . .  . . . . . . . . . .
        //      ^         ^

        //  location = 2, length = 12
        //  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9
        //  . . . . . . . . . .  . . . . . . . . . .
        //      ^                      ^
        Assert.Ensure(location >= 0, nameof(location));
        Assert.Ensure(length >= 0, nameof(length));

        DomainHelper.NormalizeRange(ref location, ref length, Length());

        List<LinkedListNode<Piece>> pieces = EnumerateCoveringRange(location, length, out location startPieceOffset, out location endPieceOffset);

        LinkedListNode<Piece>? lhsPieceNode = pieces.FirstOrDefault();
        LinkedListNode<Piece>? rhsPieceNode = pieces.LastOrDefault();

        Piece? newLhsPiece = default;
        Piece? newRhsPiece = default;

        if (lhsPieceNode is not null)
        {
            index newStart = location - startPieceOffset;
            size newLength = Math.Min(lhsPieceNode.Value.Span.Length - newStart, length);

            Assert.Ensure(newStart >= 0, nameof(newStart));
            Assert.Ensure(newLength >= 0, nameof(newLength));

            if (newLength > 0 && newLength != lhsPieceNode.Value.Span.Length)
            {
                newLhsPiece = CreatePiece(id: lhsPieceNode.Value.Id,
                                          start: newStart,
                                          length: newLength,
                                          contentType: lhsPieceNode.Value.ContentType);
            }
        }

        if (rhsPieceNode is not null && !ReferenceEquals(lhsPieceNode, rhsPieceNode))
        {
            index newStart = 0;
            size newLength = location + length - endPieceOffset;

            Assert.Ensure(newStart >= 0, nameof(newStart));
            Assert.Ensure(newLength >= 0, nameof(newLength));

            if (newLength > 0 && newLength != rhsPieceNode.Value.Span.Length)
            {
                newRhsPiece = CreatePiece(id: rhsPieceNode.Value.Id,
                                          start: newStart,
                                          length: newLength,
                                          contentType: rhsPieceNode.Value.ContentType);
            }
        }

        if (newLhsPiece is not null)
        {
            pieces.RemoveAt(0);
            pieces.Insert(0, new(newLhsPiece));
        }

        if (newRhsPiece is not null)
        {
            pieces.RemoveAt(pieces.Count - 1);
            pieces.Add(new(newRhsPiece));
        }

        return pieces.Select(p => p.Value).ToList();
    }

    private void BuilLineMap(Piece piece)
    {
        //  end-of-line : 0x000D         \r     CR
        //              | 0x000A         \n     LF
        //              | 0x000D 0x000A  \r\n   CRLF
        //              | 0x0085                <Next Line> (NEL)
        //              | 0x2028                LINE SEPARATOR
        //              | 0x2029                PARAGRAPH SEPARATOR
        Assert.NonNullReference(piece, nameof(piece));

        LineMappings lineMappings = piece.LineMappings;

        List<location> lineMap = lineMappings.LineMap;

        ReadOnlyMemory<codepoint>? codepointsOpt = piece.ContentType == ContentType.Added ?
                                                        WorkingContent.Contents.LastOrDefault()?.Data :
                                                        OriginalContent.Contents.LastOrDefault()?.Data;
        if (codepointsOpt is null)
            return;

        ReadOnlyMemory<codepoint> codepoints = codepointsOpt.Value;

        index spanIndex = 0;
        size spanLength = Math.Min(codepoints.Length, piece.Span.Length);

        index lineStart = 0;

        Assert.Ensure(spanIndex >= 0, nameof(spanIndex));
        Assert.Ensure(spanLength >= 0, nameof(spanLength));

        while (spanIndex < spanLength)
        {
            codepoint ch0 = codepoints.Span[spanIndex + 0];
            codepoint ch1 = spanIndex + 1 < spanLength ? codepoints.Span[spanIndex + 1] : Codepoint.BadCodepoint;

            if (ch0 == 0x0000000D)
            {
                if (ch1 == 0x0000000A)
                {
                    spanIndex += 2;
                    lineStart += 1;
                    lineMappings.CrLf++;
                    lineMap.Add(lineStart);
                }
                else
                {
                    spanIndex += 1;
                    lineStart += 1;
                    lineMappings.Cr++;
                    lineMap.Add(lineStart);
                }
            }
            else if (ch0 == 0x0000000A)
            {
                spanIndex += 1;
                lineStart += 1;
                lineMappings.Lf++;
                lineMap.Add(lineStart);
            }
            else if (ch0 == 0x00000085)
            {
                spanIndex += 1;
                lineStart += 1;
                lineMappings.Nel++;
                lineMap.Add(lineStart);
            }
            else if (ch0 == 0x00002028)
            {
                spanIndex += 1;
                lineStart += 1;
                lineMappings.Ls++;
                lineMap.Add(lineStart);
            }
            else if (ch0 == 0x00002029)
            {
                spanIndex += 1;
                lineStart += 1;
                lineMappings.Ps++;
                lineMap.Add(lineStart);
            }
            else
            {
                spanIndex += 1;
            }

            if (spanIndex >= spanLength)
            {
                break;
            }
        }
    }

    [SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<Pending>")]
    private static index GetLine(location location, LineMappings lineMappings)
    {
        index result = 0;
        return result;
    }

    [SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<Pending>")]
    [SuppressMessage("Major Code Smell", "S4144:Methods should not have identical implementations", Justification = "<Pending>")]
    private static index GetColumn(location location, LineMappings lineMappings)
    {
        index result = 0;
        return result;
    }

    [SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<Pending>")]
    private static Position GetPosition(location location, LineMappings lineMappings)
    {
        Position result = new(0, 0);
        return result;
    }

#pragma warning disable CS8604 // Possible null reference argument.
    private static ListNodeProxy MakeListNodeProxy(LinkedListNode<Piece> node) => new ListNodeProxy(node, node.Previous, node.Next);
#pragma warning restore CS8604 // Possible null reference argument.
}
