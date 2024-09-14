//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Adt.Graph;

public class HyperGraph<TVertex, TEdge> : EntityType<id>
    where TVertex : class
    where TEdge : class
{
    public string Label { get; init; }

    public Flags Flags { get; init; }

    public Color Color { get; init; }

    public Dictionary<string, object> Attributes { get; init; }

    private id VertexId = 0;

    private id NextVertexId() => Interlocked.Increment(ref VertexId);

    private Dictionary<id, TVertex> Vertices { get; init; }

    private id EdgeId = 0;

    private id NextEdgeId() => Interlocked.Increment(ref EdgeId);

    private Dictionary<id, TEdge> Edges { get; init; }

    /// <summary>
    /// Vertex to adjacencies map.
    /// </summary>
    protected Dictionary<id, HyperEdge<TVertex, TEdge>> Adjacencies { get; init; }

    /// <summary>
    /// Edge to adjacencies map.
    /// </summary>
    protected Dictionary<id, HyperEdge<TVertex, TEdge>> Incidents { get; init; }

    public HyperGraph(id id,
                      string? label = default,
                      Flags flags = Flags.Clear,
                      Color color = Color.Unknown,
                      string? version = default) : base(id, version)
    {
        Label = label?.Trim() ?? string.Empty;

        Flags = flags;
        Color = color;

        Attributes = new();

        Vertices = new();
        Edges = new();

        Adjacencies = new();
        Incidents = new();
    }

    public TVertex? GetVertex(id id)
    {
        return default;
    }

    public bool TryGetVertex(id id, out TVertex? vertex)
    {
        vertex = default;
        return false;
    }

    public void AddVertex(TVertex vertex)
    {
        Assert.NonNullReference(vertex, nameof(vertex));
    }

    public bool TryAddVertex(TVertex vertex)
    {
        Assert.NonNullReference(vertex, nameof(vertex));
        return false;
    }

    public TVertex? RemoveVertex(id id)
    {
        return default;
    }

    public bool TryRemoveVertex(id id, out TVertex? vertex)
    {
        vertex = default;
        return false;
    }

    public TEdge? GetEdge(id id)
    {
        return default;
    }

    public bool TryGetEdge(id id, out TEdge? edge)
    {
        edge = default;
        return false;
    }

    public void AddEdge(TEdge edge)
    {
        Assert.NonNullReference(edge, nameof(edge));
    }

    public bool TryAddEdge(TEdge edge)
    {
        Assert.NonNullReference(edge, nameof(edge));
        return false;
    }

    public TEdge? RemoveEdge(id id)
    {
        return default;
    }

    public bool TryRemoveEdge(id id, out TEdge? edge)
    {
        edge = default;
        return false;
    }

    public HyperEdge<TVertex, TEdge>? GetHyperEdge(id id)
    {
        return default;
    }

    public bool TryGetHyperEdge(id id, out HyperEdge<TVertex, TEdge>? hyperEdge)
    {
        hyperEdge = default;
        return false;
    }

    public bool TryAddHyperEdge(HyperEdge<TVertex, TEdge> hyperEdge)
    {
        Assert.NonNullReference(hyperEdge, nameof(hyperEdge));
        return false;
    }

    public HyperEdge<TVertex, TEdge>? RemoveHyperEdge(id id)
    {
        return default;
    }

    public bool TryRemoveHyperEdge(id id, out HyperEdge<TVertex, TEdge>? hyperEdge)
    {
        hyperEdge = default;
        return false;
    }

    /// <summary>
    /// Gets the number of edges incident to the vertex.
    /// </summary>
    /// <param name="vertex"></param>
    public count GetDegree(TVertex vertex)
    {
        Assert.NonNullReference(vertex, nameof(vertex));
        return 0;
    }

    /// <summary>
    /// Gets the number of incoming edges incident to the vertex.
    /// </summary>
    /// <param name="vertex"></param>
    public count GetInDegree(TVertex vertex)
    {
        Assert.NonNullReference(vertex, nameof(vertex));
        return 0;
    }

    /// <summary>
    /// Gets the number of outcoming edges incident to the vertex.
    /// </summary>
    /// <param name="vertex"></param>
    public count GetOutDegree(TVertex vertex)
    {
        Assert.NonNullReference(vertex, nameof(vertex));
        return 0;
    }

    /// <summary>
    /// Finds an edge that connects the vertex U to the vertex V.
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    public TEdge? FindEdge(TVertex u, TVertex v)
    {
        Assert.NonNullReference(u, nameof(u));
        Assert.NonNullReference(v, nameof(v));
        return default;
    }

    /// <summary>
    /// Finds all edges that connects the vertex U to the vertex V.
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    public IEnumerable<TEdge> FindEdges(TVertex u, TVertex v)
    {
        Assert.NonNullReference(u, nameof(u));
        Assert.NonNullReference(v, nameof(v));
        yield return (TEdge)Enumerable.Empty<TEdge>();
    }

    /// <summary>
    /// Gets all vertices which are predecessors of the vertex.
    /// </summary>
    /// <param name="vertex"></param>
    public IEnumerable<TVertex> CollectPredecessors(TVertex vertex)
    {
        Assert.NonNullReference(vertex, nameof(vertex));
        yield return (TVertex)Enumerable.Empty<TVertex>();
    }

    /// <summary>
    /// Gets all vertices which are successors of the vertex.
    /// </summary>
    /// <param name="vertex"></param>
    public IEnumerable<TVertex> CollectSuccessors(TVertex vertex)
    {
        Assert.NonNullReference(vertex, nameof(vertex));
        yield return (TVertex)Enumerable.Empty<TVertex>();
    }

    /// <summary>
    /// Gets all vertices which are connected to the vertex via any edges.
    /// </summary>
    /// <param name="vertex"></param>
    public IEnumerable<TVertex> GetNeighbors(TVertex vertex)
    {
        Assert.NonNullReference(vertex, nameof(vertex));
        yield return (TVertex)Enumerable.Empty<TVertex>();
    }

    /// <summary>
    /// Calculates incidence matrix: V𝑛, E𝑚 then incidence matrix of size [𝑛, 𝑚].
    /// Values might be:
    ///     undirected - 0, 1, 2
    ///     directed   - 0, 1
    /// </summary>
    public byte[,] CalculateIncidenceMatrix()
    {
        size n = 0;
        size m = 0;

        byte[,] matrix = new byte[n, m];

        return matrix;
    }

    /// <summary>
    /// Calculates adjacency matrix: V𝑛, V𝑛 then adjacency matrix of size [𝑛, n].
    /// Values might be: 0, 1
    /// </summary>
    public byte[,] CalculateAdjacencyMatrix()
    {
        size n = 0;

        byte[,] matrix = new byte[n, n];

        return matrix;
    }

    /// <summary>
    /// Checks if the hypergraph is a k-uniform hypergraph.
    /// </summary>
    /// <param name="k"></param>
    public virtual bool IsUniform(size k = 0)
    {
        return true;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
        yield return Label;
        yield return Vertices;
        yield return Edges;
    }
}
