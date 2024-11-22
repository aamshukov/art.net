//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.Cache;

public sealed class CacheEventArgs<TKey, TValue> : EventArgs
    where TKey : class
    where TValue : class
{
     public TKey Key { get; init; }

     public CacheData<TValue> Value { get; init; }

     public CacheEventArgs(TKey key, CacheData<TValue> value)
     {
        Assert.NonNullReference(key, nameof(key));
        Assert.NonNullReference(value, nameof(value));

        Key = key;
        Value = value;
     }
}
