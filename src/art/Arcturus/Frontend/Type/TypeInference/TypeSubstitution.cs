//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Arcturus.Typing;

public sealed class TypeSubstitution
{
    /// <summary>
    /// Mapping of type variables to types.
    /// </summary>
    private Dictionary<string, Type> Substitution { get; init; }

    public TypeSubstitution()
    {
        Substitution = new();
    }
}
