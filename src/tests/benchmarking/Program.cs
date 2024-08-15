//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace UILab.Art.Benchmarking.Tests;

public sealed class Program
{
    // start VS command line
    // cd D:\Projects\art.net\src\tests\benchmarking
    // dotnet run --project Art.BenchmarkingTests.csproj -c Release --filter *JsonConfigurationWithExpansionProvider_ArrayOfCases_Success*
    // dotnet run --project Art.BenchmarkingTests.csproj -c Release --filter *Text_Search_BoyerMoore*
    //  or
    // cd D:\Projects\art.net\src\tests\benchmarking\bin\Release\net8.0
    // Art.BenchmarkingTests.exe --filter *JsonConfigurationWithExpansionProvider_ArrayOfCases_Success*
    // Art.BenchmarkingTests.exe --filter *Text_Search_BoyerMoore*
    static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
    }
}
