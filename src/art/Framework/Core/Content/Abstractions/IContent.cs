//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using Art.Framework.Core.Content;

namespace UILab.Art.Framework.Core.Content.Abstractions;

public interface IContent<T>
{
    string Source { get; }

    List<Buffer<T>> Contents { get; }

    size Length();

    T GetCodepoint(location location, T defaultValue);

    List<ReadOnlyMemory<T>> GetContent(location location, size length = size.MaxValue);

    count AppendContent(ReadOnlyMemory<T> codepoints);

    void Clear();
}
