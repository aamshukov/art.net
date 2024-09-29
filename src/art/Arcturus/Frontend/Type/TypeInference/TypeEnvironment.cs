//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Arcturus.Typing;

public sealed class TypeEnvironment
{
    /// <summary>
    /// Mapping of variable names and their inferred type (variable names to inferred type).
    /// </summary>
    private Dictionary<string, Type> Environments { get; init; }

    public TypeEnvironment()
    {
        Environments = new();
    }
}
