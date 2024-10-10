//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.DistributedComputing;

public enum EventOrdering
{
    Unknown = 0, // not comparable
    Equal = 1,
    Concurrent,
    HappensBefore,
    HappensAfter
}
