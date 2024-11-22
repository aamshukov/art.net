//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Cache.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.Cache;

public abstract class CachePolicy<TKey, TValue> : Disposable, ICachePolicy<TKey, TValue>
    where TKey : class, IComparable<TKey>
    where TValue : class
{
    protected Dictionary<TKey, CacheItem<TValue>> Storage { get; init; }

    public CacheStatistics Statistics { get; init; }

    public Diagnostics.Diagnostics Diagnostics { get; init; }

    public CachePolicy(Dictionary<TKey, CacheItem<TValue>> storage,
                       CacheStatistics? statistics = default,
                       Diagnostics.Diagnostics? diagnostics = default)
    {
        Assert.NonNullReference(storage, nameof(storage));

        Storage = storage;

        Statistics = statistics ?? new();
        Diagnostics = diagnostics ?? new();
    }

    public abstract bool IsExpired(TKey key, string region);

    public abstract TValue Get(TKey key, string region);

    public abstract Task<TValue> GetAsync(TKey key, string region, CancellationToken cancellationToken);

    public abstract bool TryGet(TKey key, string region, out TValue value);

    public abstract void Set(TKey key, TValue value, string region);

    public abstract Task SetAsync(TKey key, TValue value, string region, CancellationToken cancellationToken);

    public abstract bool TrySet(TKey key, TValue value, string region);

    public abstract void Remove(TKey key, string region);

    public abstract Task RemoveAsync(TKey key, string region, CancellationToken cancellationToken);

    public abstract bool TryRemove(TKey key, string region, out TValue value);

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
