using Algorithms.DataStructures;
using BenchmarkDotNet.Attributes;
using System.Collections.Concurrent;

namespace Algorithms.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class SuffixTreeMergeBenchmarks
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

        string[] commonWords = [
            "time",
            "year",
            "people",
            //"way",
                "STRONGEST",
            "day",
            "man",
            "thing",
            "woman",
            "life",
            "child",
            "world",
            "school",
            //"state",
                "expect",
            "family",
            "student",
            "group",
            "country",
            "problem",
            "hand",
            "part",
            "place",
            "case",
            "week",
            "company",
            "system",
            "program",
            "question",
            "work",
            "government",
            "number",
            "night",
            //"point",
                "create",
            "home",
            "water",
            "room",
            "mother",
            "area",
            "money",
            "story",
            "fact",
            "month",
            "lot",
            "right",
            "study",
            "book",
            "eye",
            "job",
            "word",
            "business",
            "issue",
            //"side",
                "STRONG",
            "kind",
            "head",
            "house",
            "service",
            "friend",
            "father",
            "power",
            "hour",
            "game",
            "line",
            //"end",
                "might",
            "member",
            "law",
            "car",
            "city",
            "community",
            "name",
            "president",
            "team",
            "minute",
            "idea",
            "kid",
            "body",
            "information",
            "back",
            "parent",
            "face",
            "others",
            "level",
            "office",
            "door",
            "health",
            "person",
            "art",
            //"war",
                "continue",
            "history",
            "party",
            "result",
            "change",
            "morning",
            "reason",
            "research",
            "girl",
            "guy",
            "moment",
            //"air",
                "believe",
            "teacher",
            "force",
            "education",
            ];

        [Benchmark(Baseline = true)]
        public void Add()
        {
            var source = new SuffixTree();
            foreach (var word in verbs)
            {
                source.Add(word);
            }

            foreach (var word in commonWords)
            {
                source.Add(word);
            }

            //source.Merge(merge);
        }

        [Benchmark]
        public void CreateNoMerge()
        {
            var source = new SuffixTree();
            foreach (var word in verbs)
            {
                source.Add(word);
            }

            var merge = new SuffixTree();
            foreach (var word in commonWords)
            {
                merge.Add(word);
            }

            //source.Merge(merge);
        }

        [Benchmark]
        public void CreateMerge()
        {
            var source = new SuffixTree();
            foreach (var word in verbs)
            {
                source.Add(word);
            }

            var merge = new SuffixTree();
            foreach (var word in commonWords)
            {
                merge.Add(word);
            }

            source.Merge(merge);
        }

        [Benchmark]
        public void FindInMerged()
        {
            var source = new SuffixTree();
            foreach (var word in verbs)
            {
                source.Add(word);
            }

            var merge = new SuffixTree();
            foreach (var word in commonWords)
            {
                merge.Add(word);
            }

            source.Merge(merge);

            var resultContainer = new ConcurrentBag<string>();
            foreach(var word in source.Match("s", true))
            {
                resultContainer.Add(word);
            }
        }

        [Benchmark]
        public void FindInAdded()
        {
            var source = new SuffixTree();
            foreach (var word in verbs)
            {
                source.Add(word);
            }

            foreach (var word in commonWords)
            {
                source.Add(word);
            }

            var resultContainer = new ConcurrentBag<string>();
            foreach (var word in source.Match("s", true))
            {
                resultContainer.Add(word);
            }
        }

        [Benchmark]
        public void Find_Parallel_2()
        {
            var source = new SuffixTree();
            foreach (var word in verbs)
            {
                source.Add(word);
            }

            var merge = new SuffixTree();
            foreach (var word in commonWords)
            {
                merge.Add(word);
            }

            //source.Merge(merge);

            var resultContainer = new ConcurrentBag<string>();
            SuffixTree[] trees = [source, merge];
            Parallel.ForEach(trees, t =>
            {
                foreach (var word in t.Match("s", true))
                {
                    resultContainer.Add(word);
                }
            });
        }

        [Benchmark]
        public void Find_Parallel_10()
        {
            var source = new SuffixTree();
            foreach (var word in verbs)
            {
                source.Add(word);
            }

            var merge = new SuffixTree();
            foreach (var word in commonWords)
            {
                merge.Add(word);
            }

            //source.Merge(merge);

            var resultContainer = new ConcurrentBag<string>();
            SuffixTree[] trees = [source, merge, source, merge, source, merge, source, merge, source, merge];
            Parallel.ForEach(trees, t =>
            {
                foreach (var word in t.Match("s", true))
                {
                    resultContainer.Add(word);
                }
            });
        }
    }
}
