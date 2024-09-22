//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain.Abstractions;

namespace UILab.Art.Framework.Adt.Graph;

public class GraphVisitor<TVertex, TEdge> : IVisitor
    where TVertex : Vertex
    where TEdge : HyperEdge<TVertex>
{
    public HyperGraph<TVertex, TEdge> Graph { get; init; }

    public GraphVisitor(HyperGraph<TVertex, TEdge> graph)
    {
        Assert.NonNullReference(graph, nameof(graph));
        Graph = graph;
    }

    public virtual TResult? Visit<TParam, TResult>(IVisitable visitable, TParam? param = default)
    {
        Assert.NonNullReference(visitable, nameof(visitable));
        return default; //??
    }
}
