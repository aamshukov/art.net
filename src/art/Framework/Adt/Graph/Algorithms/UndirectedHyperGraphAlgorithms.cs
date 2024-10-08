﻿//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Text;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

/// <summary>
/// Some comments are generated by ChatGPT.
/// </summary>
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
    /// Two vertices are hyperEdge-adjacent if they both belong to the same hyperedge, meaning they share a common connection through that hyperedge.
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

    public static bool IsLeaf(UndirectedVertex u)
    {
        count degree = GetVertexDegree(u);
        return degree == 1 || degree == 0;
    }

    public static UndirectedVertex ContractHyperEdge(UndirectedHyperGraph graph, UndirectedHyperEdge contractedHyperEdge)
    {
        Assert.NonNullReference(graph, nameof(graph));
        Assert.NonNullReference(contractedHyperEdge, nameof(contractedHyperEdge));

        var vertices = contractedHyperEdge.Vertices.Values;

        // Merge all vertices in 𝑒 into a single vertex 𝑣𝑒.
        UndirectedVertex contractedVertex = MergeVertices(graph, vertices);

        graph.AddVertex(contractedVertex);
        
        // Any hyperedges that included one or more vertices from 𝑒 are updated to include 𝑣𝑒 instead of the individual vertices.
        foreach(UndirectedHyperEdge hyperEdge in graph.HyperEdges.Values)
        {
            if(ReferenceEquals(hyperEdge, contractedHyperEdge))
                continue;

            List<UndirectedVertex> verticesToRemove = new();

            foreach(UndirectedVertex vertex in hyperEdge.Vertices.Values)
            {
                if(vertices.Contains(vertex))
                {
                    verticesToRemove.Add(vertex);
                }
            }

            if(verticesToRemove.Count > 0)
            {
                // If a hyperedge includes all the vertices of 𝑒, after contraction, it collapses to 𝑣𝑒.
                // This happens automatically as we remove all vertices in case they are the same as in the contracting hyperedge.
                foreach(UndirectedVertex vertex in verticesToRemove)
                {
                    hyperEdge.RemoveVertex(vertex.Id);
                }

                hyperEdge.AddVertex(contractedVertex);
            }
        }

        // Remove the original hyperedge 𝑒 after contraction.
        graph.RemoveHyperEdge(contractedHyperEdge.Id, RemoveActionType.Weak);

        graph.Cleanup();

        return contractedVertex;
    }

    private static UndirectedVertex MergeVertices(UndirectedHyperGraph graph, IEnumerable<UndirectedVertex> vertices)
    {
        StringBuilder sb = new("V");

        foreach(UndirectedVertex vertex in vertices)
        {
            sb.Append($":{vertex.Label}");
        }

        UndirectedVertex contractedVertex = graph.CreateVertex(label: sb.ToString());
        return contractedVertex;

    }

    public static IEnumerable<UndirectedVertex> GetSelfLoopVertices(UndirectedHyperEdge hyperEdge)
    {
        Assert.NonNullReference(hyperEdge, nameof(hyperEdge));

        if(hyperEdge.Vertices.Count == 1)
        {
            yield return hyperEdge.Vertices.First().Value;
        }
        else
        {
            var grouppedVertices = hyperEdge.Vertices.Values.GroupBy(v => v.Id).Where(g => g.Count() > 1).SelectMany(g => g);

            foreach(var vertex in grouppedVertices)
            {
                yield return vertex;
            }
        }
    }

    public static IEnumerable<UndirectedVertex> Dfs(UndirectedHyperGraph graph,
                                                    UndirectedVertex u,
                                                    bool preorder = true,
                                                    IObserver<UndirectedVertex>? observer = default)

    {
        Assert.NonNullReference(graph, nameof(graph));
        Assert.NonNullReference(u, nameof(u));

        Stack<UndirectedVertex> stack = new();

        stack.Push(u);

        while(stack.Count > 0)
        {
            UndirectedVertex v = stack.Pop();

            if(v.Flags.Has(Flags.Visited))
                continue;

            v.Flags = HyperGraphAlgorithms.ModifyFlags(v.Flags, add: Flags.Visited);

            if(preorder)
            {
                observer?.OnNext(v);
                yield return v;
            }

            foreach(UndirectedVertex v_adjacence in UndirectedHyperGraphAlgorithms.GetAdjacentVertices(v))
            {
                if(v_adjacence.Flags.HasNot(Flags.Visited))
                {
                    stack.Push(v_adjacence);
                }
            }

            if(!preorder) // postorder
            {
                observer?.OnNext(v);
                yield return v;
            }
        }

        observer?.OnCompleted();

        graph.ResetFlags(remove: Flags.Visited);
    }

    public static IEnumerable<UndirectedVertex> Bfs(UndirectedHyperGraph graph,
                                                    UndirectedVertex u,
                                                    bool preorder = true,
                                                    IObserver<UndirectedVertex>? observer = default)
    {
        Assert.NonNullReference(graph, nameof(graph));
        Assert.NonNullReference(u, nameof(u));

        Queue<UndirectedVertex> queue = new();

        queue.Enqueue(u);

        while(queue.Count > 0)
        {
            UndirectedVertex v = queue.Dequeue();

            if(v.Flags.Has(Flags.Visited))
                continue;

            v.Flags = HyperGraphAlgorithms.ModifyFlags(v.Flags, add: Flags.Visited);

            if(preorder)
            {
                observer?.OnNext(v);
                yield return v;
            }

            foreach(UndirectedVertex v_adjacence in UndirectedHyperGraphAlgorithms.GetAdjacentVertices(v))
            {
                if(v_adjacence.Flags.HasNot(Flags.Visited))
                {
                    queue.Enqueue(v_adjacence);
                }
            }

            if(!preorder) // postorder
            {
                observer?.OnNext(v);
                yield return v;
            }
        }

        observer?.OnCompleted();

        graph.ResetFlags(remove: Flags.Visited);
    }
}
