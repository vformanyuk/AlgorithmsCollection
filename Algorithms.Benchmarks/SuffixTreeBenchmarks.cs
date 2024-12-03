using Algorithms.DataStructures;
using BenchmarkDotNet.Attributes;

namespace Algorithms.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class SuffixTreeBenchmarks
    {
        private string[] verbs = [
                "be",
                "have",
                "do",
                "say",
                "go",
                "can",
                "get",
                "would",
                "make",
                "know",
                "will",
                "think",
            "strange",
                "take",
                "see",
                "come",
                "could",
                "want",
                "look",
                "use",
                "find",
                "give",
                "tell",
                "work",
                "may",
                "should",
                "call",
                "try",
                "ask",
                "need",
            "Strong",
                "feel",
                "become",
                "leave",
            "STRONG",
                "put",
                "mean",
                "keep",
                "let",
                "begin",
                "seem",
            "STRONGER",
                "help",
                "talk",
                "turn",
                "start",
                "might",
                "show",
                "hear",
                "play",
                "run",
                "move",
                "like",
                "live",
                "believe",
                "hold",
                "bring",
                "happen",
                "must",
                "write",
                "provide",
                "sit",
                "stand",
                "lose",
                "pay",
                "meet",
                "include",
                "continue",
                "set",
                "learn",
                "change",
                "lead",
                "understand",
                "watch",
                "follow",
                "stop",
                "create",
                "speak",
                "read",
                "allow",
                "add",
                "spend",
                "grow",
                "open",
                "walk",
                "win",
            "STRONGEST",
                "offer",
                "remember",
                "love",
                "consider",
                "appear",
                "buy",
                "wait",
                "serve",
                "die",
                "send",
                "expect",
                "build",
                "stay",
                "fall",
                "cut",
                "reach",
                "kill",
                "remain"];

        [Benchmark(Baseline = true)]
        public void Load()
        {
            var PrefixTree = new PrefixTree();
            foreach (var word in verbs)
            {
                PrefixTree.Add(word);
            }
        }

        [Benchmark]
        public void FindMatch()
        {
            var PrefixTree = new PrefixTree();
            foreach (var word in verbs)
            {
                PrefixTree.Add(word);
            }

            PrefixTree.Match("con", caseSensitive: true);
        }

        [Benchmark]
        public void FindWildcardMatch()
        {
            var PrefixTree = new PrefixTree('*');
            foreach (var word in verbs)
            {
                PrefixTree.Add(word);
            }

            PrefixTree.Match("c*n", caseSensitive: true);
        }

        [Benchmark]
        public bool RemoveAll()
        {
            var PrefixTree = new PrefixTree();
            foreach (var word in verbs)
            {
                PrefixTree.Add(word);
            }

            bool result = true;
            foreach (var word in verbs)
            {
                result &= PrefixTree.Remove(word);
            }
            return result;
        }
    }
}
