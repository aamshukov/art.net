//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Cache.Abstractions;
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.Cache;

public abstract class Cache<TKey, TValue> : Disposable, ICache<TKey, TValue>
    where TKey : class, IComparable<TKey>
    where TValue : class
{
    protected Dictionary<TKey, CacheData<TValue>> Storage { get; init; }

    public count Capacity { get { return Storage.Capacity; } }

    public count Count { get { return Storage.Count; } }

    public Statistics.Statistics Statistics { get; init; }

    public Diagnostics.Diagnostics Diagnostics { get; init; }

    public abstract event EventHandler<CacheEventArgs<TKey, CacheData<TValue>>>? Added;

    public abstract event EventHandler<CacheEventArgs<TKey, CacheData<TValue>>>? Replaced;

    public abstract event EventHandler<CacheEventArgs<TKey, CacheData<TValue>>>? Removed;

    public abstract event EventHandler<CacheEventArgs<TKey, CacheData<TValue>>>? Evicted;

    public abstract event EventHandler<CacheEventArgs<TKey, CacheData<TValue>>>? Expired;

    public abstract event EventHandler? Cleared;

    public new abstract event EventHandler? Disposed;

    public Cache(count capacity,
                 Statistics.Statistics statistics,
                 Diagnostics.Diagnostics diagnostics)
    {
        Assert.Ensure(capacity >= 0, nameof(capacity));
        Assert.NonNullReference(statistics, nameof(statistics));
        Assert.NonNullReference(diagnostics, nameof(diagnostics));

        Statistics = statistics;
        Diagnostics = diagnostics;

        Storage = new(capacity);
    }

    public abstract TValue Get(TKey key);

    public abstract Task<TValue> GetAsync(TKey key, CancellationToken cancellationToken);

    public abstract bool TryGet(TKey key, out TValue value);

    public abstract void Set(TKey key, TValue value);

    public abstract Task SetAsync(TKey key, TValue value, CancellationToken cancellationToken);

    public abstract bool TrySet(TKey key, TValue value);

    public abstract void Remove(TKey key);

    public abstract Task RemoveAsync(TKey key, CancellationToken cancellationToken);

    public abstract bool TryRemove(TKey key, out TValue value);

    public abstract void Clear();

    public abstract Task Clear(CancellationToken cancellationToken);

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
