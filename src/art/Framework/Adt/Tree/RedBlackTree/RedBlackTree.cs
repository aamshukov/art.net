//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Tree;

/// <summary>
/// Red–Black Tree - self-balancing binary search tree.
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class RedBlackTree<TKey> : BinaryTree<TKey>
     where TKey : IComparable<TKey>
{
    public RedBlackTree(id id,
                        Tree? papa = default,
                        string? label = default,
                        object? value = default,
                        Flags flags = Flags.Clear,
                        Color color = Color.Unknown,
                        Dictionary<string, object>? attributes = default,
                        string? version = default) : base(id, papa, label, value, flags, color, attributes, version)
    {
    }

    public static RedBlackTree<TKey> Insert(RedBlackTree<TKey> tree, RedBlackTree<TKey>? root)
    {
        Assert.NonNullReference(tree, nameof(tree));

        //?? implement RedBlack tree logic.
        BinaryTree<TKey>.Insert(tree, root);

        return root!;
    }

    public static RedBlackTree<TKey> Delete(TKey key, RedBlackTree<TKey>? root)
    {
        Assert.NonNullReference(key, nameof(key));

        //?? implement RedBlack tree logic.
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
        Assert.NonNullReference(visitor, nameof(visitor));
        return visitor.Visit<TParam, TResult>(this, param);
    }
}
