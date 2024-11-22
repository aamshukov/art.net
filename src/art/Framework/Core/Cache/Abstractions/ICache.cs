//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Core.Cache.Abstractions;

public interface ICache<TKey, TValue>
    where TKey : class, IComparable<TKey>
    where TValue : class
{
    count Capacity { get; }

    count Count { get; }

    Statistics.Statistics Statistics { get; }

    Diagnostics.Diagnostics Diagnostics { get; }

    TValue Get(TKey key);

    Task<TValue> GetAsync(TKey key, CancellationToken cancellationToken);

    bool TryGet(TKey key, out TValue value);

    void Set(TKey key, TValue value);

    Task SetAsync(TKey key, TValue value, CancellationToken cancellationToken);

    bool TrySet(TKey key, TValue value);

    void Remove(TKey key);

    Task RemoveAsync(TKey key, CancellationToken cancellationToken);

    bool TryRemove(TKey key, out TValue value);

    void Clear();

    Task Clear(CancellationToken cancellationToken);
}
