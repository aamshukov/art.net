//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Adt.Graph;

/// <summary>
/// Represents vanila graph: 2-uniform hypergraph.
/// </summary>
/// <typeparam name="TVertex"></typeparam>
/// <typeparam name="TEdge"></typeparam>
public class Graph<TVertex, TEdge> : HyperGraph<TVertex, TEdge>
    where TVertex : EntityType<id>
    where TEdge : EntityType<id>
{
    /// <summary>
    /// Root vertex.
    /// Optional, used in some algorithms.
    /// </summary>
    public TVertex? Root { get; set; }

    public Graph(id id,
                 string? label = default,
                 Flags flags = Flags.Clear,
                 Color color = Color.Unknown,
                 Dictionary<string, object>? attributes = default,
                 string? version = default) : base(id, label, flags, color, attributes, version)
    {
    }

    /// <summary>
    /// Checks if the graph is a k-uniform hypergraph.
    ///                          2-uniform
    /// </summary>
    /// <param name="k"></param>
    public override bool IsUniform(size k = 0)
    {
        return k == 0 || k == 2;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
    }
}
