//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public class DirectedVertex : Vertex
{
    public static readonly DirectedVertex Sentinel = new(0, "DirectedVertex:Sentinel");

    /// <summary>
    /// Incoming hyper edges / hyper arcs.
    /// </summary>
    public Dictionary<id, DirectedHyperEdge> InHyperEdges { get; init; }

    /// <summary>
    /// Outcoming hyper edges / hyper arcs.
    /// </summary>
    public Dictionary<id, DirectedHyperEdge> OutHyperEdges { get; init; }

    public DirectedVertex(id id,
                          string? label = default,
                          List<DirectedHyperEdge>? inHyperEdges = default,
                          List<DirectedHyperEdge>? outHyperEdges = default,
                          object? value = default,
                          Flags flags = Flags.Clear,
                          Color color = Color.Unknown,
                          Dictionary<string, object>? attributes = default,
                          string? version = default) : base(id, label, value, flags, color, attributes, version)
    {
        InHyperEdges = inHyperEdges?.ToDictionary(kvp => kvp.Id, kvp => kvp) ?? new();
        OutHyperEdges = outHyperEdges?.ToDictionary(kvp => kvp.Id, kvp => kvp) ?? new();
    }

    public bool IsEmpty() => InHyperEdges.Count == 0 && OutHyperEdges.Count == 0;

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
    }

    public override TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param = default)
        where TResult : default
        where TParam : default
    {
        Assert.NonNullReference(visitor);
        return visitor.Visit<TParam, TResult>(this, param);
    }
}
