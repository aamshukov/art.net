//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain.Abstractions;

namespace UILab.Art.Framework.Adt.Graph;

public class UndirectedVertex : Vertex
{
    public static readonly UndirectedVertex Sentinel = new(0, "UndirectedVertex:Sentinel");

    public Dictionary<id, UndirectedHyperEdge> HyperEdges { get; init; }

    public UndirectedVertex(id id,
                            string? label = default,
                            List<UndirectedHyperEdge>? hyperEdges = default,
                            object? value = default,
                            Flags flags = Flags.Clear,
                            Color color = Color.Unknown,
                            Dictionary<string, object>? attributes = default,
                            string? version = default) : base(id, label, value, flags, color, attributes, version)
    {
        HyperEdges = hyperEdges?.ToDictionary(kvp => kvp.Id, kvp => kvp) ?? new();
    }

    public bool IsEmpty() => HyperEdges.Count == 0;

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
    }

    public override TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param = default)
        where TResult : default
        where TParam : default
    {
        Assert.NonNullReference(visitor, nameof(visitor));
        return visitor.Visit<TParam, TResult>(this, param);
    }
}
