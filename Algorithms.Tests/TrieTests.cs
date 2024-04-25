using Algorithms.DataStructures;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;

namespace Algorithms.Tests
{
    [TestClass]
    public class TrieTests
    {
        private Trie _testSubject;

        [TestInitialize]
        public void Setup()
        {
            _testSubject = new Trie(caseSensetive: true);

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
        public void TestGet(string input, int expected)
        {
            var words = _testSubject.Get(input).ToList();
            Assert.AreEqual(expected, words.Count);

            foreach (var w in words)
            {
                Console.WriteLine(w);
            }
        }

        [TestMethod]
        public void TestGet_NoMatch()
        {
            var words = _testSubject.Get("st0").ToList();

            Assert.AreEqual(0, words.Count);
        }

        [TestMethod]
        public void TestGet_KeyPart()
        {
            var words = _testSubject.Get("de").ToList();

            Assert.AreEqual(1, words.Count, "Expected result 'def'");
        }

        [TestMethod]
        public void TestGet_KeyPart2()
        {
            var words = _testSubject.Get("stri").ToList();

            Assert.AreEqual(1, words.Count, "Expected result 'string'");
        }

        [TestMethod]
        public void TestGet_SubBranching()
        {
            var words = _testSubject.Get("ST").ToList();

            Assert.AreEqual(3, words.Count);

            foreach (var w in words)
            {
                Console.WriteLine(w);
            }
        }

        [TestMethod]
        public void TestRemove()
        {
            Assert.AreEqual(true, _testSubject.Remove("STRONG"));

            var words = _testSubject.Get("ST").ToList();

            Assert.AreEqual(2, words.Count);
        }

        [TestMethod]
        public void TestRemove_LastLeaf()
        {
            Assert.AreEqual(true, _testSubject.Remove("STRONGEST"));

            var words = _testSubject.Get("ST").ToList();

            Assert.AreEqual(2, words.Count);
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
    }
}
