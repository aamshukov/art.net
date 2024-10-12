//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Security.Cryptography;
using UILab.Art.Framework.Adt.Graph;
using UILab.Art.Framework.Adt.Tree;

namespace UILab.Art.Tests;

internal class BinaryTreeInt : BinaryTree<int>
{
    public BinaryTreeInt(id id,
                         Tree? papa = default,
                         string? label = default,
                         object? value = default,
                         Flags flags = Flags.Clear,
                         Color color = Color.Unknown,
                         Dictionary<string, object>? attributes = default,
                         string? version = default) : base(id, papa, label, value, flags, color, attributes, version)
    {
    }

    public override int GetKey()
    {
        return Convert.ToInt32(Label);
    }
}

internal class RedBlackTreeInt : RedBlackTree<int>
{
    public RedBlackTreeInt(id id,
                           Tree? papa = default,
                           string? label = default,
                           object? value = default,
                           Flags flags = Flags.Clear,
                           Color color = Color.Unknown,
                           Dictionary<string, object>? attributes = default,
                           string? version = default) : base(id, papa, label, value, flags, color, attributes, version)
    {
    }

    public override int GetKey()
    {
        return Convert.ToInt32(Label);
    }
}

internal class AvlTreeInt : AvlTree<int>
{
    public AvlTreeInt(id id,
                      Tree? papa = default,
                      string? label = default,
                      object? value = default,
                      Flags flags = Flags.Clear,
                      Color color = Color.Unknown,
                      Dictionary<string, object>? attributes = default,
                      string? version = default) : base(id, papa, label, value, flags, color, attributes, version)
    {
    }

    public override int GetKey()
    {
        return Convert.ToInt32(Label);
    }
}

[TestFixture]
internal class TreeTests
{
    private const string DotDirectory = @"d:\tmp\art.trees.viz";

    private static void PruneTree(List<BinaryTreeInt> nodes)
    {
        while(nodes.Count > 0)
        {
            var index = (nodes.Count - 1) == 0 ? 0 : RandomNumberGenerator.GetInt32(0, nodes.Count - 1);
            var node = nodes[index];
            var newRoot = node.Root();

            //Console.WriteLine($"node: {node.Label}, newRoot: {newRoot.Label}");

            Assert.That(BinaryTreeInt.Validate(newRoot), Is.True);

            nodes.RemoveAt(index);

            if(index % 2 == 0)
                BinaryTreeInt.Delete(node);
            else
                BinaryTreeInt.Delete(node.GetKey(), newRoot);
        }
    }

    private static void PruneRedBlackTree(List<RedBlackTreeInt> nodes)
    {
        while(nodes.Count > 0)
        {
            var index = (nodes.Count - 1) == 0 ? 0 : RandomNumberGenerator.GetInt32(0, nodes.Count - 1);
            var node = nodes[index];
            var newRoot = node.Root();

            //Console.WriteLine($"node: {node.Label}, newRoot: {newRoot.Label}");

            Assert.That(RedBlackTreeInt.Validate(newRoot), Is.True);

            nodes.RemoveAt(index);

            if(index % 2 == 0)
                RedBlackTreeInt.Delete(node);
            else
                RedBlackTreeInt.Delete(node.GetKey(), newRoot);
        }
    }

    private static void PruneAvlTree(List<AvlTreeInt> nodes)
    {
        while(nodes.Count > 0)
        {
            var index = (nodes.Count - 1) == 0 ? 0 : RandomNumberGenerator.GetInt32(0, nodes.Count - 1);
            var node = nodes[index];
            var newRoot = node.Root();

            //Console.WriteLine($"node: {node.Label}, newRoot: {newRoot.Label}");

            Assert.That(AvlTreeInt.Validate(newRoot), Is.True);

            nodes.RemoveAt(index);

            if(index % 2 == 0)
                AvlTreeInt.Delete(node);
            else
                AvlTreeInt.Delete(node.GetKey(), newRoot);
        }
    }

    [Test]
    public void BinaryTree_Insert_Success()
    {
        //              5
        //        3          8
        //     2     4    6     9
        //  1               7
        //
        var binTree1 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "1");
        var binTree2 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "2");
        var binTree3 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "3");
        var binTree4 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "4");
        var binTree5 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "5");
        var binTree6 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "6");
        var binTree7 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "7");
        var binTree8 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "8");
        var binTree9 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "9");

        Assert.That(BinaryTreeInt.Insert(binTree3, binTree5), Is.True);
        Assert.That(BinaryTreeInt.Insert(binTree8, binTree5), Is.True);
        Assert.That(BinaryTreeInt.Insert(binTree2, binTree5), Is.True);
        Assert.That(BinaryTreeInt.Insert(binTree4, binTree5), Is.True);
        Assert.That(BinaryTreeInt.Insert(binTree6, binTree5), Is.True);
        Assert.That(BinaryTreeInt.Insert(binTree9, binTree5), Is.True);
        Assert.That(BinaryTreeInt.Insert(binTree7, binTree5), Is.True);
        Assert.That(BinaryTreeInt.Insert(binTree1, binTree5), Is.True);

        Assert.That(binTree5?.Left, Is.EqualTo(binTree3));
        Assert.That(binTree3.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree5.Right, Is.EqualTo(binTree8));
        Assert.That(binTree8.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree3.Left, Is.EqualTo(binTree2));
        Assert.That(binTree2.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));
        Assert.That(binTree4.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree8.Left, Is.EqualTo(binTree6));
        Assert.That(binTree6.Papa, Is.EqualTo(binTree8));
        Assert.That(binTree8.Right, Is.EqualTo(binTree9));
        Assert.That(binTree9.Papa, Is.EqualTo(binTree8));
        Assert.That(binTree6.Right, Is.EqualTo(binTree7));
        Assert.That(binTree7.Papa, Is.EqualTo(binTree6));
        Assert.That(binTree2.Left, Is.EqualTo(binTree1));
        Assert.That(binTree1.Papa, Is.EqualTo(binTree2));
    }

    [Test]
    public void BinaryTree_Delete_Case0_NotFound_Success()
    {
        var binTree2 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "2");
        var binTree3 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "3");
        var binTree4 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "4");

        BinaryTreeInt.Insert(binTree3, binTree4);
        Assert.That(binTree4.Left, Is.EqualTo(binTree3));
        Assert.That(binTree3.Papa, Is.EqualTo(binTree4));

        var deletedNode = BinaryTreeInt.Delete(binTree2.GetKey(), binTree4);
        Assert.That(BinaryTreeInt.Validate(binTree4), Is.True);

        Assert.That(binTree4.Left, Is.EqualTo(binTree3));
        Assert.That(binTree3.Papa, Is.EqualTo(binTree4));
        Assert.That(deletedNode, Is.Null);
    }

    [Test]
    public void BinaryTree_Delete_Case1_LeftLeaf_Success()
    {
        //              5
        //        3           7
        //     2     4     6     8
        //
        var binTree2 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "2");
        var binTree3 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "3");
        var binTree4 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "4");
        var binTree5 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "5");
        var binTree6 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "6");
        var binTree7 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "7");
        var binTree8 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "8");

        BinaryTreeInt.Insert(binTree3, binTree5);
        BinaryTreeInt.Insert(binTree7, binTree5);
        BinaryTreeInt.Insert(binTree2, binTree5);
        BinaryTreeInt.Insert(binTree4, binTree5);
        BinaryTreeInt.Insert(binTree6, binTree5);
        BinaryTreeInt.Insert(binTree8, binTree5);

        Assert.That(binTree5.Left, Is.EqualTo(binTree3));
        Assert.That(binTree3.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree5.Right, Is.EqualTo(binTree7));
        Assert.That(binTree7.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree3.Left, Is.EqualTo(binTree2));
        Assert.That(binTree2.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));
        Assert.That(binTree4.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree7.Left, Is.EqualTo(binTree6));
        Assert.That(binTree6.Papa, Is.EqualTo(binTree7));
        Assert.That(binTree7.Right, Is.EqualTo(binTree8));
        Assert.That(binTree8.Papa, Is.EqualTo(binTree7));

        var deletedNode = BinaryTreeInt.Delete(binTree2.GetKey(), binTree5);
        Assert.That(deletedNode, Is.Not.Null);
        Assert.That(deletedNode?.Left, Is.Null);
        Assert.That(deletedNode?.Right, Is.Null);
        Assert.That(deletedNode?.Papa, Is.Null);
        Assert.That(BinaryTreeInt.Validate(binTree5), Is.True);

        Assert.That(binTree5.Left, Is.EqualTo(binTree3));
        Assert.That(binTree3.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree5.Right, Is.EqualTo(binTree7));
        Assert.That(binTree7.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree3.Left, Is.Null);
        Assert.That(binTree2.Papa, Is.Null);
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));
        Assert.That(binTree4.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree7.Left, Is.EqualTo(binTree6));
        Assert.That(binTree6.Papa, Is.EqualTo(binTree7));
        Assert.That(binTree7.Right, Is.EqualTo(binTree8));
        Assert.That(binTree8.Papa, Is.EqualTo(binTree7));

        PruneTree([ binTree2, binTree3, binTree4, binTree5, binTree6, binTree7, binTree8 ]);
        Assert.That(binTree5.Papa, Is.Null);
        Assert.That(binTree5.Leaf, Is.True);
    }

    [Test]
    public void BinaryTree_Delete_Case1_RightLeaf_Success()
    {
        //              5
        //        3           7
        //     2     4     6     8
        //
        var binTree2 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "2");
        var binTree3 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "3");
        var binTree4 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "4");
        var binTree5 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "5");
        var binTree6 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "6");
        var binTree7 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "7");
        var binTree8 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "8");

        BinaryTreeInt.Insert(binTree3, binTree5);
        BinaryTreeInt.Insert(binTree7, binTree5);
        BinaryTreeInt.Insert(binTree2, binTree5);
        BinaryTreeInt.Insert(binTree4, binTree5);
        BinaryTreeInt.Insert(binTree6, binTree5);
        BinaryTreeInt.Insert(binTree8, binTree5);

        Assert.That(binTree5.Left, Is.EqualTo(binTree3));
        Assert.That(binTree3.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree5.Right, Is.EqualTo(binTree7));
        Assert.That(binTree7.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree3.Left, Is.EqualTo(binTree2));
        Assert.That(binTree2.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));
        Assert.That(binTree4.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree7.Left, Is.EqualTo(binTree6));
        Assert.That(binTree6.Papa, Is.EqualTo(binTree7));
        Assert.That(binTree7.Right, Is.EqualTo(binTree8));
        Assert.That(binTree8.Papa, Is.EqualTo(binTree7));

        var deletedNode = BinaryTreeInt.Delete(binTree4.GetKey(), binTree5);
        Assert.That(deletedNode?.Left, Is.Null);
        Assert.That(deletedNode?.Right, Is.Null);
        Assert.That(deletedNode?.Papa, Is.Null);
        Assert.That(BinaryTreeInt.Validate(binTree5), Is.True);

        Assert.That(binTree5.Left, Is.EqualTo(binTree3));
        Assert.That(binTree3.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree5.Right, Is.EqualTo(binTree7));
        Assert.That(binTree7.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree3.Left, Is.EqualTo(binTree2));
        Assert.That(binTree2.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree3.Right, Is.Null);
        Assert.That(binTree4.Papa, Is.Null);
        Assert.That(binTree7.Left, Is.EqualTo(binTree6));
        Assert.That(binTree6.Papa, Is.EqualTo(binTree7));
        Assert.That(binTree7.Right, Is.EqualTo(binTree8));
        Assert.That(binTree8.Papa, Is.EqualTo(binTree7));

        //BinaryTreeInt.Delete(binTree7);
        //Assert.That(BinaryTreeInt.Validate(binTree5), Is.True);
        //BinaryTreeInt.Delete(binTree3);
        //Assert.That(BinaryTreeInt.Validate(binTree5), Is.True);
        //BinaryTreeInt.Delete(binTree4);
        //Assert.That(BinaryTreeInt.Validate(binTree5), Is.True);

        PruneTree([ binTree2, binTree3, binTree4, binTree5, binTree6, binTree7, binTree8 ]);
        Assert.That(binTree5.Papa, Is.Null);
        Assert.That(binTree5.Leaf, Is.True);
    }

    [Test]
    public void BinaryTree_Delete_Case2_LeftInternalKid_Success()
    {
        //              5
        //        3          8
        //     2     4    6     9
        //  1               7
        //
        var binTree1 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "1");
        var binTree2 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "2");
        var binTree3 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "3");
        var binTree4 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "4");
        var binTree5 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "5");
        var binTree6 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "6");
        var binTree7 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "7");
        var binTree8 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "8");
        var binTree9 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "9");

        BinaryTreeInt.Insert(binTree3, binTree5);
        BinaryTreeInt.Insert(binTree8, binTree5);
        BinaryTreeInt.Insert(binTree2, binTree5);
        BinaryTreeInt.Insert(binTree4, binTree5);
        BinaryTreeInt.Insert(binTree6, binTree5);
        BinaryTreeInt.Insert(binTree9, binTree5);
        BinaryTreeInt.Insert(binTree7, binTree5);
        BinaryTreeInt.Insert(binTree1, binTree5);

        Assert.That(binTree5.Left, Is.EqualTo(binTree3));
        Assert.That(binTree3.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree5.Right, Is.EqualTo(binTree8));
        Assert.That(binTree8.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree3.Left, Is.EqualTo(binTree2));
        Assert.That(binTree2.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));
        Assert.That(binTree4.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree8.Left, Is.EqualTo(binTree6));
        Assert.That(binTree6.Papa, Is.EqualTo(binTree8));
        Assert.That(binTree8.Right, Is.EqualTo(binTree9));
        Assert.That(binTree9.Papa, Is.EqualTo(binTree8));
        Assert.That(binTree6.Right, Is.EqualTo(binTree7));
        Assert.That(binTree7.Papa, Is.EqualTo(binTree6));
        Assert.That(binTree2.Left, Is.EqualTo(binTree1));
        Assert.That(binTree1.Papa, Is.EqualTo(binTree2));

        var deletedNode = BinaryTreeInt.Delete(binTree2.GetKey(), binTree5);
        Assert.That(deletedNode?.Left, Is.Null);
        Assert.That(deletedNode?.Right, Is.Null);
        Assert.That(deletedNode?.Papa, Is.Null);
        Assert.That(BinaryTreeInt.Validate(binTree5), Is.True);

        Assert.That(binTree5.Left, Is.EqualTo(binTree3));
        Assert.That(binTree3.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree5.Right, Is.EqualTo(binTree8));
        Assert.That(binTree8.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree3.Left, Is.EqualTo(binTree1));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));
        Assert.That(binTree4.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree8.Left, Is.EqualTo(binTree6));
        Assert.That(binTree6.Papa, Is.EqualTo(binTree8));
        Assert.That(binTree8.Right, Is.EqualTo(binTree9));
        Assert.That(binTree9.Papa, Is.EqualTo(binTree8));
        Assert.That(binTree6.Right, Is.EqualTo(binTree7));
        Assert.That(binTree7.Papa, Is.EqualTo(binTree6));
        Assert.That(binTree2.Left, Is.Null);
        Assert.That(binTree2.Right, Is.Null);
        Assert.That(binTree2.Papa, Is.Null);
        Assert.That(binTree1.Papa, Is.EqualTo(binTree3));

        PruneTree([ binTree1, binTree2, binTree3, binTree4, binTree5, binTree6, binTree7, binTree8, binTree9 ]);
        Assert.That(binTree5.Papa, Is.Null);
        Assert.That(binTree5.Leaf, Is.True);
    }

    [Test]
    public void BinaryTree_Delete_Case2_RightInternalKid_Success()
    {
        //              5
        //        2          8
        //     1     3    6     9
        //             4    7
        //
        var binTree1 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "1");
        var binTree2 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "2");
        var binTree3 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "3");
        var binTree4 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "4");
        var binTree5 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "5");
        var binTree6 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "6");
        var binTree7 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "7");
        var binTree8 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "8");
        var binTree9 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "9");

        BinaryTreeInt.Insert(binTree2, binTree5);
        BinaryTreeInt.Insert(binTree8, binTree5);
        BinaryTreeInt.Insert(binTree3, binTree5);
        BinaryTreeInt.Insert(binTree4, binTree5);
        BinaryTreeInt.Insert(binTree6, binTree5);
        BinaryTreeInt.Insert(binTree7, binTree5);
        BinaryTreeInt.Insert(binTree9, binTree5);
        BinaryTreeInt.Insert(binTree1, binTree5);

        Assert.That(binTree5.Papa, Is.Null);
        Assert.That(binTree5.Left, Is.EqualTo(binTree2));
        Assert.That(binTree5.Right, Is.EqualTo(binTree8));

        Assert.That(binTree2.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree2.Left, Is.EqualTo(binTree1));
        Assert.That(binTree2.Right, Is.EqualTo(binTree3));

        Assert.That(binTree8.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree8.Left, Is.EqualTo(binTree6));
        Assert.That(binTree8.Right, Is.EqualTo(binTree9));

        Assert.That(binTree1.Papa, Is.EqualTo(binTree2));
        Assert.That(binTree1.Left, Is.Null);
        Assert.That(binTree1.Right, Is.Null);

        Assert.That(binTree3.Papa, Is.EqualTo(binTree2));
        Assert.That(binTree3.Left, Is.Null);
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));

        Assert.That(binTree6.Papa, Is.EqualTo(binTree8));
        Assert.That(binTree6.Left, Is.Null);
        Assert.That(binTree6.Right, Is.EqualTo(binTree7));

        Assert.That(binTree9.Papa, Is.EqualTo(binTree8));
        Assert.That(binTree9.Left, Is.Null);
        Assert.That(binTree9.Right, Is.Null);

        Assert.That(binTree4.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree4.Left, Is.Null);
        Assert.That(binTree4.Right, Is.Null);

        Assert.That(binTree7.Papa, Is.EqualTo(binTree6));
        Assert.That(binTree7.Left, Is.Null);
        Assert.That(binTree7.Right, Is.Null);

        var deletedNode = BinaryTreeInt.Delete(binTree3.GetKey(), binTree5);
        Assert.That(deletedNode?.Left, Is.Null);
        Assert.That(deletedNode?.Right, Is.Null);
        Assert.That(deletedNode?.Papa, Is.Null);
        Assert.That(BinaryTreeInt.Validate(binTree5), Is.True);

        Assert.That(binTree5.Papa, Is.Null);
        Assert.That(binTree5.Left, Is.EqualTo(binTree2));
        Assert.That(binTree5.Right, Is.EqualTo(binTree8));

        Assert.That(binTree2.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree2.Left, Is.EqualTo(binTree1));
        Assert.That(binTree2.Right, Is.EqualTo(binTree4));

        Assert.That(binTree8.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree8.Left, Is.EqualTo(binTree6));
        Assert.That(binTree8.Right, Is.EqualTo(binTree9));

        Assert.That(binTree1.Papa, Is.EqualTo(binTree2));
        Assert.That(binTree1.Left, Is.Null);
        Assert.That(binTree1.Right, Is.Null);

        Assert.That(binTree3.Papa, Is.Null);
        Assert.That(binTree3.Left, Is.Null);
        Assert.That(binTree3.Right, Is.Null);

        Assert.That(binTree6.Papa, Is.EqualTo(binTree8));
        Assert.That(binTree6.Left, Is.Null);
        Assert.That(binTree6.Right, Is.EqualTo(binTree7));

        Assert.That(binTree9.Papa, Is.EqualTo(binTree8));
        Assert.That(binTree9.Left, Is.Null);
        Assert.That(binTree9.Right, Is.Null);

        Assert.That(binTree4.Papa, Is.EqualTo(binTree2));
        Assert.That(binTree4.Left, Is.Null);
        Assert.That(binTree4.Right, Is.Null);

        Assert.That(binTree7.Papa, Is.EqualTo(binTree6));
        Assert.That(binTree7.Left, Is.Null);
        Assert.That(binTree7.Right, Is.Null);

        PruneTree([ binTree1, binTree2, binTree3, binTree4, binTree5, binTree6, binTree7, binTree8, binTree9 ]);
        Assert.That(binTree5.Papa, Is.Null);
        Assert.That(binTree5.Leaf, Is.True);
    }

    [Test]
    public async Task BinaryTree_Delete_Case3_TwoKids_Root_Success()
    {
        //                5                                          6
        //         3             8           5                3             8
        //     2       4     6       9       ==>          2       4     7       9
        //                      7
        //
        var binTree2 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "2");
        var binTree3 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "3");
        var binTree4 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "4");
        var binTree5 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "5");
        var binTree6 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "6");
        var binTree7 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "7");
        var binTree8 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "8");
        var binTree9 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "9");

        BinaryTreeInt.Insert(binTree3, binTree5);
        BinaryTreeInt.Insert(binTree8, binTree5);
        BinaryTreeInt.Insert(binTree2, binTree5);
        BinaryTreeInt.Insert(binTree4, binTree5);
        BinaryTreeInt.Insert(binTree6, binTree5);
        BinaryTreeInt.Insert(binTree9, binTree5);
        BinaryTreeInt.Insert(binTree7, binTree5);

        Assert.That(binTree5.Papa, Is.Null);
        Assert.That(binTree5.Left, Is.EqualTo(binTree3));
        Assert.That(binTree5.Right, Is.EqualTo(binTree8));

        Assert.That(binTree3.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree3.Left, Is.EqualTo(binTree2));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));

        Assert.That(binTree8.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree8.Left, Is.EqualTo(binTree6));
        Assert.That(binTree8.Right, Is.EqualTo(binTree9));

        Assert.That(binTree2.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree2.Left, Is.Null);
        Assert.That(binTree2.Right, Is.Null);

        Assert.That(binTree4.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree4.Left, Is.Null);
        Assert.That(binTree4.Right, Is.Null);

        Assert.That(binTree6.Papa, Is.EqualTo(binTree8));
        Assert.That(binTree6.Left, Is.Null);
        Assert.That(binTree6.Right, Is.EqualTo(binTree7));

        Assert.That(binTree9.Papa, Is.EqualTo(binTree8));
        Assert.That(binTree9.Left, Is.Null);
        Assert.That(binTree9.Right, Is.Null);

        Assert.That(binTree7.Papa, Is.EqualTo(binTree6));
        Assert.That(binTree7.Left, Is.Null);
        Assert.That(binTree7.Right, Is.Null);

        const string fileName = "BinaryTree_Delete_Case3_TwoKids_Root_Success";
        await TreeSerialization.SerializeTree(DotDirectory, $"{fileName}.dot", binTree5, digraph: true);

        var deletedNode = BinaryTreeInt.Delete(binTree5.GetKey(), binTree5);
        Assert.That(deletedNode?.Left, Is.Null);
        Assert.That(deletedNode?.Right, Is.Null);
        Assert.That(deletedNode?.Papa, Is.Null);
        Assert.That(BinaryTreeInt.Validate(binTree5), Is.True);

        Assert.That(binTree5.Papa, Is.Null);
        Assert.That(binTree5.Left, Is.Null);
        Assert.That(binTree5.Right, Is.Null);

        Assert.That(binTree3.Papa, Is.EqualTo(binTree6));
        Assert.That(binTree3.Left, Is.EqualTo(binTree2));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));

        Assert.That(binTree8.Papa, Is.EqualTo(binTree6));
        Assert.That(binTree8.Left, Is.EqualTo(binTree7));
        Assert.That(binTree8.Right, Is.EqualTo(binTree9));

        Assert.That(binTree2.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree2.Left, Is.Null);
        Assert.That(binTree2.Right, Is.Null);

        Assert.That(binTree4.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree4.Left, Is.Null);
        Assert.That(binTree4.Right, Is.Null);

        Assert.That(binTree6.Papa, Is.Null); // root
        Assert.That(binTree6.Left, Is.EqualTo(binTree3));
        Assert.That(binTree6.Right, Is.EqualTo(binTree8));

        Assert.That(binTree9.Papa, Is.EqualTo(binTree8));
        Assert.That(binTree9.Left, Is.Null);
        Assert.That(binTree9.Right, Is.Null);

        Assert.That(binTree7.Papa, Is.EqualTo(binTree8));
        Assert.That(binTree7.Left, Is.Null);
        Assert.That(binTree7.Right, Is.Null);

        PruneTree([ binTree2, binTree3, binTree4, binTree5, binTree6, binTree7, binTree8, binTree9 ]);
        Assert.That(binTree5.Papa, Is.Null);
        Assert.That(binTree5.Leaf, Is.True);
    }

    [Test]
    public void BinaryTree_Delete_Case3_TwoKids_Left_Success()
    {
        //                 7                  3                        7
        //         3              12          ==>              4              12
        //     1       5       11                           1      5       11
        //           4      10                                          10
        //
        var binTree7  = new TreeFactory().CreateTree<BinaryTreeInt>(label: "7");
        var binTree3  = new TreeFactory().CreateTree<BinaryTreeInt>(label: "3");
        var binTree12 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "12");
        var binTree1  = new TreeFactory().CreateTree<BinaryTreeInt>(label: "1");
        var binTree5  = new TreeFactory().CreateTree<BinaryTreeInt>(label: "5");
        var binTree11 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "11");
        var binTree4  = new TreeFactory().CreateTree<BinaryTreeInt>(label: "4");
        var binTree10 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "10");

        Assert.That(BinaryTreeInt.Insert(binTree3, binTree7), Is.True);
        Assert.That(BinaryTreeInt.Insert(binTree12, binTree7), Is.True);
        Assert.That(BinaryTreeInt.Insert(binTree12, binTree7), Is.False);
        Assert.That(BinaryTreeInt.Insert(binTree1,  binTree7), Is.True);
        Assert.That(BinaryTreeInt.Insert(binTree5, binTree7), Is.True);
        Assert.That(BinaryTreeInt.Insert(binTree4, binTree7), Is.True);
        Assert.That(BinaryTreeInt.Insert(binTree11, binTree7), Is.True);
        Assert.That(BinaryTreeInt.Insert(binTree10, binTree7), Is.True);

        Assert.That(binTree7.Papa, Is.Null);
        Assert.That(binTree7.Left, Is.EqualTo(binTree3));
        Assert.That(binTree7.Right, Is.EqualTo(binTree12));

        Assert.That(binTree3.Papa, Is.EqualTo(binTree7));
        Assert.That(binTree3.Left, Is.EqualTo(binTree1));
        Assert.That(binTree3.Right, Is.EqualTo(binTree5));

        Assert.That(binTree12.Papa, Is.EqualTo(binTree7));
        Assert.That(binTree12.Left, Is.EqualTo(binTree11));
        Assert.That(binTree12.Right, Is.Null);

        Assert.That(binTree1.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree1.Left, Is.Null);
        Assert.That(binTree1.Right, Is.Null);

        Assert.That(binTree5.Papa, Is.EqualTo(binTree3));
        Assert.That(binTree5.Left, Is.EqualTo(binTree4));
        Assert.That(binTree5.Right, Is.Null);

        Assert.That(binTree11.Papa, Is.EqualTo(binTree12));
        Assert.That(binTree11.Left, Is.EqualTo(binTree10));
        Assert.That(binTree11.Right, Is.Null);

        Assert.That(binTree4.Papa, Is.EqualTo(binTree5));
        Assert.That(binTree4.Left, Is.Null);
        Assert.That(binTree4.Right, Is.Null);

        Assert.That(binTree10.Papa, Is.EqualTo(binTree11));
        Assert.That(binTree10.Left, Is.Null);
        Assert.That(binTree10.Right, Is.Null);

        var deletedNode = BinaryTreeInt.Delete(binTree3.GetKey(), binTree7);
        Assert.That(deletedNode?.Left, Is.Null);
        Assert.That(deletedNode?.Right, Is.Null);
        Assert.That(deletedNode?.Papa, Is.Null);
        Assert.That(BinaryTreeInt.Validate(binTree7), Is.True);

        PruneTree([ binTree7, binTree3, binTree12, binTree1, binTree5, binTree11, binTree4, binTree10 ]);
        Assert.That(binTree7.Papa, Is.Null);
        Assert.That(binTree7.Leaf, Is.True);
    }

    [Test]
    public void BinaryTree_Delete_Case3_TwoKids_Left_2_Success()
    {
        //                 20                     15                      20
        //             15               25        ==>                16              25
        //     10             18                               10           18
        //        12       17    19                               12     17    19
        //              16
        //
        var binTree20 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "20");
        var binTree15 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "15");
        var binTree25 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "25");
        var binTree10 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "10");
        var binTree18 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "18");
        var binTree12 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "12");
        var binTree17 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "17");
        var binTree19 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "19");
        var binTree16 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "16");

        BinaryTreeInt.Insert(binTree15, binTree20);
        BinaryTreeInt.Insert(binTree25, binTree20);
        BinaryTreeInt.Insert(binTree10, binTree20);
        BinaryTreeInt.Insert(binTree18, binTree20);
        BinaryTreeInt.Insert(binTree12, binTree20);
        BinaryTreeInt.Insert(binTree17, binTree20);
        BinaryTreeInt.Insert(binTree19, binTree20);
        BinaryTreeInt.Insert(binTree16, binTree20);

        Assert.That(binTree20.Papa, Is.Null);
        Assert.That(binTree20.Left, Is.EqualTo(binTree15));
        Assert.That(binTree20.Right, Is.EqualTo(binTree25));

        Assert.That(binTree15.Papa, Is.EqualTo(binTree20));
        Assert.That(binTree15.Left, Is.EqualTo(binTree10));
        Assert.That(binTree15.Right, Is.EqualTo(binTree18));

        Assert.That(binTree15.Papa, Is.EqualTo(binTree20));
        Assert.That(binTree15.Left, Is.EqualTo(binTree10));
        Assert.That(binTree15.Right, Is.EqualTo(binTree18));

        Assert.That(binTree10.Papa, Is.EqualTo(binTree15));
        Assert.That(binTree10.Left, Is.Null);
        Assert.That(binTree10.Right, Is.EqualTo(binTree12));

        Assert.That(binTree18.Papa, Is.EqualTo(binTree15));
        Assert.That(binTree18.Left, Is.EqualTo(binTree17));
        Assert.That(binTree18.Right, Is.EqualTo(binTree19));

        Assert.That(binTree12.Papa, Is.EqualTo(binTree10));
        Assert.That(binTree12.Left, Is.Null);
        Assert.That(binTree12.Right, Is.Null);

        Assert.That(binTree17.Papa, Is.EqualTo(binTree18));
        Assert.That(binTree17.Left, Is.EqualTo(binTree16));
        Assert.That(binTree17.Right, Is.Null);

        Assert.That(binTree19.Papa, Is.EqualTo(binTree18));
        Assert.That(binTree19.Left, Is.Null);
        Assert.That(binTree19.Right, Is.Null);

        Assert.That(binTree16.Papa, Is.EqualTo(binTree17));
        Assert.That(binTree16.Left, Is.Null);
        Assert.That(binTree16.Right, Is.Null);

        var deletedNode = BinaryTreeInt.Delete(binTree15.GetKey(), binTree20);
        Assert.That(deletedNode?.Left, Is.Null);
        Assert.That(deletedNode?.Right, Is.Null);
        Assert.That(deletedNode?.Papa, Is.Null);
        Assert.That(BinaryTreeInt.Validate(binTree20), Is.True);

        Assert.That(binTree20.Papa, Is.Null);
        Assert.That(binTree20.Left, Is.EqualTo(binTree16));
        Assert.That(binTree20.Right, Is.EqualTo(binTree25));

        Assert.That(binTree15.Papa, Is.Null);
        Assert.That(binTree15.Left, Is.Null);
        Assert.That(binTree15.Right, Is.Null);

        Assert.That(binTree10.Papa, Is.EqualTo(binTree16));
        Assert.That(binTree10.Left, Is.Null);
        Assert.That(binTree10.Right, Is.EqualTo(binTree12));

        Assert.That(binTree18.Papa, Is.EqualTo(binTree16));
        Assert.That(binTree18.Left, Is.EqualTo(binTree17));
        Assert.That(binTree18.Right, Is.EqualTo(binTree19));

        Assert.That(binTree12.Papa, Is.EqualTo(binTree10));
        Assert.That(binTree12.Left, Is.Null);
        Assert.That(binTree12.Right, Is.Null);

        Assert.That(binTree17.Papa, Is.EqualTo(binTree18));
        Assert.That(binTree17.Left, Is.Null);
        Assert.That(binTree17.Right, Is.Null);

        Assert.That(binTree19.Papa, Is.EqualTo(binTree18));
        Assert.That(binTree19.Left, Is.Null);
        Assert.That(binTree19.Right, Is.Null);

        Assert.That(binTree16.Papa, Is.EqualTo(binTree20));
        Assert.That(binTree16.Left, Is.EqualTo(binTree10));
        Assert.That(binTree16.Right, Is.EqualTo(binTree18));

        PruneTree([ binTree20, binTree15, binTree25, binTree10, binTree18, binTree12, binTree17, binTree19, binTree16 ]);
        Assert.That(binTree20.Papa, Is.Null);
        Assert.That(binTree20.Leaf, Is.True);
    }

    [Test]
    public void BinaryTree_Delete_Case3_TwoKids_Right_Success()
    {
        //                       15                              20                                 15
        //          10                         20                ==>                   10                         30
        //     8          12             18          30                           8          12             18
        //            11      14      16    19                                           11      14      16    19
        //
        var binTree8  = new TreeFactory().CreateTree<BinaryTreeInt>(label: "8");
        var binTree10 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "10");
        var binTree11 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "11");
        var binTree12 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "12");
        var binTree14 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "14");
        var binTree15 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "15");
        var binTree16 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "16");
        var binTree18 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "18");
        var binTree19 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "19");
        var binTree20 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "20");
        var binTree30 = new TreeFactory().CreateTree<BinaryTreeInt>(label: "30");

        BinaryTreeInt.Insert(binTree10, binTree15);
        BinaryTreeInt.Insert(binTree20, binTree15);
        BinaryTreeInt.Insert(binTree8, binTree15);
        BinaryTreeInt.Insert(binTree12, binTree15);
        BinaryTreeInt.Insert(binTree11, binTree15);
        BinaryTreeInt.Insert(binTree14, binTree15);
        BinaryTreeInt.Insert(binTree18, binTree15);
        BinaryTreeInt.Insert(binTree30, binTree15);
        BinaryTreeInt.Insert(binTree16, binTree15);
        BinaryTreeInt.Insert(binTree19, binTree15);

        Assert.That(binTree15.Papa, Is.Null);
        Assert.That(binTree15.Left, Is.EqualTo(binTree10));
        Assert.That(binTree15.Right, Is.EqualTo(binTree20));

        Assert.That(binTree10.Papa, Is.EqualTo(binTree15));
        Assert.That(binTree10.Left, Is.EqualTo(binTree8));
        Assert.That(binTree10.Right, Is.EqualTo(binTree12));

        Assert.That(binTree20.Papa, Is.EqualTo(binTree15));
        Assert.That(binTree20.Left, Is.EqualTo(binTree18));
        Assert.That(binTree20.Right, Is.EqualTo(binTree30));

        Assert.That(binTree8.Papa, Is.EqualTo(binTree10));
        Assert.That(binTree8.Left, Is.Null);
        Assert.That(binTree8.Right, Is.Null);

        Assert.That(binTree12.Papa, Is.EqualTo(binTree10));
        Assert.That(binTree12.Left, Is.EqualTo(binTree11));
        Assert.That(binTree12.Right, Is.EqualTo(binTree14));

        Assert.That(binTree18.Papa, Is.EqualTo(binTree20));
        Assert.That(binTree18.Left, Is.EqualTo(binTree16));
        Assert.That(binTree18.Right, Is.EqualTo(binTree19));

        Assert.That(binTree30.Papa, Is.EqualTo(binTree20));
        Assert.That(binTree30.Left, Is.Null);
        Assert.That(binTree30.Right, Is.Null);

        Assert.That(binTree11.Papa, Is.EqualTo(binTree12));
        Assert.That(binTree11.Left, Is.Null);
        Assert.That(binTree11.Right, Is.Null);

        Assert.That(binTree14.Papa, Is.EqualTo(binTree12));
        Assert.That(binTree14.Left, Is.Null);
        Assert.That(binTree14.Right, Is.Null);

        Assert.That(binTree16.Papa, Is.EqualTo(binTree18));
        Assert.That(binTree16.Left, Is.Null);
        Assert.That(binTree16.Right, Is.Null);

        Assert.That(binTree19.Papa, Is.EqualTo(binTree18));
        Assert.That(binTree19.Left, Is.Null);
        Assert.That(binTree19.Right, Is.Null);

        var deletedNode = BinaryTreeInt.Delete(binTree20.GetKey(), binTree15);
        Assert.That(deletedNode?.Left, Is.Null);
        Assert.That(deletedNode?.Right, Is.Null);
        Assert.That(deletedNode?.Papa, Is.Null);
        Assert.That(BinaryTreeInt.Validate(binTree15), Is.True);

        Assert.That(binTree15.Papa, Is.Null);
        Assert.That(binTree15.Left, Is.EqualTo(binTree10));
        Assert.That(binTree15.Right, Is.EqualTo(binTree30));

        Assert.That(binTree10.Papa, Is.EqualTo(binTree15));
        Assert.That(binTree10.Left, Is.EqualTo(binTree8));
        Assert.That(binTree10.Right, Is.EqualTo(binTree12));

        Assert.That(binTree30.Papa, Is.EqualTo(binTree15));
        Assert.That(binTree30.Left, Is.EqualTo(binTree18));
        Assert.That(binTree30.Right, Is.Null);

        Assert.That(binTree20.Papa, Is.Null);
        Assert.That(binTree20.Left, Is.Null);
        Assert.That(binTree20.Right, Is.Null);

        Assert.That(binTree8.Papa, Is.EqualTo(binTree10));
        Assert.That(binTree8.Left, Is.Null);
        Assert.That(binTree8.Right, Is.Null);

        Assert.That(binTree12.Papa, Is.EqualTo(binTree10));
        Assert.That(binTree12.Left, Is.EqualTo(binTree11));
        Assert.That(binTree12.Right, Is.EqualTo(binTree14));

        Assert.That(binTree18.Papa, Is.EqualTo(binTree30));
        Assert.That(binTree18.Left, Is.EqualTo(binTree16));
        Assert.That(binTree18.Right, Is.EqualTo(binTree19));

        Assert.That(binTree11.Papa, Is.EqualTo(binTree12));
        Assert.That(binTree11.Left, Is.Null);
        Assert.That(binTree11.Right, Is.Null);

        Assert.That(binTree14.Papa, Is.EqualTo(binTree12));
        Assert.That(binTree14.Left, Is.Null);
        Assert.That(binTree14.Right, Is.Null);

        Assert.That(binTree16.Papa, Is.EqualTo(binTree18));
        Assert.That(binTree16.Left, Is.Null);
        Assert.That(binTree16.Right, Is.Null);

        Assert.That(binTree19.Papa, Is.EqualTo(binTree18));
        Assert.That(binTree19.Left, Is.Null);
        Assert.That(binTree19.Right, Is.Null);

        PruneTree([ binTree8, binTree10, binTree11, binTree12, binTree14, binTree15, binTree16, binTree18, binTree19, binTree20, binTree30 ]);
        Assert.That(binTree15.Papa, Is.Null);
        Assert.That(binTree15.Leaf, Is.True);
    }

    [Test]
    public async Task BinaryTree_Random_Success()
    {
        int start = 100;
        int end = 1000;

        int count = 1000;

        for(int i = 0; i < count; i++)
        {
            List<BinaryTreeInt> nodes = new();

            var rootLabel = $"{RandomNumberGenerator.GetInt32(start, end)}";
            var binTreeRoot  = new TreeFactory().CreateTree<BinaryTreeInt>(label: rootLabel);

            nodes.Add(binTreeRoot);

            for(int k = 0; k < count; k++)
            {
                var nodeLabel = $"{RandomNumberGenerator.GetInt32(start, end)}";
                var binTree = new TreeFactory().CreateTree<BinaryTreeInt>(label: nodeLabel);

                if(BinaryTreeInt.Insert(binTree, binTreeRoot))
                    nodes.Add(binTree);

                Assert.That(BinaryTreeInt.Validate(binTreeRoot), Is.True);
            }

            //const string fileName = "BinaryTree_Random_Success";
            //await TreeSerialization.SerializeTree(DotDirectory, $"{fileName}.dot", binTreeRoot, digraph: true);
            await Task.CompletedTask;

            PruneTree(nodes);

            Assert.That(binTreeRoot.Papa, Is.Null);
            Assert.That(binTreeRoot.Leaf, Is.True);
        }
    }

    [Test]
    public void RedBlackTree_Success()
    {
        var rbTree = new TreeFactory().CreateTree<RedBlackTreeInt>(label: "1");
    }

    [Test]
    public void RedBlackTree_Random_Success()
    {
        int start = 100;
        int end = 1000;

        int count = 1000;

        for(int i = 0; i < count; i++)
        {
            List<RedBlackTreeInt> nodes = new();

            var rootLabel = $"{RandomNumberGenerator.GetInt32(start, end)}";
            var binTreeRoot  = new TreeFactory().CreateTree<RedBlackTreeInt>(label: rootLabel);

            nodes.Add(binTreeRoot);

            for(int k = 0; k < count; k++)
            {
                var nodeLabel = $"{RandomNumberGenerator.GetInt32(start, end)}";
                var binTree = new TreeFactory().CreateTree<RedBlackTreeInt>(label: nodeLabel);

                if(RedBlackTreeInt.Insert(binTree, binTreeRoot))
                    nodes.Add(binTree);

                Assert.That(RedBlackTreeInt.Validate(binTreeRoot), Is.True);
            }

            PruneRedBlackTree(nodes);

            Assert.That(binTreeRoot.Papa, Is.Null);
            Assert.That(binTreeRoot.Leaf, Is.True);
        }
    }

    [Test]
    public void AvlTree_Success()
    {
        var avlTree = new TreeFactory().CreateTree<AvlTreeInt>(label: "1");
    }

    [Test]
    public void AvlTree_Random_Success()
    {
        int start = 100;
        int end = 1000;

        int count = 1000;

        for(int i = 0; i < count; i++)
        {
            List<AvlTreeInt> nodes = new();

            var rootLabel = $"{RandomNumberGenerator.GetInt32(start, end)}";
            var binTreeRoot  = new TreeFactory().CreateTree<AvlTreeInt>(label: rootLabel);

            nodes.Add(binTreeRoot);

            for(int k = 0; k < count; k++)
            {
                var nodeLabel = $"{RandomNumberGenerator.GetInt32(start, end)}";
                var binTree = new TreeFactory().CreateTree<AvlTreeInt>(label: nodeLabel);

                if(AvlTreeInt.Insert(binTree, binTreeRoot))
                    nodes.Add(binTree);

                Assert.That(RedBlackTreeInt.Validate(binTreeRoot), Is.True);
            }

            PruneAvlTree(nodes);

            Assert.That(binTreeRoot.Papa, Is.Null);
            Assert.That(binTreeRoot.Leaf, Is.True);
        }
    }
}
