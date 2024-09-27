//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Arcturus.Ir.Instructions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Arcturus.Ir;

public sealed class Code<TOperation> where TOperation : notnull
{
    public List<Instruction<TOperation>> Instructions { get; init; }

    public Code()
    {
        Instructions = new();
    }

    public void AddInstruction(Instruction<TOperation> instruction)
    {
        Assert.NonNullReference(instruction, nameof(instruction));
        Instructions.Add(instruction);
    }

    public void RemoveInstruction(Instruction<TOperation> instruction)
    {
        Assert.NonNullReference(instruction, nameof(instruction));
        Instructions.Remove(instruction);
    }

    public void ClearInstructions()
    {
        Instructions.Clear();
    }
}
