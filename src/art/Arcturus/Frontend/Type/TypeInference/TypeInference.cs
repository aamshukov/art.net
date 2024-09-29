//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Counter;

namespace UILab.Art.Arcturus.Typing;

public sealed class TypeInference
{
    private Counter TypeNameCounter { get; init; }

    public TypeInference()
    {
        TypeNameCounter = new();
    }

    public void Infer()
    {
    }

    public void Unify()
    {
    }

    private string GenerateTypeName()
    {
        return $"T:{TypeNameCounter.NextId()}";
    }
}
