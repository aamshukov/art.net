//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Collections.Concurrent;
using UILab.Art.Framework.Core.Cache.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.Cache;

public abstract class Cache<TKey, TValue> : Disposable, ICache<TKey, TValue>
    where TKey : class, IComparable<TKey>
    where TValue : class
{
    protected ConcurrentDictionary<TKey, CacheItem<TValue>> Storage { get; init; }

    public string Name { get; init; }

    public string Region { get; init; }

    public ICachePolicy<TKey, TValue> Policy { get; init; }

    public count Capacity { get; init; }

    public count Count { get { return Storage.Count; } }

    public CacheStatistics Statistics { get; init; }

    public Diagnostics.Diagnostics Diagnostics { get; init; }

    public abstract event EventHandler<CacheEventArgs<TKey, CacheItem<TValue>>>? Got;

    public abstract event EventHandler<CacheEventArgs<TKey, CacheItem<TValue>>>? Added;

    public abstract event EventHandler<CacheEventArgs<TKey, CacheItem<TValue>>>? Replaced;

    public abstract event EventHandler<CacheEventArgs<TKey, CacheItem<TValue>>>? Removed;

    public abstract event EventHandler<CacheEventArgs<TKey, CacheItem<TValue>>>? Evicted;

    public abstract event EventHandler<CacheEventArgs<TKey, CacheItem<TValue>>>? Expired;

    public abstract event EventHandler? Cleared;

    public abstract event EventHandler? ClearedRegion;

    public new abstract event EventHandler? Disposed;

    public Cache(string name,
                 ICachePolicy<TKey, TValue> policy,
                 string? region = default,
                 count capacity = 1024,
                 CacheStatistics? statistics = default,
                 Diagnostics.Diagnostics? diagnostics = default)
    {
        Assert.NonEmptyString(name);
        Assert.NonNullReference(policy);
        Assert.Ensure(capacity >= 0, nameof(capacity));

        Policy = policy;

        Name = name;
        Region = region ?? name;

        Capacity = capacity;

        Statistics = statistics ?? new();
        Diagnostics = diagnostics ?? new();

        Storage = new(-1, capacity);
    }

    public abstract bool IsExpired(TKey key, string region);

    public abstract TValue Get(TKey key, string region);

    public abstract Task<TValue> GetAsync(TKey key, string region, CancellationToken cancellationToken);

    public abstract bool TryGet(TKey key, string region, out TValue value);

    public abstract Task<bool> TryGetAsync(TKey key, string region, CancellationToken cancellationToken, out TValue value);

    public abstract void Set(TKey key, TValue value, string region);

    public abstract Task SetAsync(TKey key, TValue value, string region, CancellationToken cancellationToken);

    public abstract bool TrySet(TKey key, TValue value, string region);

    public abstract Task<bool> TrySetAsync(TKey key, TValue value, string region, CancellationToken cancellationToken);

    public abstract void Remove(TKey key, string region);

    public abstract Task RemoveAsync(TKey key, string region, CancellationToken cancellationToken);

    public abstract bool TryRemove(TKey key, string region, out TValue value);

    public abstract Task<bool> TryRemoveAsync(TKey key, string region, out TValue value, CancellationToken cancellationToken);

    public abstract void Clear();

    public abstract Task ClearAsync(CancellationToken cancellationToken);

    public abstract void ClearRegion(string region);

    public abstract Task ClearRegionAsync(string region, CancellationToken cancellationToken);

    protected override void DisposeManagedResources()
    {
    }

    protected override async ValueTask DisposeManagedResourcesAsync()
    {
        await ValueTask.CompletedTask;
    }

    protected override void DisposeUnmanagedResources()
    {
    }

    protected override async ValueTask DisposeUnmanagedResourcesAsync()
    {
        await ValueTask.CompletedTask;
    }
}
