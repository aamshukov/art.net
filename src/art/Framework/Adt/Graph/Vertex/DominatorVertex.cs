//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Graph;

public class DominatorVertex : DirectedVertex
{
    public DominatorVertex? IDominator { get; set; }

    public List<DominatorVertex> Dominators { get; init; }

    public List<DominatorVertex> Frontiers { get; init; }

    public DominatorVertex(id id,
                           DominatorVertex? idominator = default,
                           string? label = default,
                           object? value = default,
                           Flags flags = Flags.Clear,
                           Color color = Color.Unknown,
                           Dictionary<string, object>? attributes = default,
                           string? version = default) : base(id: id,
                                                             label: label,
                                                             value: value,
                                                             flags: flags,
                                                             color: color,
                                                             attributes: attributes,
                                                             version: version)
    {
        IDominator = idominator;

        Dominators = new();
        Frontiers = new();
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
