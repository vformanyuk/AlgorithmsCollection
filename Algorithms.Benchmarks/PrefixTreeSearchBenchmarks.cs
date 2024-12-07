using Algorithms.DataStructures;
using Algorithms.Extensions;
using BenchmarkDotNet.Attributes;

namespace Algorithms.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class PrefixTreeSearchBenchmarks
    {
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
                "strange",
            "teacher",
            "force",
            "education",
            ];

        private readonly PrefixTree _prefixTree = new PrefixTree('*');

        [GlobalSetup]
        public void Load()
        {
            foreach (var word in commonWords)
            {
                _prefixTree.Add(word);
            }
        }

        [Benchmark(Baseline = true)]
        public string[] SearchLinear()
        {
            return commonWords.Where(w => w.IndexOf("res", StringComparison.InvariantCultureIgnoreCase) >= 0).ToArray();
        }

        [Benchmark]
        public string[] SearchPrefixTree()
        {
            return _prefixTree.MatchContains("res", caseSensitive: false).ToArray();
        }
    }
}
