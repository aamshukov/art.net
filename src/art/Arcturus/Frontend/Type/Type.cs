//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Arcturus.Infrastructure;
using UILab.Art.Framework.Adt.Tree;
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Arcturus.Typing;

public abstract class Type : Tree
{
    public TypeKind Kind { get; init; }

    /// <summary>
    /// Gets Cardinality: scalar (0), vector/1D-array(1), matrix/2D-array(2), etc.
    /// </summary>
    public size Cardinality { get; init; }

    public TypeLayout Layout { get; init; }

    public new Flags Flags { get; set; }

    public Type(id id,
                TypeKind kind,
                size cardinality = 0,
                TypeLayout? typeLayout = default,
                string? label = default,
                object? value = default,
                Flags flags = Flags.Clear,
                Dictionary<string, object>? attributes = default,
                string? version = default) : base(id, label: label, value: value, attributes: attributes, version: version)
    {
        Kind = kind;
        Cardinality = cardinality;
        Layout = typeLayout ?? new();
        Flags = flags;
    }

    public virtual bool Equivalent(Type other)
    {
        Assert.NonNullReference(other);
        return Id == other.Id && Kind == other.Kind && Cardinality == other.Cardinality && Label == other.Label;
    }

    public bool Builtin()
    {
        return (Kind & TypeKind.BuiltinMask) == TypeKind.BuiltinMask;
    }

    public bool Integer()
    {
        return (Kind & TypeKind.Integer) == TypeKind.Integer;
    }

    public bool Real()
    {
        return (Kind & TypeKind.Real) == TypeKind.Real;
    }

    public bool String()
    {
        return (Kind & TypeKind.String) == TypeKind.String;
    }

    public bool Boolean()
    {
        return (Kind & TypeKind.Boolean) == TypeKind.Boolean;
    }

    public bool Scalar()
    {
        return (Kind & TypeKind.ScalarMask) == TypeKind.ScalarMask;
    }

    public bool Array()
    {
        return (Kind & TypeKind.ArrayMask) == TypeKind.ArrayMask;
    }

    public bool Composite()
    {
        return (Kind & TypeKind.CompositeMask) == TypeKind.CompositeMask;
    }

    public bool Subtype()
    {
        return (Kind & TypeKind.SubtypeMask) == TypeKind.SubtypeMask;
    }

    public bool Callable()
    {
        return (Kind & TypeKind.CallableMask) == TypeKind.CallableMask;
    }

    public bool Generic()
    {
        return (Kind & TypeKind.GenericMask) == TypeKind.GenericMask;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;

        yield return Kind;
        yield return Layout;
        yield return Cardinality;
    }

    public override TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param = default)
        where TResult : default
        where TParam : default
    {
        Assert.NonNullReference(visitor);
        return visitor.Visit<TParam, TResult>(this, param);
    }
}
