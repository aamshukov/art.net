//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Adt.Graph;

public class Graph<TVertex, TEdge> : Hypergraph<TVertex, TEdge>
    where TVertex : class
    where TEdge : class
{
    public Graph(id id,
                 string? label = default,
                 Flags flags = Flags.Clear,
                 Color color = Color.Unknown,
                 string? version = default) : base(id, label, flags, color, version)
    {
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
    }
}
