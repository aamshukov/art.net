//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace UILab.Art.Benchmarking;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.Method)]
[SimpleJob(launchCount: 3, warmupCount: 1, iterationCount: 3, invocationCount: 1)]
public class BenchmarkingStringBuilder
{
    [Benchmark]
    public void Benchmarking_StringBuilder_Test()
    {
        StringBuilder sb = new();

        for(int i = 0; i < 100; i++)
        {
            sb.Append("Hello World!" + i);
        }
    }
}
