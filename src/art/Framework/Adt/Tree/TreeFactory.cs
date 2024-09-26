//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;
using UILab.Art.Framework.Core.Counter;

namespace UILab.Art.Framework.Adt.Tree;

public sealed class TreeFactory
{
    private Counter TreeCounter { get; init; }

    public TreeFactory()
    {
        TreeCounter = new();
    }

    public Tree CreateTree(Tree? papa = default,
                           string? label = default,
                           object? value = default,
                           Flags flags = Flags.Clear,
                           Color color = Color.Unknown,
                           Dictionary<string, object>? attributes = default,
                           string? version = default)
    {
        return new(TreeCounter.NextId(), papa, label, value, flags, color, attributes, version);
    }
}
