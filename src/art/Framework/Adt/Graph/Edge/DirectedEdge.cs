//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public class DirectedEdge : DirectedHyperEdge
{
    /// <summary>
    /// Gets the first element of the vertices, mimics graph's endpoint.
    /// </summary>
    public DirectedVertex? U { get; init; }

    /// <summary>
    /// Gets the second element of the vertices, mimics graph's endpoint.
    /// </summary>
    public DirectedVertex? V { get; init; }

    public DirectedEdge(id id,
                        DirectedVertex? u = default,
                        DirectedVertex? v = default,
                        string? label = default,
                        Flags flags = Flags.Clear,
                        Dictionary<string, object>? attributes = default,
                        string? version = default) : base(id, label, domain: [u], codomain: [v is null ? u : v], flags, attributes, version)
    {
        U = u;
        V = v;
    }

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
