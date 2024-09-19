//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public static class UndirectedHyperGraphAlgorithms
{
    /// <summary>
    /// Gets the number of edges incident to the vertex.
    /// </summary>
    public static count GetVertexDegree(UndirectedHyperGraph graph, UndirectedVertex vertex)
    {
        Assert.NonNullReference(graph, nameof(graph));
        Assert.NonNullReference(vertex, nameof(vertex));

        count degree = 0;

        foreach(UndirectedHyperEdge hyperEdge in graph.HyperEdges.Values)
        {
            if(hyperEdge.Vertices.ContainsKey(vertex.Id))
            {
                degree++;
            }
        }

        return degree;
    }

    /// <summary>
    /// Two vertices are edge-adjacent if they both belong to the same hyperedge, meaning they share a common connection through that hyperedge.
    /// </summary>
    public static bool AreVerticesAdjacent(UndirectedHyperGraph graph, UndirectedVertex u, UndirectedVertex v)
    {
        Assert.NonNullReference(graph, nameof(graph));
        Assert.NonNullReference(v, nameof(v));
        Assert.NonNullReference(u, nameof(u));

        bool adjacent = false;

        foreach(UndirectedHyperEdge hyperEdge in graph.HyperEdges.Values)
        {
            if(hyperEdge.Vertices.ContainsKey(v.Id) && hyperEdge.Vertices.ContainsKey(u.Id))
            {
                adjacent = true;
                break;
            }
        }

        return adjacent;
    }

    public static IEnumerable<UndirectedVertex> GetAdjacentVertices(UndirectedHyperGraph graph, UndirectedVertex u)
    {
        Assert.NonNullReference(graph, nameof(graph));
        Assert.NonNullReference(u, nameof(u));

        foreach(UndirectedHyperEdge hyperEdge in graph.HyperEdges.Values)
        {
            if(hyperEdge.Vertices.ContainsKey(u.Id))
            {
                foreach(UndirectedVertex v in hyperEdge.Vertices.Values)
                {
                    if(ReferenceEquals(u, v))
                        continue;

                    yield return v;
                }
            }
        }
    }

    /// <summary>
    /// A vertex is incident to a hyperedge if it belongs to the set of vertices that the hyperedge connects.
    /// </summary>
    public static IEnumerable<UndirectedHyperEdge> GetVertexIncidentHyperEdges(UndirectedHyperGraph graph, UndirectedVertex vertex)
    {
        Assert.NonNullReference(graph, nameof(graph));
        Assert.NonNullReference(vertex, nameof(vertex));

        foreach(UndirectedHyperEdge hyperEdge in graph.HyperEdges.Values)
        {
            if(hyperEdge.Vertices.ContainsKey(vertex.Id))
            {
                yield return hyperEdge;
            }
        }
    }

    public static bool IsVertexIsolated(UndirectedHyperGraph graph, UndirectedVertex vertex)
    {
        Assert.NonNullReference(graph, nameof(graph));
        Assert.NonNullReference(vertex, nameof(vertex));

        return GetVertexDegree(graph, vertex) == 0;
    }

    public static bool IsVertexPendant(UndirectedHyperGraph graph, UndirectedVertex vertex)
    {
        Assert.NonNullReference(graph, nameof(graph));
        Assert.NonNullReference(vertex, nameof(vertex));

        return GetVertexDegree(graph, vertex) == 1;
    }

    /// <summary>
    /// Gets all vertices which are connected to the vertex via any edges.
    /// </summary>
    public static IEnumerable<UndirectedVertex> GetVertexNeighbors(UndirectedHyperGraph graph, UndirectedVertex vertex)
    {
        Assert.NonNullReference(graph, nameof(graph));
        Assert.NonNullReference(vertex, nameof(vertex));

        return GetAdjacentVertices(graph, vertex);
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
            if(GetVertexDegree(graph, vertex) != k)
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
