﻿//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Core.Cache.Abstractions;

public interface ICachePolicy<TKey, TValue>
    where TKey : class, IComparable<TKey>
    where TValue : class
{
    bool IsExpired(TKey key, string region);

    TValue Get(TKey key, string region);

    Task<TValue> GetAsync(TKey key, string region, CancellationToken cancellationToken);

    bool TryGet(TKey key, string region, out TValue value);

    Task<bool> TryGetAsync(TKey key, string region, CancellationToken cancellationToken, out TValue value);

    void Set(TKey key, TValue value, string region);

    Task SetAsync(TKey key, TValue value, string region, CancellationToken cancellationToken);

    bool TrySet(TKey key, TValue value, string region);

    Task<bool> TrySetAsync(TKey key, TValue value, string region, CancellationToken cancellationToken);

    void Remove(TKey key, string region);

    Task RemoveAsync(TKey key, string region, CancellationToken cancellationToken);

    bool TryRemove(TKey key, string region, out TValue value);

    Task<bool> TryRemoveAsync(TKey key, string region, out TValue value, CancellationToken cancellationToken);

    void Clear();

    Task ClearAsync(CancellationToken cancellationToken);

    void ClearRegion(string region);

    Task ClearRegionAsync(string region, CancellationToken cancellationToken);
}
