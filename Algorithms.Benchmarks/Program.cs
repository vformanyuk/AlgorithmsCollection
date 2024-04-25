using Algorithms.DataStructures;
using BenchmarkDotNet.Running;

namespace Algorithms.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<TrieBenchmarks>();

            //var _testSubject = new Trie(caseSensetive: true);

            //_testSubject.Add("string");
            //_testSubject.Add("strange");
            //_testSubject.Add("Strong");
            //_testSubject.Add("STRONG");
            //_testSubject.Add("STRONGER");
            //_testSubject.Add("STRONGEST");
            //_testSubject.Add("str");
            //_testSubject.Add("sword");
            //_testSubject.Add("def");
            //_testSubject.Add(" random");

            //_testSubject.Match("Str");
        }
    }
}
