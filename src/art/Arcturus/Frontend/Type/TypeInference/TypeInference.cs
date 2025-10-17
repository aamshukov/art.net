//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Arcturus.SymTable;
using UILab.Art.Framework.Core.Counter;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Arcturus.Typing;

public sealed class TypeInference
{
    private Counter TypeNameCounter { get; init; }

    public TypeInference(SymbolTable symTable)
    {
        Assert.NonNullReference(symTable);
        TypeNameCounter = new();
    }

    public void Infer(IEnumerable<Type> types)
    {
    }

    private void AssignTypenames(IEnumerable<Symbol> symbols, TypeEnvironment environment)
    {
    }

    private void GenerateConstrains(IEnumerable<Symbol> symbols, TypeEnvironment environment)
    {
    }

    private void Unify(IEnumerable<Symbol> symbols, TypeEnvironment environment)
    {
    }

    private string GenerateTypeName()
    {
        return $"T:{TypeNameCounter.NextId()}";
    }
}
