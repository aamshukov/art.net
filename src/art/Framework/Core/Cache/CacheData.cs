//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.Cache;

public sealed class CacheData<TValue>
    where TValue : class
{
    public TValue Value { get; init; }

    public DateTime Added { get; init; }

    public DateTime LastUsed { get; set; }

    public DateTime Expiration { get; init; }

    public CacheData(TValue value, DateTime? expiration = default)
    {
        Assert.NonNullReference(value, nameof(value));

        Value = value;

        var timestamp = DateTime.Now;

        Added = timestamp;
        LastUsed = timestamp;

        Expiration = expiration ?? DateTime.MaxValue;
    }
}
