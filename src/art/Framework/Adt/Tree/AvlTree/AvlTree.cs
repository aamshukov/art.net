//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Tree;

/// <summary>
/// Adelson-Velsky and Landis Tree (AVL) - self-balancing binary search tree.
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class AvlTree<TKey> : BinaryTree<TKey>
     where TKey : IComparable<TKey>
{
    public AvlTree(id id,
                   Tree? papa = default,
                   string? label = default,
                   object? value = default,
                   Flags flags = Flags.Clear,
                   Color color = Color.Unknown,
                   Dictionary<string, object>? attributes = default,
                   string? version = default) : base(id, papa, label, value, flags, color, attributes, version)
    {
    }

    public new AvlTree<TKey>? Papa
    {
        get { return (AvlTree<TKey>?)base.Papa; }
        set { base.Papa = value; }
    }

    public new AvlTree<TKey>? Left
    {
        get { return (AvlTree<TKey>?)Kids[0]; }
        set { Kids[0] = value; }
    }

    public new AvlTree<TKey>? Right
    {
        get { return (AvlTree<TKey>?)Kids[1]; }
        set { Kids[1] = value; }
    }

    public AvlTree<TKey>? Uncle(AvlTree<TKey> node) => (AvlTree<TKey>?)(node?.Papa?.Papa?.Kids[1]);

    public static bool Insert(AvlTree<TKey> tree, AvlTree<TKey> root)
    {
        Assert.NonNullReference(tree);
        Assert.NonNullReference(root);

        // phase I (insert)
        return BinaryTree<TKey>.Insert(tree, root);

        // phase II (rebalance)
        //?? implement AVL tree logic.
    }

    public static AvlTree<TKey> Delete(TKey key, AvlTree<TKey> root)
    {
        Assert.NonNullReference(key);
        Assert.NonNullReference(root);

        //?? implement AVL tree logic.
        BinaryTree<TKey>.Delete(key, root);

        return root!;
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
        return visitor.Visit<TParam, TResult>(this, param);
    }
}
