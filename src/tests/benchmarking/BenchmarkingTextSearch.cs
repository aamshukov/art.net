//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
#pragma warning disable CS8981
global using index = int;
global using size = int;
global using count = int;
#pragma warning restore CS8981

using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using NUnit.Framework;
using UILab.Art.Framework.Core.Text.Search;
using static UILab.Art.Framework.Core.Text.Search.BoyerMoore;
using ArtText = UILab.Art.Framework.Core.Text.Text;

namespace UILab.Art.Benchmarking;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.Method)]
[SimpleJob(launchCount: 3, warmupCount: 1, iterationCount: 3, invocationCount: 1)]
public class BenchmarkingTextSearch
{
    [Benchmark]
    public void Text_Search_BoyerMoore_Naive2_Random_Success()
    {
        count count = 10;
        size length = 1000;

        for(int k = 0; k < count; k++)
        {
            for(int n = 1; n < length; n++)
            {
                var randomText = string.Empty;
                var randomPattern = string.Empty;

                try
                {
                    randomText = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, n));
                    randomPattern = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, randomText.Length));
                    var randomMatches = GetMatches(randomText, randomPattern);
                    var text = ArtText.GetCodepoints(randomText);
                    var pattern = ArtText.GetCodepoints(randomPattern);
                    var matches = new BoyerMoore().Search(text, pattern);
                    Assert.That(matches.SequenceEqual(randomMatches), Is.True);
                }
                catch(Exception ex)
                {
                    var m = ex.Message;
                    Console.WriteLine($"randomText: {randomText}");
                    Console.WriteLine($"randomPattern: {randomPattern}");
                    k = count;
                    break;
                }
            }
        }
    }

    [Benchmark]
    public void Text_Search_BoyerMoore_Gusfield2_Random_Success()
    {
        count count = 10;
        size length = 1000;

        for(int k = 0; k < count; k++)
        {
            for(int n = 1; n < length; n++)
            {
                var randomText = string.Empty;
                var randomPattern = string.Empty;

                try
                {
                    randomText = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, n));
                    randomPattern = ArtText.GetRandomText(RandomNumberGenerator.GetInt32(0, randomText.Length));
                    var randomMatches = GetMatches(randomText, randomPattern);
                    var text = ArtText.GetCodepoints(randomText);
                    var pattern = ArtText.GetCodepoints(randomPattern);
                    var matches = new BoyerMoore().Search(text, pattern, meta: ZAlgorithm.Gusfield);
                    Assert.That(matches.SequenceEqual(randomMatches), Is.True);
                }
                catch(Exception ex)
                {
                    var m = ex.Message;
                    Console.WriteLine($"randomText: {randomText}");
                    Console.WriteLine($"randomPattern: {randomPattern}");
                    k = count;
                    break;
                }
            }
        }
    }

    private static List<index> GetMatches(string text, string pattern)
    {
        List<index> matches = new();

        if(string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
            return matches;

        for(int index = 0;; index += pattern.Length)
        {
            index = text.IndexOf(pattern, index);

            if(index == -1)
                break;

            matches.Add(index);
        }

        return matches;
    }
}
