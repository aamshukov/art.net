//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics.CodeAnalysis;
using UILab.Art.Framework.Core.Counter;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public class UndirectedHyperGraph : HyperGraph<UndirectedVertex, UndirectedHyperEdge>
{
    public Dictionary<id, UndirectedHyperEdge> HyperEdges { get; init; }

    protected Counter HyperEdgeCounter { get; init; }

    public event HyperEdgeEventHandler<UndirectedHyperEdge>? HyperEdgeAdded;

    public event HyperEdgeEventHandler<UndirectedHyperEdge>? HyperEdgeRemoved;

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
                                         List<UndirectedHyperEdge>? hyperEdges = default,
                                         object? value = default,
                                         Flags flags = Flags.Clear,
                                         Color color = Color.Unknown,
                                         Dictionary<string, object>? attributes = default,
                                         string? version = default)
    {
        return new(VertexCounter.NextId(), label, hyperEdges, value, flags, color, attributes, version);
    }

    public UndirectedVertex CloneVertex(UndirectedVertex vertex)
    {
        return new(VertexCounter.NextId(),
                   $"{vertex.Label}:cloned",
                   vertex.HyperEdges.Values.ToList(),
                   vertex.Value,
                   vertex.Flags | Flags.Synthetic,
                   vertex.Color,
                   vertex.Attributes,
                   vertex.Version);
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

    public void AddHyperEdge(UndirectedHyperEdge hyperEdge)
    {
        Assert.NonNullReference(hyperEdge, nameof(hyperEdge));

        HyperEdges.Add(hyperEdge.Id, hyperEdge);

        foreach(UndirectedVertex vertex in hyperEdge.Vertices.Values)
        {
            vertex.HyperEdges.Add(hyperEdge.Id, hyperEdge);
        }

        OnHyperEdgeAdd(hyperEdge);
    }

    protected virtual void OnHyperEdgeAdd(UndirectedHyperEdge hyperEdge)
    {
        Assert.NonNullReference(hyperEdge, nameof(hyperEdge));
        HyperEdgeAdded?.Invoke(hyperEdge);
    }

    public UndirectedHyperEdge? RemoveHyperEdge(id id, RemoveActionType removeActionType)
    {
        if(HyperEdges.Remove(id, out UndirectedHyperEdge? hyperEdge))
        {
            // release this hyperedge's self-vertices
            foreach(UndirectedVertex vertex in hyperEdge.Vertices.Values)
            {
                hyperEdge.RemoveVertex(vertex.Id);
            }

            // release this hyperedge from other vertices
            foreach(UndirectedVertex vertex in Vertices.Values)
            {
                if(vertex.HyperEdges.TryGetValue(hyperEdge.Id, out UndirectedHyperEdge? hyperEdgeToCleanup))
                {
                    if(ReferenceEquals(hyperEdgeToCleanup, hyperEdge))
                        continue;

                    hyperEdgeToCleanup.RemoveVertex(vertex.Id);
                }
            }

            if(removeActionType == RemoveActionType.Weak)
            {
                // Weak-deleting an edge only removes that edge from the hypergraph.
                // The vertices in the deleted edge remain part of the hypergraph.
                // No other changes are made to the remaining edges or vertices.
                // DO NOTHING!
            }
            else if(removeActionType == RemoveActionType.Strong)
            {
                // Strong-delete removes a hyperedge and weakly deletes all the vertices incident with this hyperedge.
                // A vertex is incident with a hyperedge if it is one of the vertices that the hyperedge contains.
                foreach(UndirectedVertex vertex in Vertices.Values)
                {
                    if(vertex.HyperEdges.TryGetValue(hyperEdge.Id, out UndirectedHyperEdge? hyperEdgeToCleanup))
                    {
                        if(ReferenceEquals(hyperEdgeToCleanup, hyperEdge))
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

    protected virtual void OnHyperEdgeRemoved(UndirectedHyperEdge hyperEdge)
    {
        Assert.NonNullReference(hyperEdge, nameof(hyperEdge));
        HyperEdgeRemoved?.Invoke(hyperEdge);
    }

    public UndirectedVertex? RemoveVertex(id id, RemoveActionType removeActionType)
    {
        if(Vertices.TryGetValue(id, out UndirectedVertex? vertex))
        {
            RemoveVertexInternal(id, removeActionType);
        }

        return vertex;
    }

    private void RemoveVertexInternal(id id, RemoveActionType removeActionType)
    {
        if(removeActionType == RemoveActionType.Weak)
        {
            // In an undirected hypergraph, weak vertex deletion refers to the removal of a vertex from the hypergraph
            // along with the removal of the vertex from any hyperedges that contain it.
            foreach(UndirectedHyperEdge hyperEdge in HyperEdges.Values)
            {
                hyperEdge.RemoveVertex(id);
            }

            // However, the remaining part of the hyperedge (if non-empty) remains in the hypergraph.
            foreach(UndirectedHyperEdge hyperEdge in HyperEdges.Values)
            {
                if(hyperEdge.Vertices.Count == 0)
                {
                    RemoveHyperEdge(hyperEdge.Id, RemoveActionType.Weak);
                }
            }
        }
        else if(removeActionType == RemoveActionType.Strong)
        {
            // 1. ... strong deletion of v removes v and all edges that are incident to v from the hypergraph.
            // 2. ... in a general (undirected) hypergraph, strong vertex deletion refers to the removal of a vertex
            //        from the hypergraph along with any hyperedges that contain that vertex.
            foreach(UndirectedHyperEdge hyperEdge in HyperEdges.Values)
            {
                if(hyperEdge.Vertices.ContainsKey(id))
                {
                    RemoveHyperEdge(hyperEdge.Id, RemoveActionType.Weak);
                }
            }
        }
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
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
