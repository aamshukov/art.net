//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Core.Cache;

public enum CacheStatisticsType
{
    Hits = 1,
    Misses,
    GetCalls = 10,
    AddCalls,
    ReplaceCalls,
    RemoveCalls,
    EvictedCalls,
    ExpiredCalls,
    ClearCalls,
    ClearRegionCalls,
    DisposeCalls
}
