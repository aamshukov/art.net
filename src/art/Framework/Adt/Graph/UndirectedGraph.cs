//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public class UndirectedGraph : UndirectedHyperGraph
{
    public UndirectedGraph(id id,
                           string? label = default,
                           Flags flags = Flags.Clear,
                           Color color = Color.Unknown,
                           Dictionary<string, object>? attributes = default,
                           string? version = default) : base(id, label, flags, color, attributes, version)
    {
    }

    public UndirectedEdge CreateEdge(UndirectedVertex u,
                                     UndirectedVertex v,
                                     string? label = default,
                                     Flags flags = Flags.Clear,
                                     Dictionary<string, object>? attributes = default,
                                     string? version = default)
    {
        Assert.NonNullReference(u, nameof(u));
        Assert.NonNullReference(v, nameof(v));

        return new(HyperEdgeCounter.NextId(),  u, v, label, flags, attributes, version);
    }

    public UndirectedEdge? GetEdge(id id)
    {
        return (UndirectedEdge?)base.GetHyperEdge(id);
    }

    public void AddEdge(UndirectedEdge edge)
    {
        Assert.NonNullReference(edge, nameof(edge));
        base.AddHyperEdge(edge);
    }

    public UndirectedEdge? RemoveEdge(id id, RemoveActionType removeActionType = RemoveActionType.Weak)
    {
        return (UndirectedEdge?)base.RemoveHyperEdge(id, removeActionType);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
    }
}
