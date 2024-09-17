//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Counter;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public class DirectedHyperGraph : HyperGraph<DirectedVertex, DirectedHyperEdge>
{
    public Dictionary<id, DirectedHyperEdge> HyperEdges { get; init; }

    private Counter HyperEdgeCounter { get; init; }

    public DirectedHyperGraph(id id,
                              string? label = default,
                              Flags flags = Flags.Clear,
                              Color color = Color.Unknown,
                              Dictionary<string, object>? attributes = default,
                              string? version = default) : base(id, label, flags, color, Direction.Directional, attributes, version)
    {
        HyperEdges = new();
        HyperEdgeCounter = new();
    }

    public DirectedVertex CreateVertex(string? label = default,
                                       List<HyperEdge<DirectedVertex>>? inHyperEdges = default,
                                       List<HyperEdge<DirectedVertex>>? outHyperEdges = default,
                                       object? value = default,
                                       Flags flags = Flags.Clear,
                                       Color color = Color.Unknown,
                                       Dictionary<string, object>? attributes = default,
                                       string? version = default)
    {
        return new(VertexCounter.NextId(), label, inHyperEdges, outHyperEdges, value, flags, color, attributes, version);
    }

    public DirectedHyperEdge CreateHyperEdge(string? label = default,
                                              List<DirectedVertex>? domain = default,
                                              List<DirectedVertex>? codomain = default,
                                              Flags flags = Flags.Clear,
                                              Dictionary<string, object>? attributes = default,
                                              string? version = default)
    {
        return new(HyperEdgeCounter.NextId(), label, domain, codomain, flags, attributes, version);
    }

    public DirectedHyperEdge? GetHyperEdge(id id)
    {
        if(HyperEdges.TryGetValue(id, out DirectedHyperEdge? hyperEdge))
            return hyperEdge;
        return default;
    }

    public bool TryGetEdge(id id, out DirectedHyperEdge? hyperEdge)
    {
        return HyperEdges.TryGetValue(id, out hyperEdge);
    }

    public void AddHyperEdge(DirectedHyperEdge hyperEdge)
    {
        Assert.NonNullReference(hyperEdge, nameof(hyperEdge));

        HyperEdges.Add(hyperEdge.Id, hyperEdge);
        hyperEdge.UpdateDependencies(link: true);
    }

    public bool TryAddHyperEdge(DirectedHyperEdge hyperEdge)
    {
        Assert.NonNullReference(hyperEdge, nameof(hyperEdge));

        bool result = !HyperEdges.ContainsKey(hyperEdge.Id);

        if(result)
        {
            AddHyperEdge(hyperEdge);
        }

        return result;
    }

    public DirectedHyperEdge? RemoveHyperEdge(id id)
    {
        if(HyperEdges.Remove(id, out DirectedHyperEdge? hyperEdge))
        {
            hyperEdge.UpdateDependencies(link: false);
            UpdateDependencies(hyperEdge);
            return hyperEdge;
        }

        return default;
    }

    public bool TryRemoveHyperEdge(id id, out DirectedHyperEdge? hyperEdge)
    {
        if(HyperEdges.Remove(id, out hyperEdge))
        {
            hyperEdge.UpdateDependencies(link: false);
            UpdateDependencies(hyperEdge);
            return true;
        }

        return false;
    }

    public override DirectedVertex? RemoveVertex(id id)
    {
        foreach(DirectedHyperEdge hyperEdge in HyperEdges.Values)
        {
            hyperEdge.RemoveVertex(id);
        }

        foreach(DirectedHyperEdge hyperEdge in HyperEdges.Values)
        {
            if(hyperEdge.Domain.Count == 0 || hyperEdge.Codomain.Count == 0)
            {
                RemoveHyperEdge(hyperEdge.Id);
            }
        }

        return base.RemoveVertex(id);
    }

    public override bool TryRemoveVertex(id id, out DirectedVertex? vertex)
    {
        foreach(DirectedHyperEdge hyperEdge in HyperEdges.Values)
        {
            hyperEdge.RemoveVertex(id);
        }

        foreach(DirectedHyperEdge hyperEdge in HyperEdges.Values)
        {
            if(hyperEdge.Domain.Count == 0 || hyperEdge.Codomain.Count == 0)
            {
                RemoveHyperEdge(hyperEdge.Id);
            }
        }

        return base.TryRemoveVertex(id, out vertex);
    }

    private void UpdateDependencies(DirectedHyperEdge hyperEdge)
    {
        foreach(DirectedHyperEdge edge in HyperEdges.Values)
        {
            if(ReferenceEquals(edge, hyperEdge))
                continue;

            foreach(DirectedVertex vertex in edge.Domain.Values)
            {
                vertex.InHyperEdges.Remove(hyperEdge.Id);
                vertex.OutHyperEdges.Remove(hyperEdge.Id);
            }

            foreach(DirectedVertex vertex in edge.Codomain.Values)
            {
                vertex.InHyperEdges.Remove(hyperEdge.Id);
                vertex.OutHyperEdges.Remove(hyperEdge.Id);
            }
        }
    }

    /// <summary>
    /// Gets all vertices which are connected to the vertex via any edges.
    /// </summary>
    /// <param name="vertex"></param>
    public IEnumerable<UndirectedVertex> GetNeighbors(UndirectedVertex vertex)
    {
        Assert.NonNullReference(vertex, nameof(vertex));
        yield return (UndirectedVertex)Enumerable.Empty<UndirectedVertex>();
    }

    ///// <summary>
    ///// Gets all vertices which are predecessors of the vertex.
    ///// </summary>
    ///// <param name="vertex"></param>
    //public abstract IEnumerable<TVertex> CollectPredecessors(TVertex vertex)
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

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
    }
}
