//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.IO.MemoryMappedFiles;
using UILab.Art.Framework.Core.DataAccess.Repository.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;
using UILab.Framework.Core.Platform;

namespace UILab.Art.Framework.Core.DataAccess.Repository.Mmf;

public class MmfRepository<T> : MmfReadOnlyRepository<T>, IRepository<T>
{
    public MmfRepository(MemoryMappedFile mmf,
                      MemoryMappedViewAccessor? view,
                      MmfCursor? cursor,
                      EndiannessType endiannessType = EndiannessType.LittleEndian) : base(mmf, view, cursor, endiannessType)
    {
    }

    public Result<WriteOperataionResult> Write(ReadOnlyMemory<T> buffer, offset offset, count count)
    {
        Assert.NonDisposed(Disposed);
        return Result<WriteOperataionResult>.Success(payload: new(0));
    }

    public bool TryWrite(ReadOnlyMemory<T> buffer, offset offset, count count, out Result<WriteOperataionResult> result)
    {
        Assert.NonDisposed(Disposed);
        result = Result<WriteOperataionResult>.Success(payload: new(0));
        return false;
    }

    public async Task<Result<WriteOperataionResult>> WriteAsync(ReadOnlyMemory<T> buffer, offset offset, count count, CancellationToken cancellationToken)
    {
        Assert.NonDisposed(Disposed);
        await Task.CompletedTask;
        return Result<WriteOperataionResult>.Success(payload: new(0));
    }
}
