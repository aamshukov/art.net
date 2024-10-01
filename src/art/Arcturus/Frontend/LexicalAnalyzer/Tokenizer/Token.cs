//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using ValueType = UILab.Art.Framework.Core.Domain.ValueType;

namespace UILab.Art.Arcturus.LexicalAnalyzer.Tokenizer;

public sealed class Token : ValueType
{
    /// <summary>
    /// Gets type of lexeme.
    /// </summary>
    public TypeKind Kind { get; init; }

    /// <summary>
    /// Gets offset in context (absolute address).
    /// </summary>
    public offset Offset { get; init; }

    /// <summary>
    /// Gets length of lexeme.
    /// </summary>
    public size Length { get; init; }

    /// <summary>
    /// Gets string or char literal (if unicode - always decoded), numeric value, etc.
    /// </summary>
    public string Literal { get; init; }

    /// <summary>
    /// Roslyn:
    /// Gets trivia that appears before the token.
    /// </summary>
    public string LeadingTrivia { get; init; }

    /// <summary>
    /// Roslyn:
    /// Gets trivia that appears after the token.
    /// </summary>
    public string TrailingTrivia { get; init; }

    public enum TokenFlags : flag
    {
        Clear     = 0x00,
        Genuine   = 0x01,
        Contextual= 0x02, // contextual, recognized in specific contexts, similar to C# get/set, async/await ...
        Synthetic = 0x04  // additional (artificial) tokens which are inserted into the token stream, syntactic sugar - desugaring ...
    };

    public TokenFlags Flags { get; set; }

    /// <summary>
    /// Gets lexical analyser which recognizes this lexeme, could be from different files/inputs.
    /// </summary>
    public id Source { get; init; }

    public Token(TypeKind kind,
                 offset offset,
                 size length,
                 id source,
                 string? literal = default,
                 string? leadingTrivia = default,
                 string? trailingTrivia = default,
                 TokenFlags flags = TokenFlags.Clear,
                 string? version = default) : base(version)
    {
        Kind = kind;
        Offset = offset;
        Length = length;
        Source = source;
        Literal = literal?.Trim() ?? string.Empty;
        LeadingTrivia = leadingTrivia?.Trim() ?? string.Empty;
        TrailingTrivia = trailingTrivia?.Trim() ?? string.Empty;
        Flags = flags;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
        yield return Kind;
    }
}
