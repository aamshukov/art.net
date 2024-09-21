//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Counter;
using UILab.Art.Framework.Core.Domain;
using UILab.Art.Framework.Core.Domain.Abstractions;

namespace UILab.Art.Framework.Adt.Graph;

public abstract class Vertex : EntityType<id>, IVisitable
{
    public string Label { get; init; }

    public object? Value { get; set; }

    public Flags Flags { get; set; }

    public Color Color { get; set; }

    public Dictionary<string, object> Attributes { get; init; }

    public ReferenceCounter ReferenceCounter { get; init; }

    public Vertex(id id,
                  string? label = default,
                  object? value = default,
                  Flags flags = Flags.Clear,
                  Color color = Color.Unknown,
                  Dictionary<string, object>? attributes = default,
                  string? version = default) : base(id, version)
    {
        Label = label?.Trim() ?? $"V:{id.ToString()}";
        Value = value;
        Flags = flags;
        Color = color;
        Attributes = attributes ?? new();
        ReferenceCounter = new();
    }

    public count AddReference() => ReferenceCounter.AddReference();

    public bool CanRelease() => ReferenceCounter.CanRelease();

    public count Release() => ReferenceCounter.Release();

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
        yield return Label;
        if(Value is not null)
            yield return Value;
    }

    public abstract TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param);
}
