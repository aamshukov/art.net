//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;
using UILab.Art.Framework.Adt.Tree;

namespace UILab.Art.Tests;

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

    protected override string GetKey()
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

    protected override string GetKey()
    {
        return Label;
    }
}

[TestFixture]
internal class TreeTests
{
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
