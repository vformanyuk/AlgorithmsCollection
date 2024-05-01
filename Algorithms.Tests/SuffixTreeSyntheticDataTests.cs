using Algorithms.DataStructures;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;

namespace Algorithms.Tests
{
    [TestClass]
    public class SuffixTreeSyntheticDataTests
    {
        private SuffixTree _testSubject;
        private SuffixTree _mergeTree;

        [TestInitialize]
        public void Setup()
        {
            _testSubject = new SuffixTree();

            _testSubject.Add("string");
            _testSubject.Add("strange");
            _testSubject.Add("Strong");
            _testSubject.Add("STRONG");
            _testSubject.Add("STRONGER");
            _testSubject.Add("STRONGEST");
            _testSubject.Add("str");
            _testSubject.Add("sword");
            _testSubject.Add("def");
            _testSubject.Add(" random");

            _mergeTree = new SuffixTree();
            _mergeTree.Add("believe");
            _mergeTree.Add("hold");
            _mergeTree.Add("bring");
            _mergeTree.Add("happen");
            _mergeTree.Add("must");
            _mergeTree.Add("write");
            _mergeTree.Add("provide");
            _mergeTree.Add("sit");
            _mergeTree.Add("stand");
            _mergeTree.Add("lose");
            _mergeTree.Add("STRONG");
            _mergeTree.Add("meet");
            _mergeTree.Add("include");
            _mergeTree.Add("continue");
            _mergeTree.Add("set");
            _mergeTree.Add("learn");
            _mergeTree.Add("change");
            _mergeTree.Add("lead");
            _mergeTree.Add("understand");
            _mergeTree.Add("sword");
        }

        [TestCleanup]
        public void TearDown()
        {
            _testSubject.Clear();
            _testSubject = null;
        }

        [TestMethod]
        public void TestAdd()
        {
            Assert.AreEqual(10, _testSubject.WordsCount);
        }

        [DataTestMethod]
        [DataRow("", 10, DisplayName = "Get all")]
        [DataRow("str", 3, DisplayName = "Test 'str' string")]
        [DataRow("s", 4, DisplayName = "Get all 's' words")]
        [DataRow("S", 4, DisplayName = "Get all 'S' words")]
        public void TestMatch(string input, int expected)
        {
            var words = _testSubject.Match(input, caseSensitive: true).ToList();
            Assert.AreEqual(expected, words.Count);
        }
        
        [TestMethod]
        public void TestMatch_NoMatch()
        {
            var words = _testSubject.Match("st0", caseSensitive: true).ToList();

            Assert.AreEqual(0, words.Count);
        }

        [TestMethod]
        public void TestMatch_KeyPart()
        {
            var words = _testSubject.Match("de", caseSensitive: true).ToList();

            Assert.AreEqual(1, words.Count, "Expected result 'def'");
            Assert.AreEqual("def", words[0], "Expected result 'def'");
        }

        [TestMethod]
        public void TestMatch_KeyPart2()
        {
            var words = _testSubject.Match("stri", caseSensitive: true).ToList();

            Assert.AreEqual(1, words.Count);
            Assert.AreEqual("string", words[0], "Expected result 'string'");
        }

        [TestMethod]
        public void TestMatch_SubBranching()
        {
            var words = _testSubject.Match("ST", caseSensitive: true).ToList();

            Assert.AreEqual(3, words.Count);

            Assert.AreEqual("STRONG", words[0]);
            Assert.AreEqual("STRONGER", words[1]);
            Assert.AreEqual("STRONGEST", words[2]);
        }

        [TestMethod]
        public void TestRemove()
        {
            Assert.AreEqual(true, _testSubject.Remove("STRONG"));

            var words = _testSubject.Match("ST", caseSensitive: true).ToList();

            Assert.AreEqual(2, words.Count);

            Assert.AreEqual("STRONGER", words[0]);
            Assert.AreEqual("STRONGEST", words[1]);
        }

        [TestMethod]
        public void TestRemove_LastLeaf()
        {
            Assert.AreEqual(true, _testSubject.Remove("STRONGEST"));

            var words = _testSubject.Match("ST", caseSensitive: true).ToList();

            Assert.AreEqual(2, words.Count);

            Assert.AreEqual("STRONG", words[0]);
            Assert.AreEqual("STRONGER", words[1]);
        }

        [TestMethod]
        public void TestRemove_LeftCommon()
        {
            Assert.AreEqual(true, _testSubject.Remove("STRONGER"));
            Assert.AreEqual(true, _testSubject.Remove("STRONGEST"));

            var words = _testSubject.Match("ST", caseSensitive: true).ToList();

            Assert.AreEqual(1, words.Count);
            Assert.AreEqual("STRONG", words[0]);
        }

        [TestMethod]
        public void TestRemove_LeftCommon_NonFinal()
        {
            Assert.AreEqual(true, _testSubject.Remove("string"));
            Assert.AreEqual(true, _testSubject.Remove("strange"));

            var words = _testSubject.Match("st", caseSensitive: true).ToList();

            Assert.AreEqual(1, words.Count);
            Assert.AreEqual("str", words[0]);
        }

        [TestMethod]
        public void TestRemove_NonExistingNode()
        {
            Assert.AreEqual(false, _testSubject.Remove("non-existant"));
        }

        [TestMethod]
        public void TestRemove_PartialyExistingNode()
        {
            Assert.AreEqual(false, _testSubject.Remove("swo"));
        }

        [TestMethod]
        public void TestMerge_ValidateCount()
        {
            _testSubject.Merge(_mergeTree);
            Assert.AreEqual(28, _testSubject.WordsCount);
        }

        [TestMethod]
        public void TestMerge_GetMergedWords()
        {
            _testSubject.Merge(_mergeTree);
            var data = _testSubject.Match("s", true).ToList();

            Assert.AreEqual(7, data.Count);
        }
    }
}
