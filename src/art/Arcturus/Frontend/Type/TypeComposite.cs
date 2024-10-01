//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Arcturus.Infrastructure;
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Arcturus.Typing;

public class TypeComposite : TypeScalar
{
    public List<Type> Members { get; init; }

    public TypeComposite(id id,
                         TypeKind kind,
                         List<Type>? members = default,
                         TypeLayout? typeLayout = default,
                         string? label = default,
                         object? value = default,
                         Flags flags = Flags.Clear,
                         Dictionary<string, object>? attributes = default,
                         string? version = default) : base(id: id,
                                                           kind: kind | TypeKind.CompositeMask,
                                                           typeLayout: typeLayout,
                                                           label: label,
                                                           value: value,
                                                           attributes: attributes, version: version)
    {
        Members = members ?? new();
    }

    public override bool Equivalent(Type other)
    {
        Assert.NonNullReference(other, nameof(other));
        return Composite() && other.Composite() && base.Equivalent(other) && Members == ((TypeComposite)other).Members;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
        yield return Members;
    }

    public override TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param = default)
        where TResult : default
        where TParam : default
    {
        Assert.NonNullReference(visitor, nameof(visitor));
        return visitor.Visit<TParam, TResult>(this, param);
    }
}
