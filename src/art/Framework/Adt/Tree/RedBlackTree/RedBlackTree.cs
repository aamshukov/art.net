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

    public new RedBlackTree<TKey>? Papa
    {
        get { return (RedBlackTree<TKey>?)base.Papa; }
        set { base.Papa = value; }
    }

    public new RedBlackTree<TKey>? Left
    {
        get { return (RedBlackTree<TKey>?)Kids[0]; }
        set { Kids[0] = value; }
    }

    public new RedBlackTree<TKey>? Right
    {
        get { return (RedBlackTree<TKey>?)Kids[1]; }
        set { Kids[1] = value; }
    }

    public RedBlackTree<TKey>? Uncle(RedBlackTree<TKey> node) => (RedBlackTree<TKey>?)(node?.Papa?.Papa?.Kids[1]);

    public bool IsRed() => Color == Color.Red;

    public bool IsBlack() => Color == Color.Black;

    public static bool Insert(RedBlackTree<TKey> tree, RedBlackTree<TKey> root)
    {
        Assert.NonNullReference(tree);
        Assert.NonNullReference(root);

        // phase I (insert)
        bool result = BinaryTree<TKey>.Insert(tree, root);

        // phase II (rebalance)
        if(result)
        {
            Rebalance(tree);
        }

        return result;
    }

    public static RedBlackTree<TKey> Delete(TKey key, RedBlackTree<TKey> root)
    {
        Assert.NonNullReference(key);
        Assert.NonNullReference(root);

        //?? implement RedBlack tree logic.
        BinaryTree<TKey>.Delete(key, root);

        return root!;
    }

    public bool CheckInvariant(RedBlackTree<TKey> tree)
    {
        return false;
    }

    private static void Rebalance(RedBlackTree<TKey> tree)
    {
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
