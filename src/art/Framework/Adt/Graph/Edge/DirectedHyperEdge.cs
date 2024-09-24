//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain.Abstractions;

namespace UILab.Art.Framework.Adt.Graph;

public class DirectedHyperEdge : HyperEdge<DirectedVertex>
{
    /// <summary>
    /// Set of vertices: domain, tail, sender, source, etc.
    /// </summary>
    public Dictionary<id, DirectedVertex> Domain { get; init; }

    /// <summary>
    /// Set of vertices: codomain, head, receiver, sink, target, etc.
    /// </summary>
    public Dictionary<id, DirectedVertex> Codomain { get; init; }

    public DirectedHyperEdge(id id,
                             string? label = default,
                             List<DirectedVertex>? domain = default,
                             List<DirectedVertex>? codomain = default,
                             Flags flags = Flags.Clear,
                             Dictionary<string, object>? attributes = default,
                             string? version = default) : base(id, label, flags, attributes, version)
    {
        Domain = domain?.Where(v => v is not null).ToDictionary(kvp => kvp.Id, kvp => kvp) ?? new();
        Domain.Values.ToList().ForEach(vertex => vertex.AddReference());

        Codomain = codomain?.Where(v => v is not null).ToDictionary(kvp => kvp.Id, kvp => kvp) ?? new();
        Codomain.Values.ToList().ForEach(vertex => vertex.AddReference());
    }

    public DirectedVertex? GetVertex(id id, bool domain = true)
    {
        var vertices = domain ? Domain : Codomain;

        if(vertices.TryGetValue(id, out DirectedVertex? vertex))
            return vertex;

        return default;
    }

    public void AddVertex(DirectedVertex vertex, bool domain = true)
    {
        Assert.NonNullReference(vertex, nameof(vertex));

        var vertices = domain ? Domain : Codomain;
        Assert.Ensure(!vertices.ContainsKey(vertex.Id), nameof(vertex));

        vertices.Add(vertex.Id, vertex);
        vertex.AddReference();
    }

    public DirectedVertex? RemoveVertex(id id, bool domain = true)
    {
        var vertices = domain ? Domain : Codomain;

        if(vertices.Remove(id, out DirectedVertex? vertex))
        {
            var hyperEdges = domain ? vertex.OutHyperEdges : vertex.InHyperEdges;

            hyperEdges.Remove(Id); // unlink
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
