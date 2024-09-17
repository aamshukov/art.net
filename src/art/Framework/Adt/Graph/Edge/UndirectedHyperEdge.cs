//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain.Abstractions;

namespace UILab.Art.Framework.Adt.Graph;

public class UndirectedHyperEdge : HyperEdge<UndirectedVertex>
{
    public Dictionary<id, UndirectedVertex> Vertices { get; init; }

    public UndirectedHyperEdge(id id,
                               string? label = default,
                               List<UndirectedVertex>? vertices = default,
                               Flags flags = Flags.Clear,
                               Dictionary<string, object>? attributes = default,
                               string? version = default) : base(id, label, flags, attributes, version)
    {
        Vertices = vertices?.ToDictionary(kvp => kvp.Id, kvp => kvp) ?? new();
        Vertices.Values.ToList().ForEach(vertex => vertex.AddReference());
    }

    public UndirectedVertex? GeVertex(id id)
    {
        if(Vertices.TryGetValue(id, out UndirectedVertex? vertex))
            return vertex;
        return default;
    }

    public bool TryGetVertex(id id, out UndirectedVertex? vertex)
    {
        return Vertices.TryGetValue(id, out vertex);
    }

    public void AddVertex(UndirectedVertex vertex)
    {
        Assert.NonNullReference(vertex, nameof(vertex));
        Assert.Ensure(!Vertices.ContainsKey(vertex.Id), nameof(vertex));

        Vertices.Add(vertex.Id, vertex);
        vertex.AddReference();
    }

    public bool TryAddVertex(UndirectedVertex vertex)
    {
        Assert.NonNullReference(vertex, nameof(vertex));

        bool result = !Vertices.ContainsKey(vertex.Id);

        if(result)
        {
            Vertices.Add(vertex.Id, vertex);
            vertex.AddReference();
        }

        return result;
    }

    public UndirectedVertex? RemoveVertex(id id)
    {
        if(Vertices.Remove(id, out UndirectedVertex? vertex))
            vertex.Release();
        return vertex;
    }

    public bool TryRemoveVertex(id id, out UndirectedVertex? vertex)
    {
        if(Vertices.Remove(id, out vertex))
        {
            vertex.Release();
            return true;
        }

        return false;
    }

    public void UpdateDependencies(bool link)
    {
        if(link)
        {
            foreach(UndirectedVertex vertex in Vertices.Values)
            {
                vertex.HyperEdges.Add(Id, this);
            }
        }
        else
        {
            foreach(UndirectedVertex vertex in Vertices.Values)
            {
                vertex.HyperEdges.Remove(Id);
            }
        }
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
    }

    public override TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param = default)
        where TResult : default
        where TParam : default
    {
        Assert.NonNullReference(visitor, nameof(visitor));
        return visitor.Visit<TParam, TResult>(this, param);
    }
}
