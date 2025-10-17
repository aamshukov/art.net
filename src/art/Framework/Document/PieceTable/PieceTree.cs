//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
// Notes at:
//  http://e98cuenc.free.fr/wordprocessor/piecetable.html
//  https://code.visualstudio.com/blogs/2018/03/23/text-buffer-reimplementation
using UILab.Art.Framework.Adt.Tree;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Document;

public sealed class PieceTree : RedBlackTree<index>
{
    /// <summary>
    /// Self size - Span.Length
    /// </summary>
    public Span Span { get; init; }

    /// <summary>
    /// Text length of the node's left subtree.
    /// </summary>
    public size LeftSize { get; init; }

    /// <summary>
    /// Text length of the node's right subtree.
    /// </summary>
    public size RightSize { get; init; }

    public LineMappings LineMappings { get; init; }

    public LineMappings LeftLineMappings { get; init; }

    public LineMappings RightLineMappings { get; init; }

    public ContentType ContentType { get; init; }

    public PieceTree(id id,
                     Span span,
                     size leftSize,
                     size rightSize,
                     ContentType contentType,
                     string? version = default) : base(id: id, version: version)
    {
        Assert.Ensure(id >= 0, nameof(id));
        Assert.Ensure(leftSize >= 0, nameof(leftSize));
        Assert.Ensure(rightSize >= 0, nameof(rightSize));
        Assert.NonNullReference(span);

        Span = span;

        LeftSize = leftSize;
        RightSize = rightSize;

        LineMappings = new();
        LeftLineMappings = new();
        RightLineMappings = new();

        ContentType = contentType;
    }

    public override index GetKey()
    {
        return Span.Start;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;

        yield return Span;
        yield return LeftSize;
        yield return RightSize;
        yield return ContentType;
        // ignore LineMappings, might be large
    }
}
