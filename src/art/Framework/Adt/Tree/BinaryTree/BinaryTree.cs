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
        AddKid(default); // left
        AddKid(default); // right
    }

    public new BinaryTree<TKey>? Papa
    {
        get { return (BinaryTree<TKey>?)base.Papa; }
        set { base.Papa = value; }
    }

    public BinaryTree<TKey>? Left
    {
        get { return (BinaryTree<TKey>?)Kids[0]; }
        set { Kids[0] = value; }
    }

    public BinaryTree<TKey>? Right
    {
        get { return (BinaryTree<TKey>?)Kids[1]; }
        set { Kids[1] = value; }
    }

    public BinaryTree<TKey>? Uncle(BinaryTree<TKey> node) => (BinaryTree<TKey>?)(node?.Papa?.Papa?.Kids[1]);

    public bool Leaf => Left is null && Right is null;

    public BinaryTree<TKey> Root()
    {
        BinaryTree<TKey>? root = this;

        while(root is not null && root.Papa is not null)
        {
            root = root.Papa;
        }

        return root ?? this;
    }

    public void Reset()
    {
        Papa = default;
        Left = default;
        Right = default;
    }

    public static BinaryTree<TKey>? Search(TKey key, BinaryTree<TKey>? root)
    {
        Assert.NonNullReference(key);

        BinaryTree<TKey>? node = root;

        while(node is not null)
        {
            int compareKeysResult = CompareKeys(key, node.GetKey());

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

        return default;
    }

    public static bool Insert(BinaryTree<TKey> tree, BinaryTree<TKey> root)
    {
        Assert.NonNullReference(tree);
        Assert.NonNullReference(root);

        TKey key = tree.GetKey();

        BinaryTree<TKey>? node = root;
        BinaryTree<TKey>? papa = root;

        int compareKeysResult;

        while(node is not null)
        {
            papa = node;
            //                              floating root
            compareKeysResult = CompareKeys(key, node.GetKey());

            if(compareKeysResult < 0)
            {
                node = node.Left;
            }
            else if(compareKeysResult > 0)
            {
                node = node.Right;
            }
            else if(compareKeysResult == 0) // duplicate
            {
                return false; // ignore, do not insert
            }
        }

        tree.Papa = papa;

        compareKeysResult = CompareKeys(papa.GetKey(), key);

        if(compareKeysResult < 0)
        {
            papa.Right = tree;
        }
        else // compareKeysResult >= 0
        {
            papa.Left = tree;
        }

        return true;
    }

    public static BinaryTree<TKey>? Delete(TKey key, BinaryTree<TKey> root)
    {
        Assert.NonNullReference(key);
        Assert.NonNullReference(root);

        BinaryTree<TKey>? nodeToDelete = Search(key, root);
        return Delete(nodeToDelete);
    }

    public static BinaryTree<TKey>? Delete(BinaryTree<TKey>? nodeToDelete)
    {
        // See my AIL C++ library.
        if(nodeToDelete is null)
        {
            // case 0: not found
            // do nothing
            return nodeToDelete;
        }

        BinaryTree<TKey>? node;

        if(nodeToDelete.Left is null || nodeToDelete.Right is null)
        {
            // case 1: no kids - leaf, simply remove the node (nodeToDelete).
            // case 2: one kid (left or right), replace the node to be deleted (nodeToDelete) with its kid.
            node = nodeToDelete;
        }
        else
        {
            // case 3: two kids.
            node = GetInOrderSuccessor(nodeToDelete);
        }

        if(node is null)
        {
            nodeToDelete.Reset();
            return nodeToDelete;
        }

        BinaryTree<TKey>? tmpNode;

        if(node.Left is not null)
        {
            tmpNode = node.Left;
        }
        else
        {
            tmpNode = node.Right;
        }

        if(tmpNode is not null)
        {
            tmpNode.Papa = node.Papa;
        }

        if(node.Papa is not null)
        {
            if(ReferenceEquals(node, node.Papa.Left))
            {
                node.Papa.Left = tmpNode;
            }
            else
            {
                node.Papa.Right = tmpNode;
            }
        }

        if(!ReferenceEquals(node, nodeToDelete))
        {
            node.Papa  = nodeToDelete.Papa;
            node.Left  = nodeToDelete.Left;
            node.Right = nodeToDelete.Right;

            if(nodeToDelete.Papa is not null)
            {
                if(ReferenceEquals(nodeToDelete, nodeToDelete.Papa.Left))
                {
                    nodeToDelete.Papa.Left = node;
                }
                else
                {
                    nodeToDelete.Papa.Right = node;
                }
            }

            if(nodeToDelete.Left is not null)
            {
                nodeToDelete.Left.Papa = node;
            }

            if(nodeToDelete.Right is not null)
            {
                nodeToDelete.Right.Papa = node;
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
    public static BinaryTree<TKey>? GetInOrderSuccessor(BinaryTree<TKey>? tree)
    {
        // case 1: node has a right subtree
        if(tree is not null && tree.Right is not null)
        {
            return GetLeftMostSuccessor(tree.Right);
        }

        // case 2: node has no right subtree ... walk up parents
        BinaryTree<TKey>? node = tree;

        // Go up to the parent using the parent pointer until you find a node that is the left child of its parent.
        // The parent of that node will be the inorder kid.
        while(node is not null &&
              node.Papa is not null &&
              ReferenceEquals(node, node.Papa.Right))
        {
            node = node.Papa;
        }

        return node?.Papa;
    }

    public static BinaryTree<TKey>? GetLeftMostSuccessor(BinaryTree<TKey>? tree)
    {
        BinaryTree<TKey>? successor = tree;

        while(successor is not null && successor.Left is not null)
        {
            successor = successor.Left;
        }

        return successor;
    }

    public static BinaryTree<TKey>? GetRightMostSuccessor(BinaryTree<TKey>? tree)
    {
        BinaryTree<TKey>? successor = tree;

        while(successor is not null && successor.Right is not null)
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

    public static bool Validate(BinaryTree<TKey> tree)
    {
        Assert.NonNullReference(tree);

        IEnumerable<BinaryTree<TKey>> inorderDfs = InorderDfs(tree).Cast<BinaryTree<TKey>>();

        BinaryTree<TKey>? prevNode = inorderDfs.FirstOrDefault();

        if(prevNode is not null)
        {
            foreach(BinaryTree<TKey> node in inorderDfs.Skip(1))
            {
                if(CompareKeys(prevNode.GetKey(), node.GetKey()) > 0)
                    return false;

                prevNode = node;
            }
        }

        return true;
    }

    public static IEnumerable<BinaryTree<TKey>> InorderDfs(BinaryTree<TKey> tree)
    {
        Assert.NonNullReference(tree);

        Stack<BinaryTree<TKey>> stack = new();

        BinaryTree<TKey>? currentTree = tree;

        while(currentTree is not null || stack.Count > 0)
        {
            // traverse the left side
            while(currentTree is not null)
            {
                stack.Push(currentTree);
                currentTree = currentTree.Left;
            }

            currentTree = stack.Pop();
            yield return currentTree;

            // switch to right side
            currentTree = currentTree?.Right;
        }
    }

    /// <summary>
    /// ChatGPT.
    /// A left rotation is performed to decrease the height of the right subtree.
    ///     1. Identify the pivot node (y), which is the right child of the current root (x).
    ///     2. Make the left subtree of y the right subtree of x.
    ///     3. Make y the new root, and x becomes the left child of y.
    ///
    ///           r                r
    ///            \                \
    ///             x                y
    ///              \              / \
    ///               y            x   T1
    ///              / \            \
    ///             T1  T2           T2
    ///
    /// </summary>
    /// <param name="tree"></param>
    protected static void RotateLeft(RedBlackTree<TKey> tree)
    {

        // return pivot - might be a new root
    }

    /// <summary>
    /// ChatGPT.
    /// A right rotation is performed to decrease the height of the left subtree.
    ///     1. Identify the pivot node (y), which is the left child of the current root (x).
    ///     2. Make the right subtree of y the left subtree of x.
    ///     3. Make y the new root, and x becomes the right child of y.
    /// 
    ///           r                r
    ///            \                \
    ///             x                y
    ///            /                / \
    ///           y                T1  x
    ///          / \                   /
    ///         T1  T2                T2
    /// 
    /// </summary>
    /// <param name="tree"></param>
    protected static void RotateRight(RedBlackTree<TKey> tree)
    {
        // return pivot - might be a new root
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
