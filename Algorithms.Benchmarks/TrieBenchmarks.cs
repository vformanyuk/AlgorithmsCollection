using Algorithms.DataStructures;
using BenchmarkDotNet.Attributes;

namespace Algorithms.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class TrieBenchmarks
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
            var trie = new Trie(caseSensetive: true);
            foreach (var word in verbs)
            {
                trie.Add(word);
            }
        }

        [Benchmark]
        public void Find()
        {
            var trie = new Trie(caseSensetive: true);
            foreach (var word in verbs)
            {
                trie.Add(word);
            }

            trie.Get("STR");
            trie.Get("co");
        }

        [Benchmark]
        public void Remove()
        {
            var trie = new Trie(caseSensetive: true);
            foreach (var word in verbs)
            {
                trie.Add(word);
            }

            trie.Remove("STRONG");
            trie.Remove("expect");
            trie.Remove("show");
            trie.Remove("strange");
            trie.Remove("serve");
            trie.Remove("keep");
            trie.Remove("would");
            trie.Remove("be");
            trie.Remove("continue");
            trie.Remove("move");
        }
    }
}
