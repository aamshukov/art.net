//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Arcturus.Infrastructure;
using UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Arcturus.Ir.Instructions;

public class Quadruple<TOperation, TSymbol> : Instruction<TOperation>
    where TOperation : notnull
    where TSymbol : notnull
{
    public Argument<TSymbol>? Argument1 { get; init; }

    public Argument<TSymbol>? Argument2 { get; init; }

    /// <summary>
    /// One of: Argument, Quadruple, Phi
    /// </summary>
    public object? Result { get; init; }

    public Quadruple(id id,
                     TOperation operation,
                     Argument<TSymbol>? argument1 = default,
                     Argument<TSymbol>? argument2 = default,
                     object? result = default,
                     string? label = default,
                     Flags flags = Flags.Clear,
                     string? version = default) : base(id, operation, label, flags, version)
    {
        Argument1 = argument1;
        Argument2 = argument2;
        Result = result;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;

        if(Argument1 is not null)
            yield return Argument1;

        if(Argument2 is not null)
            yield return Argument2;

        if(Result is not null)
            yield return Result;
    }

    public override TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param = default)
        where TResult : default
        where TParam : default
    {
        Assert.NonNullReference(visitor);
        return visitor.Visit<TParam, TResult>(this, param);
    }
}
