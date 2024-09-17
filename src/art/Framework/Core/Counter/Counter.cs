//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Core.Counter;

public sealed class Counter
{
    private id Id;

    public Counter(id seedId = 0)
    {
        Id = seedId;
    }

    public id NextId() => Interlocked.Increment(ref Id);
}
