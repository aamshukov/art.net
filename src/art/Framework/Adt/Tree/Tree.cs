//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics;
using UILab.Art.Framework.Adt.Graph;
using UILab.Art.Framework.Core.DesignPatterns.Observer;
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Tree;

[DebuggerDisplay("Label = " + "{Label}")]
public class Tree : Vertex
{
    public Tree? Papa { get; set; }

    public List<Tree?> Kids { get; init; }

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

    public void AddKid(Tree? kid)
    {
        if(kid is not null)
            kid.Papa = this;

        Kids.Add(kid);
    }

    public void InsertKid(index index, Tree? kid)
    {
        if(kid is not null)
            kid.Papa = this;

        Kids.Insert(index, kid);
    }

    public void RemoveKid(Tree? kid)
    {
        if(kid is not null)
            kid.Papa = default;

        Kids.Remove(kid);
    }

    public void RemoveKidAt(index index)
    {
        Assert.Ensure(0 <= index && index < Kids.Count, nameof(index));

        Tree? kid = Kids[index];

        if(kid is not null)
            kid.Papa = default;

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
        Assert.NonNullReference(visitor);

        TResult? result = default;

        Queue<Tree> queue = new(); // queue - in reverse order for left-to-right traversal

        queue.Enqueue(this);

        while(queue?.Count > 0)
        {
            Tree tree = queue.Dequeue();

            result = visitor.Visit<TParam, TResult>(tree, param);

            foreach(Tree? kid in tree.Kids)
            {
                if(kid is not null)
                    queue?.Enqueue(kid);
            }
        }

        // returns the last obtained result
        return result;
    }
}
