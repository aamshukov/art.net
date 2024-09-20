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

    public void AddVertex(UndirectedVertex vertex)
    {
        Assert.NonNullReference(vertex, nameof(vertex));
        Assert.Ensure(!Vertices.ContainsKey(vertex.Id), nameof(vertex));

        Vertices.Add(vertex.Id, vertex);
        vertex.AddReference();
    }

    public UndirectedVertex? RemoveVertex(id id)
    {
        if(Vertices.Remove(id, out UndirectedVertex? vertex))
        {
            vertex.HyperEdges.Remove(Id); // unlink
            vertex.Release();
        }

        return vertex;
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
