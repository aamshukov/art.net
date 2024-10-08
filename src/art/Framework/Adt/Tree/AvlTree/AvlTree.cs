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
     where TKey: IComparable<TKey>
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

    public override BinaryTree<TKey> Insert(BinaryTree<TKey> tree, BinaryTree<TKey>? root)
    {
        Assert.NonNullReference(tree, nameof(tree));

        base.Insert(tree, root);

        return root!;
    }

    public override BinaryTree<TKey> Delete(BinaryTree<TKey> tree, BinaryTree<TKey>? root)
    {
        Assert.NonNullReference(tree, nameof(tree));

        base.Delete(tree, root);

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
        Assert.NonNullReference(visitor, nameof(visitor));
        return visitor.Visit<TParam, TResult>(this, param);
    }
}
