//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Statistics;

namespace UILab.Art.Arcturus.SymTable;

public sealed class SymbolTable
{
    /// <summary>
    /// Gets the root of the scope tree, might represent 'global' scope.
    /// </summary>
    public Scope Root { get; init; }

    /// <summary>
    /// Gets current scope, level.
    /// </summary>
    public Scope Scope { get; init; }

    public Statistics Statistics { get; init; }

    public Diagnostics Diagnostics { get; init; }


    public SymbolTable(Scope root, Statistics statistics, Diagnostics diagnostics)
    {
        Assert.NonNullReference(root, nameof(root));
        Assert.NonNullReference(statistics, nameof(statistics));
        Assert.NonNullReference(diagnostics, nameof(diagnostics));

        Root = root;
        Scope = root;

        Statistics = statistics;
        Diagnostics = diagnostics;
    }
}
