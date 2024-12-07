using Algorithms.DataStructures;
using BenchmarkDotNet.Running;
using System.Diagnostics;

namespace Algorithms.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<PrefixTreeSearchBenchmarks>();
            BenchmarkRunner.Run<SuffixTreeBenchmarks>();
            //BenchmarkRunner.Run<SuffixTreeMergeBenchmarks>();


            //var benchmarks = new PrefixTreeSearchBenchmarks();
            //benchmarks.Load();
            //var r1 = benchmarks.SearchPrefixTree();
            //var r2 = benchmarks.SearchLinear();
            //Debug.WriteLine(Enumerable.SequenceEqual(r1, r2));
        }
    }
}
