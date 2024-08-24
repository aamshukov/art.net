//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Art.Framework.Core.Domain;

namespace Art.Framework.Core.Content;

public class Buffer<T> : EntityType<id>
{
    public size Size { get; private set; }

    public size Capacity { get; init; }

    public Memory<T> Data { get; init; }

    public bool IsEmpty() => Size == 0;

    public bool IsFull() => Size == Capacity;

    public Buffer(id id, size size, size capacity, string? version = default) : base(id, version)
    {
        Assert.Ensure(id >= 0, nameof(size));
        Assert.Ensure(size >= 0, nameof(size));
        Assert.Ensure(capacity >= 0, nameof(capacity));
        Assert.Ensure(size <= capacity, $"{nameof(size)}:{nameof(capacity)}");

        Id = id;
        Size = size;
        Capacity = capacity;

        Data = new(new T[capacity]);
    }

    public void AppendData(ReadOnlyMemory<T> srcData, offset srcOffset, size srcSize)
    {
        SetData(srcData, srcOffset, srcSize);
    }

    public void SetData(ReadOnlyMemory<T> srcData, offset srcOffset = 0, size srcSize = size.MaxValue, offset dstOffset = offset.MaxValue)
    {
        Assert.NonNullReference(srcData, nameof(srcData));

        srcSize = Math.Min(srcSize, srcData.Length);
        dstOffset = Math.Min(dstOffset, Size);

        Assert.Ensure(srcOffset + srcSize <= srcData.Length, $"{nameof(srcOffset)}:{nameof(srcSize)}", typeof(IndexOutOfRangeException));
        Assert.Ensure(dstOffset + srcSize <= Capacity, $"{nameof(dstOffset)}:{nameof(srcSize)}", typeof(IndexOutOfRangeException));

        srcData.Slice(srcOffset, srcSize).CopyTo(Data.Slice(dstOffset, srcSize));

        Size = dstOffset + srcSize;
        Assert.Ensure(Size <= Capacity, $"{nameof(Size)}", typeof(IndexOutOfRangeException));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        foreach(var component in base.GetEqualityComponents())
            yield return component;
        yield return Size;
        yield return Capacity;
        // ignore Data, might be large
    }
}
