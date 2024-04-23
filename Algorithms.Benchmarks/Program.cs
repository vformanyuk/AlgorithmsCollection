using BenchmarkDotNet.Running;

namespace Algorithms.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<TrieBenchmarks>();

            //var benchmark = new ApexReplayBufferInsertBenchmark();
            //Console.WriteLine("Filling up replay buffer");
            //benchmark.Setup();
            //if (!benchmark.Buffer.IsComplete())
            //{
            //    Console.WriteLine("Data lost on buffer fillup");
            //    return;
            //}
            //Console.WriteLine("Beginning insert operation");
            //var watch = Stopwatch.StartNew();
            ////benchmark.InSort_LinearSearch(); //783 ms
            ////benchmark.DelaySort(); // 360 ms
            ////benchmark.InSort_BinarySearch(); // 64 ms
            ////benchmark.InSort_BinarySearch_SIMD();
            //for (int i = 0; i < 40; i++)
            //{
            //    benchmark.InSort_BinarySearch_SIMD(); // 64 ms
            //    //benchmark.InSort_BinarySearch(); // 64 ms
            //    //benchmark.InSort_BinarySearch_SIMD_Linear_SIMD(); // 64 ms
            //}
            //watch.Stop();
            //Console.WriteLine($"{watch.ElapsedMilliseconds} ms");
        }
    }
}
