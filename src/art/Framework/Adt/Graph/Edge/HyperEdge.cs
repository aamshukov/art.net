//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Domain;
using UILab.Art.Framework.Core.Domain.Abstractions;

namespace UILab.Art.Framework.Adt.Graph;

public abstract class HyperEdge<TVertex> : EntityType<id>, IVisitable
    where TVertex : Vertex
{
    public string Label { get; init; }

    public Flags Flags { get; set; }

    public Dictionary<string, object> Attributes { get; init; }

    public HyperEdge(id id,
                     string? label = default,
                     Flags flags = Flags.Clear,
                     Dictionary<string, object>? attributes = default,
                     string? version = default) : base(id, version)
    {
        Label = label?.Trim() ?? $"E:{id.ToString()}";
        Flags = flags;
        Attributes = attributes ?? new();
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
        yield return Label;
    }

    public abstract TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param = default);
}
