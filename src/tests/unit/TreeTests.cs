//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;
using UILab.Art.Framework.Adt.Tree;

namespace UILab.Art.Tests;

internal class BinaryTreeStr : BinaryTree<string>
{
    public BinaryTreeStr(id id,
                         Tree? papa = default,
                         string? label = default,
                         object? value = default,
                         Flags flags = Flags.Clear,
                         Color color = Color.Unknown,
                         Dictionary<string, object>? attributes = default,
                         string? version = default) : base(id, papa, label, value, flags, color, attributes, version)
    {
    }

    public override string GetKey()
    {
        return Label;
    }
}

internal class RedBlackTreeStr : RedBlackTree<string>
{
    public RedBlackTreeStr(id id,
                           Tree? papa = default,
                           string? label = default,
                           object? value = default,
                           Flags flags = Flags.Clear,
                           Color color = Color.Unknown,
                           Dictionary<string, object>? attributes = default,
                           string? version = default) : base(id, papa, label, value, flags, color, attributes, version)
    {
    }

    public override string GetKey()
    {
        return Label;
    }
}

internal class AvlTreeStr : AvlTree<string>
{
    public AvlTreeStr(id id,
                      Tree? papa = default,
                      string? label = default,
                      object? value = default,
                      Flags flags = Flags.Clear,
                      Color color = Color.Unknown,
                      Dictionary<string, object>? attributes = default,
                      string? version = default) : base(id, papa, label, value, flags, color, attributes, version)
    {
    }

    public override string GetKey()
    {
        return Label;
    }
}

[TestFixture]
internal class TreeTests
{
    [Test]
    public void BinaryTree_Delete_Case0_NotFound_Success()
    {
        var binTree2 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "2");
        var binTree3 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "3");
        var binTree4 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "4");

        BinaryTreeStr.Insert(binTree3, binTree4);
        Assert.That(binTree4.Left, Is.EqualTo(binTree3));
        Assert.That(binTree3.Papa, Is.EqualTo(binTree4));

        BinaryTreeStr.Delete(binTree2.GetKey(), binTree4);
        Assert.That(binTree4.Left, Is.EqualTo(binTree3));
        Assert.That(binTree3.Papa, Is.EqualTo(binTree4));
    }

    [Test]
    public void BinaryTree_Delete_Case1_Leaf_Success()
    {
        var binTree2 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "2");
        var binTree3 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "3");
        var binTree4 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "4");
        var binTree5 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "5");
        var binTree6 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "6");
        var binTree7 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "7");
        var binTree8 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "8");

        BinaryTreeStr.Insert(binTree3, binTree5);
        BinaryTreeStr.Insert(binTree7, binTree5);
        BinaryTreeStr.Insert(binTree2, binTree3);
        BinaryTreeStr.Insert(binTree4, binTree3);
        BinaryTreeStr.Insert(binTree6, binTree7);
        BinaryTreeStr.Insert(binTree8, binTree7);
        Assert.That(binTree5.Left, Is.EqualTo(binTree3));
        Assert.That(binTree5.Right, Is.EqualTo(binTree7));
        Assert.That(binTree3.Left, Is.EqualTo(binTree2));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));
        Assert.That(binTree7.Left, Is.EqualTo(binTree6));
        Assert.That(binTree7.Right, Is.EqualTo(binTree8));

        BinaryTreeStr.Delete(binTree2.GetKey(), binTree5);
        Assert.That(binTree5.Left, Is.EqualTo(binTree3));
        Assert.That(binTree5.Right, Is.EqualTo(binTree7));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));
        Assert.That(binTree7.Left, Is.EqualTo(binTree6));
        Assert.That(binTree7.Right, Is.EqualTo(binTree8));
    }

    [Test]
    public void BinaryTree_Delete_Case2_LeftKid_Success()
    {
        var binTree2 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "2");
        var binTree3 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "3");
        var binTree4 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "4");
        var binTree5 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "5");
        var binTree6 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "6");
        var binTree7 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "7");
        var binTree8 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "8");

        BinaryTreeStr.Insert(binTree3, binTree5);
        BinaryTreeStr.Insert(binTree7, binTree5);
        BinaryTreeStr.Insert(binTree2, binTree3);
        BinaryTreeStr.Insert(binTree4, binTree3);
        BinaryTreeStr.Insert(binTree6, binTree7);
        BinaryTreeStr.Insert(binTree8, binTree7);
        Assert.That(binTree5.Left, Is.EqualTo(binTree3));
        Assert.That(binTree5.Right, Is.EqualTo(binTree7));
        Assert.That(binTree3.Left, Is.EqualTo(binTree2));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));
        Assert.That(binTree7.Left, Is.EqualTo(binTree6));
        Assert.That(binTree7.Right, Is.EqualTo(binTree8));

        BinaryTreeStr.Delete(binTree3.GetKey(), binTree5);
        Assert.That(binTree5.Left, Is.EqualTo(binTree4));
        Assert.That(binTree5.Right, Is.EqualTo(binTree7));
        Assert.That(binTree4.Left, Is.EqualTo(binTree2));
        Assert.That(binTree7.Left, Is.EqualTo(binTree6));
        Assert.That(binTree7.Right, Is.EqualTo(binTree8));
    }

    [Test]
    public void BinaryTree_Delete_Case2_RightKid_Success()
    {
        var binTree2 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "2");
        var binTree3 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "3");
        var binTree4 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "4");
        var binTree5 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "5");
        var binTree6 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "6");
        var binTree7 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "7");
        var binTree8 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "8");

        BinaryTreeStr.Insert(binTree3, binTree5);
        BinaryTreeStr.Insert(binTree7, binTree5);
        BinaryTreeStr.Insert(binTree2, binTree3);
        BinaryTreeStr.Insert(binTree4, binTree3);
        BinaryTreeStr.Insert(binTree6, binTree7);
        BinaryTreeStr.Insert(binTree8, binTree7);
        Assert.That(binTree5.Left, Is.EqualTo(binTree3));
        Assert.That(binTree5.Right, Is.EqualTo(binTree7));
        Assert.That(binTree3.Left, Is.EqualTo(binTree2));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));
        Assert.That(binTree7.Left, Is.EqualTo(binTree6));
        Assert.That(binTree7.Right, Is.EqualTo(binTree8));

        BinaryTreeStr.Delete(binTree3.GetKey(), binTree5);
        Assert.That(binTree5.Left, Is.EqualTo(binTree4));
        Assert.That(binTree5.Right, Is.EqualTo(binTree7));
        Assert.That(binTree4.Left, Is.EqualTo(binTree2));
        Assert.That(binTree7.Left, Is.EqualTo(binTree6));
        Assert.That(binTree7.Right, Is.EqualTo(binTree8));
    }

    [Test]
    public void BinaryTree_Delete_Case3_TwoKids_Root_Success()
    {
        var binTree2 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "2");
        var binTree3 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "3");
        var binTree4 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "4");
        var binTree5 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "5");
        var binTree6 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "6");
        var binTree7 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "7");
        var binTree8 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "8");
        var binTree9 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "9");

        BinaryTreeStr.Insert(binTree3, binTree5);
        BinaryTreeStr.Insert(binTree8, binTree5);
        BinaryTreeStr.Insert(binTree2, binTree3);
        BinaryTreeStr.Insert(binTree4, binTree3);
        BinaryTreeStr.Insert(binTree6, binTree8);
        BinaryTreeStr.Insert(binTree9, binTree8);
        BinaryTreeStr.Insert(binTree7, binTree6);
        Assert.That(binTree5.Left, Is.EqualTo(binTree3));
        Assert.That(binTree5.Right, Is.EqualTo(binTree8));
        Assert.That(binTree3.Left, Is.EqualTo(binTree2));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));
        Assert.That(binTree8.Left, Is.EqualTo(binTree6));
        Assert.That(binTree8.Right, Is.EqualTo(binTree9));
        Assert.That(binTree6.Right, Is.EqualTo(binTree7));

        BinaryTreeStr.Delete(binTree5.GetKey(), binTree5);
        Assert.That(binTree6.Left, Is.EqualTo(binTree3));
        Assert.That(binTree6.Right, Is.EqualTo(binTree8));
        Assert.That(binTree3.Left, Is.EqualTo(binTree2));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));
        Assert.That(binTree8.Left, Is.EqualTo(binTree7));
        Assert.That(binTree8.Right, Is.EqualTo(binTree9));
    }

    [Test]
    public void BinaryTree_Delete_Case3_TwoKids_Success()
    {
        var binTree2 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "2");
        var binTree3 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "3");
        var binTree4 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "4");
        var binTree5 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "5");
        var binTree6 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "6");
        var binTree7 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "7");
        var binTree8 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "8");
        var binTree9 = new TreeFactory().CreateTree<BinaryTreeStr>(label: "9");

        BinaryTreeStr.Insert(binTree3, binTree5);
        BinaryTreeStr.Insert(binTree8, binTree5);
        BinaryTreeStr.Insert(binTree2, binTree3);
        BinaryTreeStr.Insert(binTree4, binTree3);
        BinaryTreeStr.Insert(binTree6, binTree8);
        BinaryTreeStr.Insert(binTree9, binTree8);
        BinaryTreeStr.Insert(binTree7, binTree6);
        Assert.That(binTree5.Left, Is.EqualTo(binTree3));
        Assert.That(binTree5.Right, Is.EqualTo(binTree8));
        Assert.That(binTree3.Left, Is.EqualTo(binTree2));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));
        Assert.That(binTree8.Left, Is.EqualTo(binTree6));
        Assert.That(binTree8.Right, Is.EqualTo(binTree9));
        Assert.That(binTree6.Right, Is.EqualTo(binTree7));

        BinaryTreeStr.Delete(binTree5.GetKey(), binTree5);
        Assert.That(binTree6.Left, Is.EqualTo(binTree3));
        Assert.That(binTree6.Right, Is.EqualTo(binTree8));
        Assert.That(binTree3.Left, Is.EqualTo(binTree2));
        Assert.That(binTree3.Right, Is.EqualTo(binTree4));
        Assert.That(binTree8.Left, Is.EqualTo(binTree7));
        Assert.That(binTree8.Right, Is.EqualTo(binTree9));
    }

    [Test]
    public void RedBlackTree_Success()
    {
        var rbTree = new TreeFactory().CreateTree<RedBlackTreeStr>(label: "1");
    }

    [Test]
    public void AvlTree_Success()
    {
        var avlTree = new TreeFactory().CreateTree<AvlTreeStr>(label: "1");
    }
}
