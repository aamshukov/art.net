//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Core.Cache;

public sealed class CacheStatistics : Statistics.Statistics
{
    private count[] Counters { get; init; }

    public CacheStatistics()
    {
        Counters = new count[Enum.GetNames<CacheStatisticsType>().Length];
    }

    public count Get(CacheStatisticsType type) => Counters[(int)type];

    public void Add(CacheStatisticsType type)
    {
        Interlocked.Increment(ref Counters[(int)type]);
    }

    public void Release(CacheStatisticsType type)
    {
        Interlocked.Decrement(ref Counters[(int)type]);
    }
}
