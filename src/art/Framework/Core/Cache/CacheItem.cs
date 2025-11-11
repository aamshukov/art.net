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
    public DateTimeOffset Created { get; init; }

    /// <summary>
    /// Date accessed, UTC.
    /// </summary>
    public DateTimeOffset Accessed { get; set; }

    /// <summary>
    /// Date modified, UTC.
    /// </summary>
    public DateTimeOffset Modified { get; set; }

    /// <summary>
    /// Date removed, UTC.
    /// </summary>
    public DateTimeOffset Removed { get; set; }

    /// <summary>
    /// Expiration date, UTC.
    /// </summary>
    public DateTimeOffset Expiration { get; init; }

    public CacheItem(TValue value, string region, TimeSpan? expiration = default)
    {
        Assert.NonNullReference(value);
        Assert.NonEmptyString(region);

        Value = value;
        Region = region;

        var timestamp = DateTimeOffset.Now;

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
