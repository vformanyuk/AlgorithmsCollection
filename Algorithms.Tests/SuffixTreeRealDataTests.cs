using Algorithms.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.Tests
{
    [TestClass]
    public class SuffixTreeRealDataTests
    {
        private SuffixTree _testSubject;
        private SuffixTree _mergeTree;

        private int _uniqCount;

        [TestInitialize]
        public void Setup()
        {
            string[] verbs = [
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
                "strange",
            "teacher",
            "force",
            "education",
            ];

            var commonCount = verbs.Intersect(commonWords).Count();
            _uniqCount = verbs.Length + commonWords.Length - commonCount;

            _testSubject = new SuffixTree();
            foreach (var word in verbs)
            {
                _testSubject.Add(word);
            }

            _mergeTree = new SuffixTree();
            foreach (var word in commonWords)
            {
                _mergeTree.Add(word);
            }
        }

        [TestMethod]
        public void TestMerge_ValidateCount()
        {
            _testSubject.Merge(_mergeTree);
            Assert.AreEqual(_uniqCount, _testSubject.WordsCount);
        }

        [TestMethod]
        public void TestMerge_GetMergedWords()
        {
            _testSubject.Merge(_mergeTree);
            var data = _testSubject.Match("s", true).ToList();

            Assert.AreEqual(22, data.Count);
        }
    }
}
