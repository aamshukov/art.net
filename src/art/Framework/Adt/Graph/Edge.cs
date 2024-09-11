//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;
using UILab.Art.Framework.Core.Domain.Abstractions;

namespace UILab.Art.Framework.Adt.Graph;

public class Edge<TValue> : EntityType<id>, IVisitable
{
    public static readonly Edge<TValue> Sentinel = new(0, "Edge:Sentinel");

    public string Label { get; init; }

    public TValue? Value { get; init; }

    public Flags Flags { get; init; }

    public Dictionary<string, object?> Attributes { get; init; }

    public Edge(id id,
                string? label = default,
                TValue? value = default,
                Flags flags = Flags.Clear,
                Dictionary<string, object?>? attributes = default,
                string? version = default) : base(id, version)
    {
        Label = label?.Trim() ?? id.ToString();
        Value = value;
        Flags = flags;
        Attributes = attributes ?? new();
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
        yield return Label;
        if(Value is not null)
            yield return Value;
    }

    public TResult? Accept<TParam, TResult>(IVisitor<TParam, TResult> visitor, TParam? param = default)
    {
        Assert.NonNullReference(visitor, nameof(visitor));
        return visitor.Visit(this, param);
    }
}
