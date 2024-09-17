//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.Counter;

public sealed class ReferenceCounter
{
    public count Count;

    public readonly count Threshold;

    public ReferenceCounter(id seedCount = 0)
    {
        Count = seedCount;
        Threshold = seedCount;
    }

    public count AddReference() => Interlocked.Increment(ref Count);

    public bool CanRelease()
    {
        Assert.Ensure(Count >= Threshold, nameof(count));
        return Count == Threshold;
    }

    public count Release() => Interlocked.Decrement(ref Count);
}
