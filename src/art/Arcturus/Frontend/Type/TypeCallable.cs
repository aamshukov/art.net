//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Arcturus.Infrastructure;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain.Abstractions;

namespace UILab.Art.Arcturus.Type;

public abstract class TypeCallable : TypeScalar
{
    /// <summary>
    /// Gets type params: <T, R, P, ...>.
    /// </summary>
    public List<Type> TypeParams { get; init; }

    /// <summary>
    /// Gets value params integer, real, ...
    /// </summary>
    public List<Type> FormalParams { get; init; }

    public Type? Result { get; init; }

    /// <summary>
    /// Has return value, otherwise procedure.
    /// </summary>
    public bool Function { get; init; }

    public bool Variadic { get; init; }

    public TypeCallable(id id,
                        TypeKind kind,
                        List<Type>? typeParams = default,
                        List<Type>? formalParams = default,
                        Type? result = default,
                        bool function = true,
                        bool variadic = false,
                        TypeLayout? typeLayout = default,
                        string? label = default,
                        object? value = default,
                        Flags flags = Flags.Clear,
                        Dictionary<string, object>? attributes = default,
                        string? version = default) : base(id: id,
                                                          kind: kind | TypeKind.CallableMask,
                                                          typeLayout: typeLayout,
                                                          label: label,
                                                          value: value,
                                                          attributes: attributes, version: version)
    {
        TypeParams = typeParams ?? new();
        FormalParams = formalParams ?? new();
        Result = result;
        Function = function;
        Variadic = variadic;
    }

    public override bool Equivalent(Type other)
    {
        Assert.NonNullReference(other, nameof(other));

        bool result = Callable() &&
                      other.Callable() &&
                      base.Equivalent(other) &&
                      TypeParams == ((TypeCallable)other).TypeParams &&
                      FormalParams == ((TypeCallable)other).FormalParams &&
                      Variadic == ((TypeCallable)other).Variadic;

        if(Result is not null && ((TypeCallable)other).Result is not null)
        {
            result = Result == ((TypeCallable)other).Result!;
        }

        return result;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;

        yield return TypeParams;
        yield return FormalParams;

        if(Result is not null)
            yield return Result;

        yield return Variadic;
    }

    public override TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param = default)
        where TResult : default
        where TParam : default
    {
        Assert.NonNullReference(visitor, nameof(visitor));
        return visitor.Visit<TParam, TResult>(this, param);
    }
}
