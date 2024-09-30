//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Tree;

namespace UILab.Art.Arcturus.SymTable;

public sealed class Scope : Tree
{
    public ScopeKind Kind { get; init; }

    public int Level { get; init; }

    public Dictionary<string, Symbol> Symbols { get; init; }

    /// <summary>
    /// Introduced types.
    /// </summary>
    public Dictionary<id, Type> Types { get; init; }

    public Dictionary<id, Type> Declarations { get; init; }

    public Scope(id id,
                 ScopeKind kind,
                 Tree? papa = default,
                 string? label = default,
                 Dictionary<string, object>? attributes = default,
                 string? version = default) : base(id: id,
                                                   papa: papa,
                                                   label: label,
                                                   attributes: attributes,
                                                   version: version)
    {
        Kind = kind;
        Symbols = new();
        Types = new();
        Declarations = new();
    }
}
