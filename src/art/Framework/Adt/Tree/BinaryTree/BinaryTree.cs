//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Tree;

public abstract class BinaryTree<TKey> : Tree
     where TKey: IComparable<TKey>
{
    public static readonly Tree Sentinel = new(id: 0, label: "BinaryTree:Sentinel", color: Color.Black);

    public BinaryTree(id id,
                      Tree? papa = default,
                      string? label = default,
                      object? value = default,
                      Flags flags = Flags.Clear,
                      Color color = Color.Unknown,
                      Dictionary<string, object>? attributes = default,
                      string? version = default) : base(id, papa, label, value, flags, color, attributes, version)
    {
    }

    public BinaryTree<TKey>? Left()
    {
        if(Kids.Count > 0)
            return (BinaryTree<TKey>?)Kids[0];
        return default;
    }

    public BinaryTree<TKey>? Right()
    {
        if(Kids.Count > 1)
            return (BinaryTree<TKey>?)Kids[1];
        return default;
    }

    public BinaryTree<TKey>? Search(TKey key, BinaryTree<TKey> root)
    {
        Assert.NonNullReference(key, nameof(key));
        Assert.NonNullReference(root, nameof(root));

        BinaryTree<TKey>? tree = root;

        while(tree is not null)
        {
            int compareKeysResult = CompareKeys(tree.GetKey(), key);

            if(compareKeysResult == 0)
            {
                return tree;
            }
            else if(compareKeysResult < 0)
            {
                tree = tree.Left();
            }
            else if(compareKeysResult > 0)
            {
                tree = tree.Right();
            }
        }

        return default;
    }

    public virtual BinaryTree<TKey> Insert(BinaryTree<TKey> tree, BinaryTree<TKey>? root)
    {
        Assert.NonNullReference(tree, nameof(tree));

        if(root is null)
        {
            return tree;
        }

        return root!;
    }

    public virtual BinaryTree<TKey> Delete(BinaryTree<TKey> tree, BinaryTree<TKey>? root)
    {
        Assert.NonNullReference(tree, nameof(tree));

        return root!;
    }

    protected abstract TKey GetKey();

    protected static int CompareKeys(TKey key1, TKey key2)
    {
        return key1.CompareTo(key2);
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
