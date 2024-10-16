//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.IO.MemoryMappedFiles;
using UILab.Art.Framework.Core.DataAccess.Storage.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Framework.Core.Platform;

namespace UILab.Art.Framework.Core.DataAccess.Storage.Mmf;

public class MmfReadOnlyStorage<T> : Disposable, IReadOnlyStorage<T>
{
    public MemoryMappedFile Mmf { get; init; }

    public MemoryMappedViewAccessor View { get; init; }

    public MmfCursor Cursor { get; init; }

    public EndiannessType Endianness { get; init; }

    public MmfReadOnlyStorage(MemoryMappedFile mmf,
                              MemoryMappedViewAccessor? view,
                              MmfCursor? cursor,
                              EndiannessType endiannessType = EndiannessType.LittleEndian)
    {
        Assert.NonNullReference(mmf, nameof(mmf));

        Mmf = mmf;
        View = view ?? mmf.CreateViewAccessor();
        Cursor = cursor ?? new(View);

        Endianness = endiannessType;
    }

    public Result<ReadOperataionResult<T>> Read(offset offset, count count)
    {
        Assert.NonDisposed(Disposed);
        return Result<ReadOperataionResult<T>>.Success(payload: new(default, 0));
    }

    public bool TryRead(offset offset, count count, out Result<ReadOperataionResult<T>> result)
    {
        Assert.NonDisposed(Disposed);
        result = Result<ReadOperataionResult<T>>.Success(payload: new(default, 0));
        return false;
    }

    public async Task<Result<ReadOperataionResult<T>>> ReadAsync(offset offset, count count, CancellationToken cancellationToken)
    {
        Assert.NonDisposed(Disposed);
        await Task.CompletedTask;
        return Result<ReadOperataionResult<T>>.Success(payload: new(default, 0));
    }

    protected override void DisposeManagedResources()
    {
        Cursor.Dispose();
        View.Dispose();
        Mmf.Dispose();
    }

    protected override ValueTask DisposeManagedResourcesAsync()
    {
        DisposeManagedResources();
        return default;
    }

    protected override void DisposeUnmanagedResources()
    {
    }

    protected override ValueTask DisposeUnmanagedResourcesAsync()
    {
        return default;
    }
}
