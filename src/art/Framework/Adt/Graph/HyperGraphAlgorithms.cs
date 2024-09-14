//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Adt.Graph;

public static class HyperGraphAlgorithms
{
    public static bool IsRoot<TVertex>(TVertex vertex)
        where TVertex : EntityType<id>
    {
        return false;
    }

    public static bool IsLeaf<TVertex>(TVertex vertex)
        where TVertex : EntityType<id>
    {
        return false;
    }

    public static bool IsBHyperEdge<TVertex, TEdge>(HyperEdge<TVertex, TEdge> hyperEdge)
        where TVertex : EntityType<id>
        where TEdge : EntityType<id>
    {
        return hyperEdge.Domain.Count == 1;
    }

    public static bool IsFHyperEdge<TVertex, TEdge>(HyperEdge<TVertex, TEdge> hyperEdge)
        where TVertex : EntityType<id>
        where TEdge : EntityType<id>
    {
        return hyperEdge.Codomain.Count == 1;
    }

    public static IEnumerable<HyperEdge<TVertex, TEdge>> GetVertexForwardStar<TVertex, TEdge>(HyperGraph<TVertex, TEdge> hyperGraph,
                                                                                              TVertex vertex)
        where TVertex : EntityType<id>
        where TEdge : EntityType<id>
    {
        //yield return (HyperEdge<TVertex, TEdge>)Enumerable.Empty<HyperEdge<TVertex, TEdge>>();
        throw new NotImplementedException();
    }

    public static IEnumerable<HyperEdge<TVertex, TEdge>> GetVertexBackwardStar<TVertex, TEdge>(HyperGraph<TVertex, TEdge> hyperGraph,
                                                                                               TVertex vertex)
        where TVertex : EntityType<id>
        where TEdge : EntityType<id>
    {
        //yield return (HyperEdge<TVertex, TEdge>)Enumerable.Empty<HyperEdge<TVertex, TEdge>>();
        throw new NotImplementedException();
    }
}
