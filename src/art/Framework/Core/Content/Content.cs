//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Content.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Framework.Core.Content;

public class Content<T> : EntityType<string>, IContent<T>
{
    /// <summary>
    /// Origin of the content.
    /// </summary>
    public string Source { get; init; }

    /// <summary>
    /// Sequence of data runs.
    /// </summary>
    public List<Buffer<T>> Contents { get; init; }

    private size BufferSize { get; init; }

    public Content(string id,
                   string source,
                   size bufferSize,
                   string? version = default) : base(id, version)
    {
        Assert.NonEmptyString(id, nameof(id));
        Assert.NonEmptyString(source, nameof(source));
        Assert.Ensure(bufferSize > 0, nameof(bufferSize));

        Contents = new();
        Source = source;
        BufferSize = bufferSize;
    }

    public size Length() => Contents.Aggregate(0, (result, buffer) => result + buffer.Size);

    public T GetCodepoint(location location, T defaultValue)
    {
        Assert.Ensure(location >= 0, nameof(location));

        T result = defaultValue;

        index start = location % BufferSize; // in the current buffer, location might start with some offset in the first buffer ...

        for(int k = location / BufferSize; k < Contents.Count; k++)
        {
            Buffer<T> buffer = Contents[k];

            size dataLength = Math.Min(1, buffer.Size - start); // in the current buffer
            Assert.Ensure(dataLength >= 0, $"Getting content's codepoint, out of range: {dataLength}.", typeof(IndexOutOfRangeException));

            if(dataLength == 1)
            {
                result = buffer.Data.Span[start];
                break;
            }

            start = 0; // ... reset for all subsequent operations
        }

        return result;
    }

    public List<ReadOnlyMemory<T>> GetContent(location location, size length = size.MaxValue)
    {
        //  location = 2, length = 7
        //  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9
        //  . . . . . . . . . .  . . . . . . . . . .
        //      ^           ^
        //
        //  location = 2, length = 6
        //  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9
        //  . . . . . . . . - -  - - - - - - -- - -   size: 8
        //      ^         ^
        //
        //  location = 2, length = 12
        //  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9
        //  . . . . . . . . . .  . . . . . . . . . .
        //      ^                      ^
        //
        //  location = 14, length = 10
        //  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9
        //  . . . . . . . . . .  . . . . . . . . . .  . . . . . . . . . .
        //                             ^                  ^
        //
        //  location = 8, length = 19
        //  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9
        //  . . . . . . . . . .  . . . . . . . . . .  . . . . . . . . . .
        //                ^                                     ^

        //  location = 0, length = 10
        //  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9
        //  . . . . . . . . . .  . . . . . . . . . .
        //  ^                 ^
        Assert.Ensure(location >= 0, nameof(location));
        Assert.Ensure(length >= 0, nameof(length));

        List<ReadOnlyMemory<T>> content = new();

        index start = location % BufferSize; // in the current buffer, location might start with some offset in the first buffer ...

        for(int k = location / BufferSize; length > 0 && k < Contents.Count; k++)
        {
            Buffer<T> buffer = Contents[k];

            size dataLength = Math.Min(length, buffer.Size - start); // in the current buffer
            Assert.Ensure(dataLength >= 0, $"Getting content, out of range: {dataLength}.", typeof(IndexOutOfRangeException));

            content.Add(buffer.Data.Slice(start, dataLength));

            length -= dataLength;
            Assert.Ensure(length >= 0, $"Getting content, out of range: {length}.", typeof(IndexOutOfRangeException));

            start = 0; // ... reset for all subsequent operations
        }

        return content;
    }

    /// <summary>
    /// Appends content.
    /// </summary>
    /// <param name="codepoints"></param>
    /// <returns>Number of appended elements.</returns>
    public count AppendContent(ReadOnlyMemory<T> codepoints)
    {
        //  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9
        //  - - - - - - . . . .  . . . . . . . . . .
        //  0 1 2 3 4 5
        //
        //  case I
        //  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9
        //  - - - - - - . . . .  . . . . . . . . . .
        //              a b
        //
        //  case II
        //  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9
        //  - - - - - - - - . .  . . . . . . . . . .
        //                  0 1  2 3 4 5
        //
        //  case III
        //  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9
        //  - - - - - - - - - -  - - - - . . . . . .  . . . . . . . . . .  . . . . . . . . . .
        //                               4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9  0 1 2 3 4 5 6 7 8 9
        Assert.NonNullReference(codepoints, nameof(codepoints));

        if(Contents.Count == 0)
        {
            Contents.Add(new(id: Contents.Count, size: 0, capacity: BufferSize, version: Version));
        }

        Buffer<T> dstContent = Contents[Contents.Count - 1];

        count codepointsLength = codepoints.Length;

        offset srcOffset = 0;
        size srcSize = Math.Min(codepointsLength, dstContent.Capacity - dstContent.Size);

        if(srcSize == 0)
        {
            Contents.Add(new(id: Contents.Count, size: 0, capacity: BufferSize, version: Version));

            dstContent = Contents[Contents.Count - 1];
            srcSize = Math.Min(codepointsLength, dstContent.Capacity - dstContent.Size);
        }

        while(true)
        {
            dstContent.AppendData(codepoints, srcOffset, srcSize);

            codepointsLength -= srcSize;
            Assert.Ensure(codepointsLength >= 0, $"Appending content, out of range: {codepointsLength}.", typeof(IndexOutOfRangeException));

            if(codepointsLength == 0)
            {
                break;
            }

            dstContent = new(id: Contents.Count, size: 0, capacity: BufferSize, version: Version);

            Contents.Add(dstContent); // always has an extra buffer to be able calling WorkingContent.Contents.Last()

            srcOffset += srcSize;
            srcSize = Math.Min(codepointsLength, dstContent.Capacity - dstContent.Size);
        }

        Assert.Ensure(codepoints.Length - codepointsLength >= 0, $"Appending content, out of range: {codepoints.Length}:{codepointsLength}.", typeof(IndexOutOfRangeException));
        return codepoints.Length - codepointsLength;
    }

    public void Clear()
    {
        Contents.Clear();
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
        yield return Source;
        yield return BufferSize;
        // ignore Contents, might be large
    }
}
