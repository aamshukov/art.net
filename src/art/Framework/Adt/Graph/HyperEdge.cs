//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;
using UILab.Art.Framework.Core.Domain.Abstractions;

namespace UILab.Art.Framework.Adt.Graph;

/// <summary>
/// Represents an endpoint which is edge with head (H) and tail (T) sets of vertices according to direction.
/// </summary>
/// <typeparam name="TVertex"></typeparam>
/// <typeparam name="TEdge"></typeparam>
public sealed class HyperEdge<TVertex, TEdge> : EntityType<id>, IVisitable
    where TVertex : EntityType<id>
    where TEdge : EntityType<id>
{
    /// <summary>
    /// Edge, which connects domain (D) and codomain (C) sets of vertices according to direction.
    /// </summary>
    public TEdge Edge { get; init; }

    /// <summary>
    /// Set of vertices: head, source, etc.
    /// U 'vertex'.
    /// If Direction is Undirectional all vertices are in the Domain only and Codomain is not considered.
    /// If Direction is Directional or Bidirectional vertices are in Domain and in Codomain.
    /// </summary>
    public List<TVertex> Domain { get; init; }

    /// <summary>
    /// Set of vertices: tail, target, etc.
    /// V 'vertex'.
    /// </summary>
    public List<TVertex> Codomain { get; init; }

    /// <summary>
    /// Gets the first element of the head, mimics graph's endpoint.
    /// </summary>
    public TVertex? U
    {
        get
        {
            TVertex? u = default;

            if(Domain.Count > 1)
            {
                u = Direction switch
                {
                     Direction.Undirectional => Domain[0],
                     Direction.Directional => Domain[0],
                     Direction.Bidirectional => Domain[0],
                     _ => default
                };
            }

            return u;
        }
    }

    /// <summary>
    /// Gets the first element of the tail, mimics graph's endpoint.
    /// </summary>
    public TVertex? V
    {
        get
        {
            TVertex? v = default;

            if(Codomain.Count > 1)
            {
                v = Direction switch
                {
                     Direction.Undirectional => Codomain[0],
                     Direction.Directional => Codomain[0],
                     Direction.Bidirectional => Codomain[0],
                     _ => default
                };
            }

            return v;
        }
    }

    /// <summary>
    /// Indicates how to interpret relation, directed(digraph), undirected, etc.
    /// </summary>
    public Direction Direction { get; init; }

    public HyperEdge(id id,
                     TEdge edge,
                     List<TVertex>? domain = default,
                     List<TVertex>? codomain = default,
                     Direction direction = Direction.Undirectional,
                     string? version = default) : base(id, version)
    {
        Assert.NonNullReference(edge, nameof(edge));

        Edge = edge;

        Domain = domain ?? new();
        Codomain = codomain ?? new();

        Direction = direction;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
    }

    public TResult? Accept<TParam, TResult>(IVisitor<TParam, TResult> visitor, TParam? param = default)
    {
        Assert.NonNullReference(visitor, nameof(visitor));
        return visitor.Visit(this, param);
    }
}
