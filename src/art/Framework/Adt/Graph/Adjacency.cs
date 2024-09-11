//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

/// <summary>
/// Represents an endpoint which is edge with head (H) and tail (T) sets of vertices according to direction.
/// </summary>
/// <typeparam name="TVertex"></typeparam>
/// <typeparam name="TEdge"></typeparam>
public sealed class Adjacency<TVertex, TEdge>
    where TVertex : class
    where TEdge : class
{
    /// <summary>
    /// Edge, which connects head (H) and tail (T) sets of vertices according to direction.
    /// </summary>
    public TEdge Edge { get; init; }

    /// <summary>
    /// Set of vetices.
    /// U 'vertex'.
    /// </summary>
    public List<TVertex> Head { get; init; }

    /// <summary>
    /// Set of vetices.
    /// V 'vertex'.
    /// </summary>
    public List<TVertex> Tail { get; init; }

    /// <summary>
    /// Indicates how to interpret relation, directed(digraph), undirected, etc.
    /// </summary>
    public Direction Direction { get; init; }

    public Adjacency(TEdge edge,
                     List<TVertex> head,
                     List<TVertex> tail,
                     Direction direction = Direction.Undirectional)
    {
        Assert.NonNullReference(edge, nameof(edge));
        Assert.NonEmptyCollection(head, nameof(head));
        Assert.NonEmptyCollection(tail, nameof(tail));

        Edge = edge;

        Head = head;
        Tail = tail;

        Direction = direction;
    }
}
