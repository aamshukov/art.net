//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.DataAccess.Repository.Abstractions;

public interface IReadOnlyRepository<T>
{
    Result<ReadOperataionResult<T>> Read(offset offset, count count);

    bool TryRead(offset offset, count count, out Result<ReadOperataionResult<T>> result);

    Task<Result<ReadOperataionResult<T>>> ReadAsync(offset offset, count count, CancellationToken cancellationToken);
}
