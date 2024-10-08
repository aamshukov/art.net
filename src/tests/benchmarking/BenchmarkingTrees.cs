//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace UILab.Art.Benchmarking;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.Method)]
[SimpleJob(launchCount: 3, warmupCount: 1, iterationCount: 3, invocationCount: 1)]
public class BenchmarkingTrees
{
    [Benchmark]
    public void RedBlackTree_Random_Success()
    {
    }

    [Benchmark]
    public void AvlTree_Random_Success()
    {
    }
}
