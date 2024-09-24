//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public class DirectedGraph : DirectedHyperGraph
{
    public DirectedGraph(id id,
                         string? label = default,
                         Flags flags = Flags.Clear,
                         Color color = Color.Unknown,
                         Dictionary<string, object>? attributes = default,
                         string? version = default) : base(id, label, flags, color, attributes, version)
    {
    }

    public DirectedEdge CreateEdge(DirectedVertex? u = default,
                                   DirectedVertex? v = default,
                                   string? label = default,
                                   Flags flags = Flags.Clear,
                                   Dictionary<string, object>? attributes = default,
                                   string? version = default)
    {
        return new(HyperEdgeCounter.NextId(), u, v, label, flags, attributes, version);
    }

    public DirectedEdge? GetEdge(id id)
    {
        return (DirectedEdge?)base.GetHyperEdge(id);
    }

    public void AddEdge(DirectedEdge edge)
    {
        Assert.NonNullReference(edge, nameof(edge));
        base.AddHyperEdge(edge);
    }

    public DirectedEdge? RemoveEdge(id id, RemoveActionType removeActionType = RemoveActionType.Weak)
    {
        return (DirectedEdge?)base.RemoveHyperEdge(id, removeActionType);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
    }
}
