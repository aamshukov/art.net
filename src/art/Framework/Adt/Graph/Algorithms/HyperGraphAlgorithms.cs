//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Adt.Graph.Algorithms;

public static class HyperGraphAlgorithms
{
    //public static bool IsRoot<TVertex>(TVertex vertex)
    //    where TVertex : EntityType<id>
    //{
    //    return false;
    //}

    //public static bool IsLeaf<TVertex>(TVertex vertex)
    //    where TVertex : EntityType<id>
    //{
    //    return false;
    //}

    //public static bool IsBHyperEdge<TVertex, TEdge>(HyperEdge<TVertex> hyperEdge)
    //    where TVertex : EntityType<id>
    //    where TEdge : EntityType<id>
    //{
    //    return hyperEdge.Domain.Count == 1;
    //}

    //public static bool IsFHyperEdge<TVertex, TEdge>(HyperEdge<TVertex> hyperEdge)
    //    where TVertex : EntityType<id>
    //    where TEdge : EntityType<id>
    //{
    //    return hyperEdge.Codomain.Count == 1;
    //}

    //public static IEnumerable<HyperEdge<TVertex>> GetVertexForwardStar<TVertex, TEdge>(HyperGraph<TVertex, TEdge> hyperGraph,
    //                                                                                          TVertex vertex)
    //    where TVertex : EntityType<id>
    //    where TEdge : EntityType<id>
    //{
    //    //yield return (HyperEdge<TVertex, TEdge>)Enumerable.Empty<HyperEdge<TVertex, TEdge>>();
    //    throw new NotImplementedException();
    //}

    //public static IEnumerable<HyperEdge<TVertex>> GetVertexBackwardStar<TVertex, TEdge>(HyperGraph<TVertex, TEdge> hyperGraph,
    //                                                                                           TVertex vertex)
    //    where TVertex : EntityType<id>
    //    where TEdge : EntityType<id>
    //{
    //    //yield return (HyperEdge<TVertex, TEdge>)Enumerable.Empty<HyperEdge<TVertex, TEdge>>();
    //    throw new NotImplementedException();
    //}

    ///// <summary>
    ///// Checks if the hypergraph is a k-uniform hypergraph.
    ///// </summary>
    ///// <param name="k"></param>
    //public virtual bool IsUniform(size k = 0)
    //{
    //    return true;
    //}


    ///// <summary>
    ///// Calculates incidence matrix: V𝑛, E𝑚 then incidence matrix of size [𝑛, 𝑚].
    ///// Values might be:
    /////     undirected - 0, 1, 2
    /////     directed   - 0, 1
    ///// </summary>
    //public byte[,] CalculateIncidenceMatrix()
    //{
    //    size n = 0;
    //    size m = 0;

    //    byte[,] matrix = new byte[n, m];

    //    return matrix;
    //}

    ///// <summary>
    ///// Calculates adjacency matrix: V𝑛, V𝑛 then adjacency matrix of size [𝑛, n].
    ///// Values might be: 0, 1
    ///// </summary>
    //public byte[,] CalculateAdjacencyMatrix()
    //{
    //    size n = 0;

    //    byte[,] matrix = new byte[n, n];

    //    return matrix;
    //}

    ///// <summary>
    ///// Gets the number of edges incident to the vertex.
    ///// </summary>
    ///// <param name="vertex"></param>
    //public count GetDegree(TVertex vertex)
    //{
    //    Assert.NonNullReference(vertex, nameof(vertex));
    //    return 0;
    //}

    ///// <summary>
    ///// Gets the number of incoming edges incident to the vertex.
    ///// </summary>
    ///// <param name="vertex"></param>
    //public count GetInDegree(TVertex vertex)
    //{
    //    Assert.NonNullReference(vertex, nameof(vertex));
    //    return 0;
    //}

    ///// <summary>
    ///// Gets the number of outcoming edges incident to the vertex.
    ///// </summary>
    ///// <param name="vertex"></param>
    //public count GetOutDegree(TVertex vertex)
    //{
    //    Assert.NonNullReference(vertex, nameof(vertex));
    //    return 0;
    //}

    ///// <summary>
    ///// Finds an edge that connects the vertex U to the vertex V.
    ///// </summary>
    ///// <param name="u"></param>
    ///// <param name="v"></param>
    //public TEdge? FindEdge(TVertex u, TVertex v)
    //{
    //    Assert.NonNullReference(u, nameof(u));
    //    Assert.NonNullReference(v, nameof(v));
    //    return default;
    //}

    ///// <summary>
    ///// Finds all edges that connects the vertex U to the vertex V.
    ///// </summary>
    ///// <param name="u"></param>
    ///// <param name="v"></param>
    //public IEnumerable<TEdge> FindEdges(TVertex u, TVertex v)
    //{
    //    Assert.NonNullReference(u, nameof(u));
    //    Assert.NonNullReference(v, nameof(v));
    //    yield return (TEdge)Enumerable.Empty<TEdge>();
    //}

    ///// <summary>
    ///// Gets all vertices which are predecessors of the vertex.
    ///// </summary>
    ///// <param name="vertex"></param>
    //public IEnumerable<TVertex> CollectPredecessors(TVertex vertex)
    //{
    //    Assert.NonNullReference(vertex, nameof(vertex));
    //    yield return (TVertex)Enumerable.Empty<TVertex>();
    //}

    ///// <summary>
    ///// Gets all vertices which are successors of the vertex.
    ///// </summary>
    ///// <param name="vertex"></param>
    //public IEnumerable<TVertex> CollectSuccessors(TVertex vertex)
    //{
    //    Assert.NonNullReference(vertex, nameof(vertex));
    //    yield return (TVertex)Enumerable.Empty<TVertex>();
    //}

    ///// <summary>
    ///// Gets all vertices which are connected to the vertex via any edges.
    ///// </summary>
    ///// <param name="vertex"></param>
    //public IEnumerable<TVertex> GetNeighbors(TVertex vertex)
    //{
    //    Assert.NonNullReference(vertex, nameof(vertex));
    //    yield return (TVertex)Enumerable.Empty<TVertex>();
    //}

    ///// <summary>
    ///// Calculates incidence matrix: V𝑛, E𝑚 then incidence matrix of size [𝑛, 𝑚].
    ///// Values might be:
    /////     undirected - 0, 1, 2
    /////     directed   - 0, 1
    ///// </summary>
    //public byte[,] CalculateIncidenceMatrix()
    //{
    //    size n = 0;
    //    size m = 0;

    //    byte[,] matrix = new byte[n, m];

    //    return matrix;
    //}

    ///// <summary>
    ///// Calculates adjacency matrix: V𝑛, V𝑛 then adjacency matrix of size [𝑛, n].
    ///// Values might be: 0, 1
    ///// </summary>
    //public byte[,] CalculateAdjacencyMatrix()
    //{
    //    size n = 0;

    //    byte[,] matrix = new byte[n, n];

    //    return matrix;
    //}

    ///// <summary>
    ///// Checks if the hypergraph is a k-uniform hypergraph.
    ///// </summary>
    ///// <param name="k"></param>
    //public virtual bool IsUniform(size k = 0)
    //{
    //    return true;
    //}

}
