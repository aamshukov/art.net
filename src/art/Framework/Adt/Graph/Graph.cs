//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using UILab.Art.Framework.Core.Counter;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public class Graph<TVertex, TEdge> : HyperGraph<TVertex, TEdge>
    where TVertex : Vertex
    where TEdge : HyperEdge<TVertex>
{
    public Dictionary<id, TEdge> Edges { get; init; }

    private Counter EdgeCounter { get; init; }

    public Graph(id id,
                 string? label = default,
                 Flags flags = Flags.Clear,
                 Color color = Color.Unknown,
                 Dictionary<string, object>? attributes = default,
                 string? version = default) : base(id, label, flags, color, Direction.Undirectional, attributes, version)
    {
        Edges = new();
        EdgeCounter = new();
    }

    public TVertex CreateVertex(string? label = default,
                                object? value = default,
                                Flags flags = Flags.Clear,
                                Color color = Color.Unknown,
                                Dictionary<string, object>? attributes = default,
                                bool digraph = false,
                                string? version = default)
    {
        if(digraph)
        {
            return (TVertex)Activator.CreateInstance(type: typeof(TVertex),
                                                     args: [VertexCounter.NextId(), label, default, default, value, flags, color, attributes, version])!;
        }
        else
        {
            return (TVertex)Activator.CreateInstance(type: typeof(TVertex),
                                                     args: [VertexCounter.NextId(), label, default, value, flags, color, attributes, version])!;
        }
    }

    //public TEdge CreateEdge(TVertex u,
    //                                 TVertex v,
    //                                 string? label = default,
    //                                 List<UndirectedVertex>? vertices = default,
    //                                 Flags flags = Flags.Clear,
    //                                 Dictionary<string, object>? attributes = default,
    //                                 string? version = default)
    //{
    //    return new(EdgeCounter.NextId(), label, vertices, flags, attributes, version);
    //}

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
    }
}
