//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Arcturus.Infrastructure;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain.Abstractions;

namespace UILab.Art.Arcturus.Type;

public abstract class TypeScalar : Type
{
    public TypeScalar(id id,
                      TypeKind kind,
                      TypeLayout? typeLayout = default,
                      string? label = default,
                      object? value = default,
                      Flags flags = Flags.Clear,
                      Dictionary<string, object>? attributes = default,
                      string? version = default) : base(id: id,
                                                        kind: kind | TypeKind.ScalarMask,
                                                        cardinality: 0,
                                                        typeLayout: typeLayout,
                                                        label: label,
                                                        value: value,
                                                        attributes: attributes, version: version)
    {
    }

    public override bool Equivalent(Type other)
    {
        Assert.NonNullReference(other, nameof(other));
        return Scalar() && other.Scalar() && base.Equivalent(other);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
    }

    public override TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param = default)
        where TResult : default
        where TParam : default
    {
        Assert.NonNullReference(visitor, nameof(visitor));
        return visitor.Visit<TParam, TResult>(this, param);
    }
}
