//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Document;

public sealed class Piece : EntityType<id>
{
    public static readonly Piece Sentinel = new(0, Span.Sentinel, ContentType.Sentinel);

    public Span Span { get; init; }

    public LineMappings LineMappings { get; init; }

    public ContentType ContentType { get; init; }

    public Piece(id id, Span span, ContentType contentType, string? version = default) : base(id, version)
    {
        Assert.Ensure(id >= 0, nameof(id));
        Assert.NonNullReference(span, nameof(span));

        Span = span;
        LineMappings = new();
        ContentType = contentType;
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
