//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Adt.Graph;

namespace UILab.Art.Arcturus.Ir;

public class BasicBlock<TOperation, TSymbol> : DominatorVertex
    where TOperation : notnull
    where TSymbol : notnull
{
    public Code<TOperation> Code { get; init; }

    public List<TSymbol> Ins { get; init; }

    public List<TSymbol> Outs { get; init; }

    public List<TSymbol> Defs { get; init; }

    public List<TSymbol> Uses { get; init; }

    public BasicBlock(id id,
                      Code<TOperation>? code = default,
                      DominatorVertex? idominator = default,
                      string? label = default,
                      object? value = default,
                      Flags flags = Flags.Clear,
                      Color color = Color.Unknown,
                      Dictionary<string, object>? attributes = default,
                      string? version = default) : base(id, idominator, label, value, flags, color, attributes, version)
    {
        Code = code ?? new();

        Ins = new();
        Outs = new();

        Defs = new();
        Uses = new();
    }
}
