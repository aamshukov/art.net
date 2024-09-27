//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Core.Statistics;

public sealed class Statistics
{
    public Dictionary<string, count> Usage { get; init; }

    public Dictionary<string, TimeSpan> Timings { get; init; }

    public Statistics()
    {
        Usage = new();
        Timings = new();
    }
}
