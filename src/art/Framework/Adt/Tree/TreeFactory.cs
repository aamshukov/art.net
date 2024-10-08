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

    public id GetNextId() => TreeCounter.NextId();

    public TTree CreateTree<TTree>(Tree? papa = default,
                                   string? label = default,
                                   object? value = default,
                                   Flags flags = Flags.Clear,
                                   Color color = Color.Unknown,
                                   Dictionary<string, object>? attributes = default,
                                   string? version = default)
    {
        return (TTree)Activator.CreateInstance(type: typeof(TTree),
                                               args: [GetNextId(), papa, label, value, flags, color, attributes, version])!;
    }

    public TTree CreateTree<TTree>(object[] args)
    {
        return (TTree)Activator.CreateInstance(type: typeof(TTree), args: args)!;
    }
}
