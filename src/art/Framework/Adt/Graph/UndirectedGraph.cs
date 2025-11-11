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
        return new(HyperEdgeCounter.NextId(), u, v, label, flags, attributes, version);
    }

    public UndirectedEdge? GetEdge(id id)
    {
        Assert.NonDisposed(Disposed);
        return (UndirectedEdge?)base.GetHyperEdge(id);
    }

    public void AddEdge(UndirectedEdge edge)
    {
        Assert.NonDisposed(Disposed);
        Assert.NonNullReference(edge);

        base.AddHyperEdge(edge);
    }

    public UndirectedEdge? RemoveEdge(id id, RemoveActionType removeActionType = RemoveActionType.Weak)
    {
        Assert.NonDisposed(Disposed);
        return (UndirectedEdge?)base.RemoveHyperEdge(id, removeActionType);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        Assert.NonDisposed(Disposed);

        foreach(var component in base.GetEqualityComponents())
            yield return component;
    }
}
