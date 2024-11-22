//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.Cache;

public sealed class CacheItem<TValue>
    where TValue : class
{
    public TValue Value { get; init; }

    public string Region { get; init; }

    /// <summary>
    /// Date created, UTC.
    /// </summary>
    public DateTime Created { get; init; }

    /// <summary>
    /// Date accessed, UTC.
    /// </summary>
    public DateTime Accessed { get; set; }

    /// <summary>
    /// Date modified, UTC.
    /// </summary>
    public DateTime Modified { get; set; }

    /// <summary>
    /// Date removed, UTC.
    /// </summary>
    public DateTime Removed { get; set; }

    /// <summary>
    /// Expiration date, UTC.
    /// </summary>
    public DateTime Expiration { get; init; }

    public CacheItem(TValue value, string region, TimeSpan? expiration = default)
    {
        Assert.NonNullReference(value, nameof(value));
        Assert.NonEmptyString(region, nameof(region));

        Value = value;
        Region = region;

        var timestamp = DateTime.UtcNow;

        Created = timestamp;
        Accessed = timestamp;
        Modified = timestamp;
        Removed = DateTime.MinValue;

        if(expiration is not null)
            Expiration = timestamp.Add((TimeSpan)expiration);
        else
            Expiration = DateTime.MaxValue;
    }
}
