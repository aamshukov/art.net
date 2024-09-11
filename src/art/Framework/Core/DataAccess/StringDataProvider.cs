//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics.CodeAnalysis;
using UILab.Art.Framework.Core.Content;
using UILab.Art.Framework.Core.Content.Abstractions;
using UILab.Art.Framework.Core.DataAccess.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.DataAccess;

public sealed class StringDataProvider : IStringDataProvider<codepoint>
{
    public string Data { get; init; }

    private size BufferSize { get; init; }

    public string? Version { get; init; }

    public StringDataProvider(string data, size bufferSize, string? version = default)
    {
        Assert.NonNullReference(data, nameof(data)); // might be empty or WS
        Assert.Ensure(bufferSize > 0, nameof(bufferSize));

        Data = data;
        BufferSize = bufferSize;
        Version = version?.Trim();
    }

    [SuppressMessage("Critical Code Smell", "S1994:\"for\" loop increment clauses should modify the loops' counters", Justification = "<Pending>")]
    public void Load(IContent<codepoint> content)
    {
        Assert.NonNullReference(content, nameof(content));

        var codepoints = Data.EnumerateRunes().Select(r => (codepoint)r.Value).ToArray().AsMemory<codepoint>();

        count codepointsLength = codepoints.Length;

        offset srcOffset = 0;

        for(id k = 0; ; k++)
        {
            size srcSize = Math.Min(codepointsLength, BufferSize);

            Buffer<codepoint> buffer = new(id: k, size: 0, capacity: BufferSize);

            buffer.AppendData(codepoints, srcOffset, srcSize);
            content.Contents.Add(buffer);

            codepointsLength -= srcSize;

            if(codepointsLength == 0)
            {
                break;
            }

            srcOffset += srcSize;
        }
    }

    public async Task LoadAsync(IContent<codepoint> content, CancellationToken cancellationToken)
    {
        Assert.NonNullReference(content, nameof(content));

        Load(content);
        await Task.CompletedTask;
    }
}
