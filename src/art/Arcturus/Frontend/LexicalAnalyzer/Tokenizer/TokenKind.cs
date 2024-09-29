//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Arcturus.LexicalAnalyzer.Tokenizer;

public enum TypeKind
{
    Unknown = 0,
    Epsilon = 5,

    Ws,         //  6
    Eol,        //  7
    Eos,        //  8
    Indent,     //  9,  literal = '    '
    Dedent,     //  10, literal = '    '

    Identifier,
}
