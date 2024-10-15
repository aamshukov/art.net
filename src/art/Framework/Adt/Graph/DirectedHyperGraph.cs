//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics.CodeAnalysis;
using UILab.Art.Framework.Core.Counter;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public class DirectedHyperGraph : HyperGraph<DirectedVertex, DirectedHyperEdge>
{
    public Dictionary<id, DirectedHyperEdge> HyperEdges { get; init; }

    protected Counter HyperEdgeCounter { get; init; }

    public event HyperEdgeEventHandler<DirectedHyperEdge>? HyperEdgeAdded;

    public event HyperEdgeEventHandler<DirectedHyperEdge>? HyperEdgeRemoved;

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
                                       List<DirectedHyperEdge>? inHyperEdges = default,
                                       List<DirectedHyperEdge>? outHyperEdges = default,
                                       object? value = default,
                                       Flags flags = Flags.Clear,
                                       Color color = Color.Unknown,
                                       Dictionary<string, object>? attributes = default,
                                       string? version = default)
    {
        Assert.NonDisposed(Disposed);
        return new(VertexCounter.NextId(), label, inHyperEdges, outHyperEdges, value, flags, color, attributes, version);
    }

    public DirectedVertex CloneVertex(DirectedVertex vertex)
    {
        Assert.NonDisposed(Disposed);

        return new(VertexCounter.NextId(),
                   $"{vertex.Label}:cloned",
                   vertex.InHyperEdges.Values.ToList(),
                   vertex.OutHyperEdges.Values.ToList(),
                   vertex.Value,
                   vertex.Flags | Flags.Synthetic,
                   vertex.Color,
                   vertex.Attributes,
                   vertex.Version);
    }

    public DirectedHyperEdge CreateHyperEdge(string? label = default,
                                             List<DirectedVertex>? domain = default,
                                             List<DirectedVertex>? codomain = default,
                                             Flags flags = Flags.Clear,
                                             Dictionary<string, object>? attributes = default,
                                             string? version = default)
    {
        Assert.NonDisposed(Disposed);
        return new(HyperEdgeCounter.NextId(), label, domain, codomain, flags, attributes, version);
    }

    public DirectedHyperEdge? GetHyperEdge(id id)
    {
        Assert.NonDisposed(Disposed);

        if(HyperEdges.TryGetValue(id, out DirectedHyperEdge? hyperEdge))
            return hyperEdge;
        return default;
    }

    public void AddHyperEdge(DirectedHyperEdge hyperEdge)
    {
        Assert.NonDisposed(Disposed);
        Assert.NonNullReference(hyperEdge, nameof(hyperEdge));

        HyperEdges.Add(hyperEdge.Id, hyperEdge);

        foreach(DirectedVertex vertex in hyperEdge.Domain.Values)
        {
            vertex.OutHyperEdges.Add(hyperEdge.Id, hyperEdge);
        }

        foreach(DirectedVertex vertex in hyperEdge.Codomain.Values)
        {
            vertex.InHyperEdges.Add(hyperEdge.Id, hyperEdge);
        }

        OnHyperEdgeAdd(hyperEdge);
    }

    protected virtual void OnHyperEdgeAdd(DirectedHyperEdge hyperEdge)
    {
        Assert.NonDisposed(Disposed);
        Assert.NonNullReference(hyperEdge, nameof(hyperEdge));

        HyperEdgeAdded?.Invoke(hyperEdge);
    }

    public DirectedHyperEdge? RemoveHyperEdge(id id, RemoveActionType removeActionType)
    {
        Assert.NonDisposed(Disposed);

        if(HyperEdges.Remove(id, out DirectedHyperEdge? hyperEdge))
        {
            // release this hyperedge's self-vertices
            foreach(DirectedVertex vertex in hyperEdge.Domain.Values)
            {
                hyperEdge.RemoveVertex(vertex.Id, domain: true);
            }

            foreach(DirectedVertex vertex in hyperEdge.Codomain.Values)
            {
                hyperEdge.RemoveVertex(vertex.Id, domain: false);
            }

            // release this hyperedge from other vertices
            foreach(DirectedVertex vertex in Vertices.Values)
            {
                if(vertex.InHyperEdges.TryGetValue(hyperEdge.Id, out DirectedHyperEdge? inHyperEdgeToCleanup))
                {
                    if(ReferenceEquals(inHyperEdgeToCleanup, hyperEdge))
                        continue;

                    inHyperEdgeToCleanup.RemoveVertex(vertex.Id, domain: false);
                }

                if(vertex.OutHyperEdges.TryGetValue(hyperEdge.Id, out DirectedHyperEdge? outHyperEdgeToCleanup))
                {
                    if(ReferenceEquals(outHyperEdgeToCleanup, hyperEdge))
                        continue;

                    outHyperEdgeToCleanup.RemoveVertex(vertex.Id, domain: true);
                }
            }

            if(removeActionType == RemoveActionType.Weak)
            {
                // Weak-deleting an edge only removes that edge from the hypergraph.
                // The vertices in the deleted edge (domain and codomain) remain part of the hypergraph.
                // No other changes are made to the remaining edges or vertices.
                // DO NOTHING!
            }
            else if(removeActionType == RemoveActionType.Strong)
            {
                // Strong-delete removes a hyperedge and all the vertices (domain and codomain) incident with hyperedge.
                // A vertex is incident with a hyperedge if it is one of the vertices that the hyperedge contains.
                foreach(DirectedVertex vertex in Vertices.Values)
                {
                    if(vertex.InHyperEdges.TryGetValue(hyperEdge.Id, out DirectedHyperEdge? inHyperEdgeToCleanup))
                    {
                        if(ReferenceEquals(inHyperEdgeToCleanup, hyperEdge))
                            continue;

                        RemoveVertex(vertex.Id, RemoveActionType.Weak);
                    }

                    if(vertex.OutHyperEdges.TryGetValue(hyperEdge.Id, out DirectedHyperEdge? outHyperEdgeToCleanup))
                    {
                        if(ReferenceEquals(outHyperEdgeToCleanup, hyperEdge))
                            continue;

                        RemoveVertex(vertex.Id, RemoveActionType.Weak);
                    }
                }

                Cleanup();
            }

            OnHyperEdgeRemoved(hyperEdge);
        }

        return hyperEdge;
    }

    protected virtual void OnHyperEdgeRemoved(DirectedHyperEdge hyperEdge)
    {
        Assert.NonNullReference(hyperEdge, nameof(hyperEdge));
        HyperEdgeRemoved?.Invoke(hyperEdge);
    }

    public DirectedVertex? RemoveVertex(id id, RemoveActionType removeActionType)
    {
        Assert.NonDisposed(Disposed);

        if(Vertices.TryGetValue(id, out DirectedVertex? vertex))
        {
            RemoveVertexInternal(id, removeActionType);
        }

        return vertex;
    }

    private void RemoveVertexInternal(id id, RemoveActionType removeActionType)
    {
        if(removeActionType == RemoveActionType.Weak)
        {
            // 1. ... removes the vertex and keeps the remainder of the hyperedge, but since hyperedges have
            //        source and target sets, the vertex is removed from both sets, ...
            foreach(DirectedHyperEdge hyperEdge in HyperEdges.Values)
            {
                hyperEdge.RemoveVertex(id, domain: true);
                hyperEdge.RemoveVertex(id, domain: false);
            }

            // ... and the hyperedge is retained only if both the updated source and target sets are non-empty.
            foreach(DirectedHyperEdge hyperEdge in HyperEdges.Values)
            {
                if(hyperEdge.Domain.Count == 0 || hyperEdge.Codomain.Count == 0)
                {
                    RemoveHyperEdge(hyperEdge.Id, RemoveActionType.Weak);
                }
            }
        }
        else if(removeActionType == RemoveActionType.Strong)
        {
            // 1. ... removes the vertex and any hyperedges where the vertex appears either in the source set or the target set.
            foreach(DirectedHyperEdge hyperEdge in HyperEdges.Values)
            {
                if(hyperEdge.Domain.ContainsKey(id) || hyperEdge.Codomain.ContainsKey(id))
                {
                    RemoveHyperEdge(hyperEdge.Id, RemoveActionType.Weak);
                }
            }
        }
    }

    /// <summary>
    /// Gets all vertices which are connected to the vertex via any edges.
    /// </summary>
    /// <param name="vertex"></param>
    public IEnumerable<UndirectedVertex> GetNeighbors(UndirectedVertex vertex)
    {
        Assert.NonDisposed(Disposed);
        Assert.NonNullReference(vertex, nameof(vertex));

        yield return (UndirectedVertex)Enumerable.Empty<UndirectedVertex>();
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        Assert.NonDisposed(Disposed);

        foreach(var component in base.GetEqualityComponents())
            yield return component;
    }

    [SuppressMessage("Minor Code Smell", "S2486:Generic exceptions should not be ignored", Justification = "<Pending>")]
    [SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "<Pending>")]
    [SuppressMessage("Major Code Smell", "S1066:Mergeable \"if\" statements should be combined", Justification = "<Pending>")]
    protected override void Dispose(bool disposing)
    {
        if(!Disposed)
        {
            lock(syncRoot)
            {
                try
                {
                    if(!Disposed)
                    {
                        // managed resources
                        if(disposing)
                        {
                            if(HyperEdgeAdded is not null)
                            {
                                Delegate.RemoveAll(HyperEdgeAdded, HyperEdgeAdded);
                                HyperEdgeAdded = null;
                            }

                            if(HyperEdgeRemoved is not null)
                            {
                                Delegate.RemoveAll(HyperEdgeRemoved, HyperEdgeRemoved);
                                HyperEdgeRemoved = null;
                            }
                        }

                        // unmanaged resources
                    }
                }
                catch
                {
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
