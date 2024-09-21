//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public static class UndirectedHyperGraphAlgorithms
{
    /// <summary>
    /// Gets the number of edges incident to the u.
    /// </summary>
    public static count GetVertexDegree(UndirectedVertex u)
    {
        Assert.NonNullReference(u, nameof(u));
        return u.HyperEdges.Count;
    }

    /// <summary>
    /// Two vertices are edge-adjacent if they both belong to the same hyperedge, meaning they share a common connection through that hyperedge.
    /// </summary>
    public static bool AreVerticesAdjacent(UndirectedVertex u, UndirectedVertex v)
    {
        Assert.NonNullReference(v, nameof(v));
        Assert.NonNullReference(u, nameof(u));

        bool adjacent = false;

        foreach(UndirectedHyperEdge hyperEdge in u.HyperEdges.Values)
        {
            if(hyperEdge.Vertices.ContainsKey(v.Id))
            {
                adjacent = true;
                break;
            }
        }

        return adjacent;
    }

    public static IEnumerable<UndirectedVertex> GetAdjacentVertices(UndirectedVertex u)
    {
        Assert.NonNullReference(u, nameof(u));

        foreach(UndirectedHyperEdge hyperEdge in u.HyperEdges.Values)
        {
            foreach(UndirectedVertex v in hyperEdge.Vertices.Values)
            {
                if(ReferenceEquals(u, v))
                    continue;

                yield return v;
            }
        }
    }

    /// <summary>
    /// A u is incident to a hyperedge if it belongs to the set of vertices that the hyperedge connects.
    /// </summary>
    public static IEnumerable<UndirectedHyperEdge> GetVertexIncidentHyperEdges(UndirectedVertex u)
    {
        Assert.NonNullReference(u, nameof(u));

        foreach(UndirectedHyperEdge hyperEdge in u.HyperEdges.Values)
        {
            yield return hyperEdge;
        }
    }

    public static bool IsVertexIsolated(UndirectedVertex u)
    {
        Assert.NonNullReference(u, nameof(u));
        return GetVertexDegree(u) == 0;
    }

    public static bool IsVertexPendant(UndirectedVertex u)
    {
        Assert.NonNullReference(u, nameof(u));
        return GetVertexDegree(u) == 1;
    }

    /// <summary>
    /// Gets all vertices which are connected to the u via any edges.
    /// </summary>
    public static IEnumerable<UndirectedVertex> GetVertexNeighbors(UndirectedVertex u)
    {
        Assert.NonNullReference(u, nameof(u));
        return GetAdjacentVertices(u);
    }

    public static count GetHyperEdgeDegree(UndirectedHyperEdge hyperEdge)
    {
        Assert.NonNullReference(hyperEdge, nameof(hyperEdge));
        return hyperEdge.Vertices.Count;
    }

    public static bool AreHyperEdgesAdjacent(UndirectedHyperEdge hyperEdge1, UndirectedHyperEdge hyperEdge2)
    {
        Assert.NonNullReference(hyperEdge1, nameof(hyperEdge1));
        Assert.NonNullReference(hyperEdge2, nameof(hyperEdge2));

        return hyperEdge1.Vertices.Keys.Intersect(hyperEdge2.Vertices.Keys).Any();
    }

    public static bool IsHyperEdgeSingleton(UndirectedHyperEdge hyperEdge)
    {
        Assert.NonNullReference(hyperEdge, nameof(hyperEdge));
        return GetHyperEdgeDegree(hyperEdge) == 1;
    }

    /// <summary>
    /// Checks if the hypergraph is a k-regular hypergraph,
    /// i.e. all verticers have the same k-size degree - all vertices are incident to k edges.
    /// </summary>
    public static bool IsRegular(UndirectedHyperGraph graph, size k = 2)
    {
        Assert.NonNullReference(graph, nameof(graph));

        foreach(UndirectedVertex vertex in graph.Vertices.Values)
        {
            if(GetVertexDegree(vertex) != k)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks if the hypergraph is a k-uniform hypergraph, i.e. all hyperedges have the same k-size degree.
    /// </summary>
    public static bool IsUniform(UndirectedHyperGraph graph, size k = 2)
    {
        Assert.NonNullReference(graph, nameof(graph));

        foreach(UndirectedHyperEdge hyperEdge in graph.HyperEdges.Values)
        {
            if(hyperEdge.Vertices.Count != k)
            {
                return false;
            }
        }

        return true;
    }

    /// ContractHyperEdge()
}
