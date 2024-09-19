//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Counter;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public class UndirectedHyperGraph : HyperGraph<UndirectedVertex, UndirectedHyperEdge>
{
    public Dictionary<id, UndirectedHyperEdge> HyperEdges { get; init; }

    private Counter HyperEdgeCounter { get; init; }

    public UndirectedHyperGraph(id id,
                                string? label = default,
                                Flags flags = Flags.Clear,
                                Color color = Color.Unknown,
                                Dictionary<string, object>? attributes = default,
                                string? version = default) : base(id, label, flags, color, Direction.Undirectional, attributes, version)
    {
        HyperEdges = new();
        HyperEdgeCounter = new();
    }

    public UndirectedVertex CreateVertex(string? label = default,
                                         List<HyperEdge<UndirectedVertex>>? hyperEdges = default,
                                         object? value = default,
                                         Flags flags = Flags.Clear,
                                         Color color = Color.Unknown,
                                         Dictionary<string, object>? attributes = default,
                                         string? version = default)
    {
        return new(VertexCounter.NextId(), label, hyperEdges, value, flags, color, attributes, version);
    }

    public UndirectedHyperEdge CreateHyperEdge(string? label = default,
                                               List<UndirectedVertex>? vertices = default,
                                               Flags flags = Flags.Clear,
                                               Dictionary<string, object>? attributes = default,
                                               string? version = default)
    {
        return new(HyperEdgeCounter.NextId(), label, vertices, flags, attributes, version);
    }

    public UndirectedHyperEdge? GetHyperEdge(id id)
    {
        if(HyperEdges.TryGetValue(id, out UndirectedHyperEdge? hyperEdge))
            return hyperEdge;
        return default;
    }

    public bool TryGetEdge(id id, out UndirectedHyperEdge? hyperEdge)
    {
        return HyperEdges.TryGetValue(id, out hyperEdge);
    }

    public void AddHyperEdge(UndirectedHyperEdge hyperEdge)
    {
        Assert.NonNullReference(hyperEdge, nameof(hyperEdge));

        HyperEdges.Add(hyperEdge.Id, hyperEdge);
        hyperEdge.UpdateDependencies(link: true);
    }

    public bool TryAddHyperEdge(UndirectedHyperEdge hyperEdge)
    {
        Assert.NonNullReference(hyperEdge, nameof(hyperEdge));

        bool result = !HyperEdges.ContainsKey(hyperEdge.Id);

        if(result)
        {
            AddHyperEdge(hyperEdge);
        }

        return result;
    }

    public UndirectedHyperEdge? RemoveHyperEdge(id id)
    {
        if(HyperEdges.Remove(id, out UndirectedHyperEdge? hyperEdge))
        {
            hyperEdge.UpdateDependencies(link: false);
            UpdateDependencies(hyperEdge);
            return hyperEdge;
        }

        return default;
    }

    public bool TryRemoveHyperEdge(id id, out UndirectedHyperEdge? hyperEdge)
    {
        if(HyperEdges.Remove(id, out hyperEdge))
        {
            hyperEdge.UpdateDependencies(link: false);
            UpdateDependencies(hyperEdge);
            return true;
        }

        return false;
    }

    public UndirectedVertex? RemoveVertex(id id, bool weak)
    {
        foreach(UndirectedHyperEdge hyperEdge in HyperEdges.Values)
        {
            hyperEdge.RemoveVertex(id);
        }

        foreach(UndirectedHyperEdge hyperEdge in HyperEdges.Values)
        {
            if(hyperEdge.Vertices.Count == 0)
            {
                RemoveHyperEdge(hyperEdge.Id);
            }
        }

        return base.RemoveVertex(id);
    }

    public bool TryRemoveVertex(id id, out UndirectedVertex? vertex, bool weak)
    {
        foreach(UndirectedHyperEdge hyperEdge in HyperEdges.Values)
        {
            hyperEdge.RemoveVertex(id);
        }

        foreach(UndirectedHyperEdge hyperEdge in HyperEdges.Values)
        {
            if(hyperEdge.Vertices.Count == 0)
            {
                RemoveHyperEdge(hyperEdge.Id);
            }
        }

        return base.TryRemoveVertex(id, out vertex);
    }

    private void UpdateDependencies(UndirectedHyperEdge hyperEdge)
    {
        foreach(UndirectedHyperEdge edge in HyperEdges.Values)
        {
            if(ReferenceEquals(edge, hyperEdge))
                continue;

            foreach(UndirectedVertex vertex in edge.Vertices.Values)
                vertex.HyperEdges.Remove(hyperEdge.Id);
        }
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
    }
}
