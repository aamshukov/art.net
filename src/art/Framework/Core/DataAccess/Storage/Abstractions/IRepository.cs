//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.DataAccess.Repository.Abstractions;

public interface IRepository<T> : IReadOnlyRepository<T>
{
    Result<WriteOperataionResult> Write(ReadOnlyMemory<T> buffer, offset offset, count count);

    bool TryWrite(ReadOnlyMemory<T> buffer, offset offset, count count, out Result<WriteOperataionResult> result);

    Task<Result<WriteOperataionResult>> WriteAsync(ReadOnlyMemory<T> buffer, offset offset, count count, CancellationToken cancellationToken);
}
