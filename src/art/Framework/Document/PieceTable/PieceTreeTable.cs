//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Content.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;
using ValueType = UILab.Art.Framework.Core.Domain.ValueType;

namespace UILab.Art.Framework.Document;

public sealed class PieceTreeTable : ValueType
{
    public PieceTree Pieces { get; init; }

    public size TabSize { get; init; }

    private id SeedPieceId = 2; // 0 - sentinels, 1 - original content

    private id NextPieceId() => Interlocked.Increment(ref SeedPieceId);

    /// <summary>
    /// Readonly original buffer, sequence of codepoints.
    /// </summary>
    private IContent<codepoint> OriginalContent { get; init; }

    /// <summary>
    /// Working (add) buffer, sequence of codepoints.
    /// </summary>
    private IContent<codepoint> WorkingContent { get; init; }

    public PieceTreeTable(IContent<codepoint> originalContent, IContent<codepoint> workingContent, size tabSize = 4)
    {
        Assert.NonNullReference(originalContent, nameof(originalContent));
        Assert.NonNullReference(workingContent, nameof(workingContent));
        Assert.Ensure(tabSize >= 0, nameof(tabSize));

        Pieces = PieceTree.Sentinel;

        OriginalContent = originalContent;
        WorkingContent = workingContent;

        TabSize = tabSize;
    }
}
