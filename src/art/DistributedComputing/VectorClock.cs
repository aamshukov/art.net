//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.DistributedComputing;

public sealed class VectorClock
{
    public List<count> Vector { get; init; }

    public index Index { get; init; }

    public VectorClock(index index, size capacity)
    {
        Assert.Ensure(index >= 0, nameof(index));
        Assert.Ensure(capacity > 0, nameof(capacity));

        Index = index;
        Vector = new(capacity);
    }

    public count Increment(count count)
    {
        Vector[Index] += count;
        return Vector[Index];
    }

    public VectorClock Merge(VectorClock other)
    {
        Assert.NonNullReference(other);
        Assert.Ensure(Vector.Count == other.Vector.Count, nameof(other.Vector));

        Vector[Index] += 1;

        VectorClock mergedVectorClock = new(Index, Vector.Count);

        for(index k = 0, i = Index, n = Vector.Count; k < n; k++)
        {
            if(k == i)
                continue;

            Vector[k] = Math.Max(other.Vector[k], Vector[k]);
        }

        return mergedVectorClock;
    }
}
