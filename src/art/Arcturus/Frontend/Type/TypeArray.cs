//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Arcturus.Infrastructure;
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Arcturus.Typing;

public class TypeArray : Type
{
    public Type UnderlyingType { get; init; }

    public List<TypeArrayBound> Bounds { get; init; }

    public TypeArray(id id,
                     TypeKind kind,
                     Type underlyingType,
                     List<TypeArrayBound> bounds,
                     TypeLayout? typeLayout = default, // applies to all elements of the array
                     string? label = default,
                     object? value = default,
                     Flags flags = Flags.Clear,
                     Dictionary<string, object>? attributes = default,
                     string? version = default) : base(id: id,
                                                       kind: kind | TypeKind.ArrayMask,
                                                       cardinality: bounds.Count,
                                                       typeLayout: typeLayout,
                                                       label: label,
                                                       value: value,
                                                       attributes: attributes, version: version)
    {
        Assert.NonNullReference(underlyingType, nameof(underlyingType));
        Assert.NonEmptyCollection<TypeArrayBound>(bounds, nameof(bounds));

        UnderlyingType = underlyingType;
        Bounds = bounds;
    }

    public override bool Equivalent(Type other)
    {
        Assert.NonNullReference(other, nameof(other));
        return Array() && other.Array() && base.Equivalent(other) && Bounds == ((TypeArray)other).Bounds;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
        yield return Bounds;
    }

    public override TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param = default)
        where TResult : default
        where TParam : default
    {
        Assert.NonNullReference(visitor, nameof(visitor));
        return visitor.Visit<TParam, TResult>(this, param);
    }
}
