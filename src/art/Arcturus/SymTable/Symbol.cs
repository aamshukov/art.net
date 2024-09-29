//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Arcturus.Infrastructure;
using UILab.Art.Arcturus.LexicalAnalyzer.Tokenizer;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;
using UILab.Art.Framework.Core.Domain.Abstractions;
using Type = UILab.Art.Arcturus.Typing.Type;

namespace UILab.Art.Arcturus.SymTable;

public abstract class Symbol : EntityType<id>, IVisitable
{
    public string Name { get; init; }

    public string SyntheticName { get; set; }

    /// <summary>
    /// Gets synthetic/generated id, might be introduced by SSA, etc.
    /// </summary>
    public id SyntheticId { get; set; }

    public Type Type { get; set; }

    /// <summary>
    /// Gets/Sets CFG (Context Free Grammar) parsed symbol.
    /// </summary>
    //public GrammarSymbol? GrammarSymbol { get; set; }

    /// <summary>
    /// Gets link with content, multiple tokens/locations as there are might be partial structs, etc.
    /// </summary>
    public List<Token> Tokens { get; init; }

    public Symbol? Papa { get; set; }

    public Flags Flags { get; set; }

    /// <summary>
    /// Gets optional inferred value if any, might be integer value, real value or identifier (correlated with name).
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Gets metadata: attributes, annotations, etc.
    /// </summary>
    public Dictionary<string, object> Metadata { get; init; }

    public Symbol(id id,
                  Type type,
                  Symbol? papa = default,
                  string? label = default,
                  Flags flags = Flags.Clear,
                  Dictionary<string, object>? metadata = default,
                  string? version = default) : base(id, version)
    {
        Assert.NonNullReference(type, nameof(type));

        Name = label?.Trim() ?? $"S:{id.ToString()}";
        SyntheticName = Name;
        SyntheticId = 0;
        Type = type;
        Tokens = new();
        Papa = papa;
        Flags = flags;
        Metadata = metadata ?? new();
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;

        yield return Name;
        yield return Type;
        yield return SyntheticId;
    }

    public abstract TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param = default);
}
