//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public class UndirectedEdge : UndirectedHyperEdge
{
    /// <summary>
    /// Gets the first element of the vertices, mimics graph's endpoint.
    /// </summary>
    public UndirectedVertex? U { get; init; }

    /// <summary>
    /// Gets the second element of the vertices, mimics graph's endpoint.
    /// </summary>
    public UndirectedVertex? V { get; init; }

    public UndirectedEdge(id id,
                          UndirectedVertex u,
                          UndirectedVertex v,
                          string? label = default,
                          Flags flags = Flags.Clear,
                          Dictionary<string, object>? attributes = default,
                          string? version = default) : base(id, label, [u, v], flags, attributes, version)
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
