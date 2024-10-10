//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Adt.Tree;

public abstract class BinaryTree<TKey> : Tree
     where TKey : IComparable<TKey>
{
    public BinaryTree(id id,
                      Tree? papa = default,
                      string? label = default,
                      object? value = default,
                      Flags flags = Flags.Clear,
                      Color color = Color.Unknown,
                      Dictionary<string, object>? attributes = default,
                      string? version = default) : base(id, papa, label, value, flags, color, attributes, version)
    {
        AddKid(Sentinel); // left
        AddKid(Sentinel); // right
    }

    public BinaryTree<TKey> Left
    {
        get { return (BinaryTree<TKey>)Kids[0]; }
        set { Kids[0] = value; }
    }

    public BinaryTree<TKey> Right
    {
        get { return (BinaryTree<TKey>)Kids[1]; }
        set { Kids[1] = value; }
    }

    public bool Leaf => Left == Sentinel && Right == Sentinel;

    public void Reset()
    {
        Papa = Sentinel;

        Left = (BinaryTree<TKey>)Sentinel;
        Right = (BinaryTree<TKey>)Sentinel;
    }

    public static BinaryTree<TKey> Search(TKey key, BinaryTree<TKey>? root)
    {
        Assert.NonNullReference(key, nameof(key));

        BinaryTree<TKey> node = root ?? (BinaryTree<TKey>)Sentinel;

        while(node != Sentinel)
        {
            int compareKeysResult = CompareKeys(node.GetKey(), key);

            if(compareKeysResult == 0)
            {
                return node;
            }
            else if(compareKeysResult < 0)
            {
                node = node.Left;
            }
            else if(compareKeysResult > 0)
            {
                node = node.Right;
            }
        }

        return (BinaryTree<TKey>)Sentinel;
    }

    public static BinaryTree<TKey> Insert(BinaryTree<TKey> tree, BinaryTree<TKey>? root)
    {
        Assert.NonNullReference(tree, nameof(tree));

        TKey key = tree.GetKey();

        BinaryTree<TKey>? node = root ?? (BinaryTree<TKey>)Sentinel;
        BinaryTree<TKey>? newRoot = node;

        while(node != Sentinel)
        {
            newRoot = node;

            int compareKeysResult = CompareKeys(node.GetKey(), key);

            if(compareKeysResult < 0)
            {
                node = node.Left;
            }
            else if(compareKeysResult > 0)
            {
                node = node.Right;
            }
        }

        if(newRoot == Sentinel)
        {
            newRoot = tree;
        }
        else
        {
            tree.Papa = newRoot;

            int compareKeysResult = CompareKeys(newRoot.GetKey(), key);

            if(compareKeysResult < 0)
            {
                // left
                newRoot.Left = tree;
            }
            else // compareKeysResult >= 0
            {
                // right
                newRoot.Right = tree;
            }
        }

        return newRoot;
    }

    public static BinaryTree<TKey> Delete(TKey key, BinaryTree<TKey>? root)
    {
        Assert.NonNullReference(key, nameof(key));

        BinaryTree<TKey> nodeToDelete = Search(key, root);

        if(nodeToDelete == Sentinel)
        {
            // case 0: not found
            // do nothing
            return nodeToDelete;
        }
        else if(nodeToDelete.Leaf)
        {
            // case 1: no kids - leaf, simply remove the node (nodeToDelete).
            BinaryTree<TKey> papa = (BinaryTree<TKey>)nodeToDelete.Papa;

            if(papa.Left == nodeToDelete)
            {
                papa.Left = (BinaryTree<TKey>)Sentinel;
            }
            else if(papa.Right == nodeToDelete)
            {
                papa.Right = (BinaryTree<TKey>)Sentinel;
            }
        }
        else if(nodeToDelete.Left == Sentinel || nodeToDelete.Right == Sentinel)
        {
            // case 2: one kid (left or right), replace the node to be deleted (nodeToDelete) with its kid.
            BinaryTree<TKey> kid;

            if(nodeToDelete.Left != Sentinel)
            {
                kid = nodeToDelete.Left;
            }
            else
            {
                kid = nodeToDelete.Right;
            }

            BinaryTree<TKey> papa = (BinaryTree<TKey>)nodeToDelete.Papa;

            if(papa.Left == nodeToDelete)
            {
                papa.Left = kid;
            }
            else if(papa.Right == nodeToDelete)
            {
                papa.Right = kid;
            }

            kid.Papa = papa;
        }
        else if(nodeToDelete.Left != Sentinel && nodeToDelete.Right != Sentinel)
        {
            // case 3: two kids.
            BinaryTree<TKey> successor = GetInOrderSuccessor(nodeToDelete);

            // Replace the node (nodeToDelete) with the inorder successor.
            if(successor != Sentinel && successor != nodeToDelete) // check against self ref
            {
                BinaryTree<TKey> nodeToDeletePapa = (BinaryTree<TKey>)nodeToDelete.Papa;
                BinaryTree<TKey> nodeToDeleteLeft = nodeToDelete.Left;
                BinaryTree<TKey> nodeToDeleteRight = nodeToDelete.Right;

                // update successor
                successor.Papa = nodeToDeletePapa;
                successor.Left = nodeToDeleteLeft;
                successor.Right = nodeToDeleteRight;

                // update nodeToDelete's papa
                if(nodeToDeletePapa.Left == nodeToDelete)
                    nodeToDeletePapa.Left = successor;
                if(nodeToDeletePapa.Right == nodeToDelete)
                    nodeToDeletePapa.Right = successor;

                // update nodeToDelete's left
                if(nodeToDeleteLeft != Sentinel)
                    nodeToDeleteLeft.Papa = successor;

                // update nodeToDelete's right
                if(nodeToDeleteRight != Sentinel)
                    nodeToDeleteRight.Papa = successor;
            }
        }

        nodeToDelete.Reset();

        return nodeToDelete;
    }

    /// <summary>
    /// In a Binary Search Tree (BST), the inorder kid of a node is the next node in the inorder traversal of the tree.
    /// For a given node, the inorder kid is the node with the smallest key greater than the current node's key.
    /// </summary>
    /// <param name="tree"></param>
    /// <returns></returns>
    public static BinaryTree<TKey> GetInOrderSuccessor(BinaryTree<TKey> tree)
    {
        Assert.NonNullReference(tree, nameof(tree));

        // case 1: node has a right subtree
        if(tree.Right != Sentinel)
        {
            return GetLeftMostSuccessor(tree.Right);
        }

        // case 2: node has no right subtree ... walk up parents
        BinaryTree<TKey> node = tree;

        // Go up to the parent using the parent pointer until you find a node that is the left child of its parent.
        // The parent of that node will be the inorder kid.
        while(node.Papa != Sentinel && node == ((BinaryTree<TKey>)node.Papa).Right)
        {
            node = (BinaryTree<TKey>)node.Papa;
        }

        return (BinaryTree<TKey>)node.Papa;
    }

    public static BinaryTree<TKey> GetLeftMostSuccessor(BinaryTree<TKey> tree)
    {
        Assert.NonNullReference(tree, nameof(tree));

        BinaryTree<TKey> successor = tree;

        while(successor.Left != Sentinel)
        {
            successor = successor.Left;
        }

        return successor;
    }

    public static BinaryTree<TKey> GetRightMostSuccessor(BinaryTree<TKey> tree)
    {
        Assert.NonNullReference(tree, nameof(tree));

        BinaryTree<TKey> successor = tree;

        while(successor.Right != Sentinel)
        {
            successor = successor.Right;
        }

        return successor;
    }

    public abstract TKey GetKey();

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
