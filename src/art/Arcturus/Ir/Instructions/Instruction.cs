//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Arcturus.Infrastructure;
using UILab.Art.Framework.Core.Domain;
using UILab.Art.Framework.Core.Domain.Abstractions;

namespace UILab.Art.Arcturus.Ir.Instructions;

public abstract class Instruction<TOperation> : EntityType<id>, IVisitable
    where TOperation : notnull
{
    public TOperation Operation { get; init; }

    public string Label { get; init; }

    public Flags Flags { get; set; }

    public Instruction(id id,
                       TOperation operation,
                       string? label = default,
                       Flags flags = Flags.Clear,
                       string? version = default) : base(id, version)
    {
        Operation = operation;
        Label = label?.Trim() ?? $"V:{id.ToString()}";
        Flags = flags;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach (var component in base.GetEqualityComponents())
            yield return component;
        yield return Operation;
    }

    public abstract TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param);
}
