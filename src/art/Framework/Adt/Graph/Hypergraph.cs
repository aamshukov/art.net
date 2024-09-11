//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Adt.Graph;

public class Hypergraph<TVertex, TEdge> : EntityType<id>
    where TVertex : class
    where TEdge : class
{
    public string Label { get; init; }

    /// <summary>
    /// Root vertex.
    /// Optional, used in some algorithms.
    /// </summary>
    public TVertex? Root { get; set; }

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
    protected Dictionary<id, Adjacency<TVertex, TEdge>> Adjacencies { get; init; }

    /// <summary>
    /// Edge to adjacencies map.
    /// </summary>
    protected Dictionary<id, Adjacency<TVertex, TEdge>> Incidents { get; init; }

    public Hypergraph(id id,
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
    }

    public bool TryAddVertex(TVertex vertex)
    {
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
    }

    public bool TryAddEdge(TEdge edge)
    {
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

    public Adjacency<TVertex, TEdge>? GetAdjacency(id id)
    {
        return default;
    }

    public bool TryGetAdjacency(id id, out Adjacency<TVertex, TEdge>? adjacency)
    {
        adjacency = default;
        return false;
    }

    public bool TryAddAdjacency(Adjacency<TVertex, TEdge> adjacency)
    {
        return false;
    }

    public Adjacency<TVertex, TEdge>? RemoveAdjacency(id id)
    {
        return default;
    }

    public bool TryRemoveAdjacency(id id, out Adjacency<TVertex, TEdge>? adjacency)
    {
        adjacency = default;
        return false;
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
