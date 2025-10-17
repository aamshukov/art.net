//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Tree;

/// <summary>
/// Splay Tree - BST with the additional property that recently accessed elements are quick to access again.
/// https://en.wikipedia.org/wiki/Splay_tree
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class SplayTree<TKey> : BinaryTree<TKey>
     where TKey : IComparable<TKey>
{
    public SplayTree(id id,
                     Tree? papa = default,
                     string? label = default,
                     object? value = default,
                     Flags flags = Flags.Clear,
                     Color color = Color.Unknown,
                     Dictionary<string, object>? attributes = default,
                     string? version = default) : base(id, papa, label, value, flags, color, attributes, version)
    {
    }

    public new SplayTree<TKey>? Papa
    {
        get { return (SplayTree<TKey>?)base.Papa; }
        set { base.Papa = value; }
    }

    public new SplayTree<TKey>? Left
    {
        get { return (SplayTree<TKey>?)Kids[0]; }
        set { Kids[0] = value; }
    }

    public new SplayTree<TKey>? Right
    {
        get { return (SplayTree<TKey>?)Kids[1]; }
        set { Kids[1] = value; }
    }

    public static bool Insert(SplayTree<TKey> tree, SplayTree<TKey> root)
    {
        Assert.NonNullReference(tree);
        Assert.NonNullReference(root);

        // phase I (insert)
        return BinaryTree<TKey>.Insert(tree, root);

        // phase II (rebalance)
        //?? implement SplayTree tree logic.
    }

    public static SplayTree<TKey> Delete(TKey key, SplayTree<TKey> root)
    {
        Assert.NonNullReference(key);
        Assert.NonNullReference(root);

        //?? implement SplayTree tree logic.
        BinaryTree<TKey>.Delete(key, root);

        return root!;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
        yield return Color;
    }

    public override TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param = default)
        where TResult : default
        where TParam : default
    {
        Assert.NonNullReference(visitor);
        return visitor.Visit<TParam, TResult>(this, param);
    }
}
