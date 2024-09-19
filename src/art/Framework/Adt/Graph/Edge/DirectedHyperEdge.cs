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
        Domain = domain?.ToDictionary(kvp => kvp.Id, kvp => kvp) ?? new();
        Domain.Values.ToList().ForEach(vertex => vertex.AddReference());

        Codomain = codomain?.ToDictionary(kvp => kvp.Id, kvp => kvp) ?? new();
        Codomain.Values.ToList().ForEach(vertex => vertex.AddReference());
    }

    public DirectedVertex? GetVertex(id id, bool domain = true)
    {
        var vertices = domain ? Domain : Codomain;

        if(vertices.TryGetValue(id, out DirectedVertex? vertex))
            return vertex;

        return default;
    }

    public bool TryGetVertex(id id, out DirectedVertex? vertex, bool domain = true)
    {
        var vertices = domain ? Domain : Codomain;
        return vertices.TryGetValue(id, out vertex);
    }

    public void AddVertex(DirectedVertex vertex, bool domain = true)
    {
        Assert.NonNullReference(vertex, nameof(vertex));

        var vertices = domain ? Domain : Codomain;
        Assert.Ensure(!vertices.ContainsKey(vertex.Id), nameof(vertex));

        vertices.Add(vertex.Id, vertex);
        vertex.AddReference();
    }

    public bool TryAddVertex(DirectedVertex vertex, bool domain = true)
    {
        Assert.NonNullReference(vertex, nameof(vertex));

        var vertices = domain ? Domain : Codomain;
        bool result = !vertices.ContainsKey(vertex.Id);

        if(result)
        {
            vertices.Add(vertex.Id, vertex);
            vertex.AddReference();
        }

        return result;
    }

    public DirectedVertex? RemoveVertex(id id, bool domain = true)
    {
        var vertices = domain ? Domain : Codomain;

        if(vertices.Remove(id, out DirectedVertex? vertex))
            vertex.Release();

        return vertex;
    }

    public bool TryRemoveVertex(id id, out DirectedVertex? vertex, bool domain = true)
    {
        var vertices = domain ? Domain : Codomain;

        if(vertices.Remove(id, out vertex))
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
            foreach(DirectedVertex vertex in Domain.Values)
            {
                vertex.OutHyperEdges.Add(Id, this);
            }

            foreach(DirectedVertex vertex in Codomain.Values)
            {
                vertex.InHyperEdges.Add(Id, this);
            }
        }
        else
        {
            foreach(DirectedVertex vertex in Domain.Values)
            {
                vertex.OutHyperEdges.Remove(Id);
            }

            foreach(DirectedVertex vertex in Codomain.Values)
            {
                vertex.InHyperEdges.Remove(Id);
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
