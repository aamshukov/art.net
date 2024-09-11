//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Adt.Graph;

/// <summary>
/// Represents vanila graph: 2-uniform hypergraph.
/// </summary>
/// <typeparam name="TVertex"></typeparam>
/// <typeparam name="TEdge"></typeparam>
public class Graph<TVertex, TEdge> : Hypergraph<TVertex, TEdge>
    where TVertex : class
    where TEdge : class
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
                 string? version = default) : base(id, label, flags, color, version)
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
