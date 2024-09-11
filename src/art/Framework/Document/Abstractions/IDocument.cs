//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Text;
using UILab.Art.Framework.Core.Text.Search;

namespace UILab.Art.Framework.Document.Abstractions;

public interface IDocument
{
    size Length();

    count GetLinesCount();

    index GetLineFromLocation(location location);

    location GetLocationFromLine(index line);

    codepoint GetCodepoint(location location);

    List<Piece> GetLine(index lineNumber);

    List<Piece> GetPieces(location location, size length = size.MaxValue);

    /// <summary>
    /// Gets the entire content.
    /// </summary>
    /// <returns>Returns the entire content.</returns>
    List<Piece> GetContent();

    List<index> Find(ReadOnlyMemory<codepoint> pattern,
                     index start = 0,
                     size length = size.MaxValue,
                     count count = count.MaxValue,
                     TextEncoding encoding = TextEncoding.Ascii,
                     BoyerMoore.ZAlgorithm algorithm = BoyerMoore.ZAlgorithm.Gusfield);

    List<index> Replace(location location, ReadOnlyMemory<codepoint> codepoints, count count = count.MaxValue);

    count Insert(location location, ReadOnlyMemory<codepoint> codepoints);

    void Delete(location location, size length = size.MaxValue);

    void Clear();
}
