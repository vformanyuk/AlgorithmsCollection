using Algorithms.DataStructures;
using BenchmarkDotNet.Running;

namespace Algorithms.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SuffixTreeBenchmarks>();
            //BenchmarkRunner.Run<SuffixTreeMergeBenchmarks>();


            //var benchmarks = new SuffixTreeMergeBenchmarks();
            //benchmarks.CreateMerge();
        }
    }
}
