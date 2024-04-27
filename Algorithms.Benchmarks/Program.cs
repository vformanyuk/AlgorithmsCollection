using Algorithms.DataStructures;
using BenchmarkDotNet.Running;

namespace Algorithms.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<TrieBenchmarks>();

            //var benchmarks = new TrieBenchmarks();
            //benchmarks.RemoveAll();
        }
    }
}
