//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Counter;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Adt.Graph;

public abstract class HyperGraph<TVertex, TEdge> : EntityType<id>
    where TVertex : Vertex
    where TEdge : HyperEdge<TVertex>
{
    public string Label { get; init; }

    public Flags Flags { get; init; }

    public Color Color { get; init; }

    public Dictionary<string, object> Attributes { get; init; }

    public Direction Direction { get; init; }

    public Dictionary<id, TVertex> Vertices { get; init; }

    protected Counter VertexCounter { get; init; }

    public HyperGraph(id id,
                      string? label = default,
                      Flags flags = Flags.Clear,
                      Color color = Color.Unknown,
                      Direction direction = Direction.Undirectional,
                      Dictionary<string, object>? attributes = default,
                      string? version = default) : base(id, version)
    {
        Label = label?.Trim() ?? $"G:{id.ToString()}";
        Flags = flags;
        Color = color;
        Attributes = attributes ?? new();
        Direction = direction;
        Vertices = new();
        VertexCounter = new();
    }

    public TVertex? GetVertex(id id)
    {
        if(Vertices.TryGetValue(id, out TVertex? vertex))
            return vertex;
        return default;
    }

    public void AddVertex(TVertex vertex)
    {
        Assert.NonNullReference(vertex, nameof(vertex));
        Assert.Ensure(!Vertices.ContainsKey(vertex.Id), nameof(vertex));

        Vertices.Add(vertex.Id, vertex);
    }

    public TVertex? RemoveVertex(id id)
    {
        if(Vertices.TryGetValue(id, out TVertex? vertex))
        {
            if(vertex.CanRelease())
            {
                Vertices.Remove(id);
            }
        }

        return vertex;
    }

    public void Cleanup()
    {
        var verticesToRemove = Vertices.Values.Where(vertex => vertex.CanRelease()).Select(vertex => vertex.Id);

        foreach(id id in verticesToRemove)
        {
            Vertices.Remove(id);
        }
    }

    public void ResetFlags(Flags flag, Flags add, Flags remove)
    {
        foreach(TVertex vertex in Vertices.Values)
        {
            vertex.Flags = (Flags)DomainHelper.ModifyFlags((flag)vertex.Flags, (flag)add, (flag)remove);
        }
    }

    public void ResetColor()
    {
        foreach(TVertex vertex in Vertices.Values)
        {
            vertex.Color = Color.Unknown;
        }
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
        yield return Label;
        yield return Vertices;
    }
}
