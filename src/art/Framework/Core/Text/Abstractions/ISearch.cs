//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Core.Text.Search.Abstractions;

public interface ISearch
{
    TextEncoding Encoding { get; }

    IEnumerable<index> Search(ReadOnlyMemory<codepoint> text,
                              ReadOnlyMemory<codepoint> pattern,
                              index start = 0,
                              size length = size.MaxValue,
                              count count = count.MaxValue,
                              object? meta = default);
}
