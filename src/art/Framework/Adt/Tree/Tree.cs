//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Tree;

public class Tree : Vertex
{
    public Tree? Papa { get; set; }

    public List<Tree> Kids { get; init; }

    public Tree(id id,
                Tree? papa = default,
                string? label = default,
                object? value = default,
                Flags flags = Flags.Clear,
                Color color = Color.Unknown,
                Dictionary<string, object>? attributes = default,
                string? version = default) : base(id, label, value, flags, color, attributes, version)
    {
        Papa = papa;
        Kids = new();
    }

    public void AddKid(Tree kid)
    {
        Assert.NonNullReference(kid, nameof(kid));

        kid.Papa = this;
        Kids.Add(kid);
    }

    public void InsertKid(index index, Tree kid)
    {
        Assert.NonNullReference(kid, nameof(kid));

        kid.Papa = this;
        Kids.Insert(index, kid);
    }

    public void RemoveKid(Tree kid)
    {
        Assert.NonNullReference(kid, nameof(kid));

        kid.Papa = default;
        Kids.Remove(kid);
    }

    public void RemoveKidAt(index index)
    {
        Assert.Ensure(0 <= index && index < Kids.Count, nameof(index));

        Kids[index].Papa = default;
        Kids.RemoveAt(index);
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
