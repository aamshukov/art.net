//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Tree;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Document;

public sealed class PieceTree : RedBlackTree<index>
{
    public Span Span { get; init; }

    /// <summary>
    /// Text length of the node's left subtree.
    /// </summary>
    public size Size { get; init; }

    public LineMappings LineMappings { get; init; }

    public ContentType ContentType { get; init; }

    public PieceTree(id id, Span span, ContentType contentType, string? version = default) : base(id: id, version: version)
    {
        Assert.Ensure(id >= 0, nameof(id));
        Assert.NonNullReference(span, nameof(span));

        Span = span;
        Size = span.Length;
        LineMappings = new();
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
        yield return ContentType;
        // ignore LineMappings, might be large
    }
}
