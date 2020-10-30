using Algorithms.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms.Tests
{
    [TestClass]
    public class SumTreeTests
    {
        private SumTree _testSubject;

        [TestInitialize]
        public void Setup()
        {
            _testSubject = new SumTree(capacity: 8);

            _testSubject.Add(9);
            _testSubject.Add(13);
            _testSubject.Add(1);
            _testSubject.Add(3);
            _testSubject.Add(11);
            _testSubject.Add(18);
            _testSubject.Add(5);
            _testSubject.Add(8);

            Console.WriteLine(_testSubject.ToString());
        }

        [TestCleanup]
        public void TearDown()
        {
            _testSubject = null;
        }

        [TestMethod]
        public void TestAdd()
        {
            Assert.AreEqual(68, _testSubject.Total);
        }

        [TestMethod]
        public void TestGetOverflow()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _testSubject.Get(_testSubject.Total + 1));
        }

        [TestMethod]
        public void TestAddOverlap()
        {
            _testSubject.Add(10);
            Assert.AreEqual(69, _testSubject.Total);
        }

        [TestMethod]
        public void TestDelete()
        {
            _testSubject.Delete(0);
            Assert.AreEqual(59, _testSubject.Total);
        }

        [TestMethod]
        public void TestPartialFill()
        {
            var tree = new SumTree(capacity: 8);

            tree.Add(9);
            tree.Add(13);

            Assert.AreEqual(9, tree.Get(3));
            Assert.AreEqual(13, tree.Get(17));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => tree.Get(25));
        }

        [TestMethod]
        public void TestUpdate()
        {
            _testSubject.Update(20, 6);
            Assert.AreEqual(83, _testSubject.Total);

            var result = _testSubject.Get(70); //hit updated interval
            Assert.AreEqual(20, result);
        }

        [DataTestMethod]
        [DataRow(0, 9, DisplayName = "Lower bound")]
        [DataRow(67, 8, DisplayName = "Upper bound")]
        [DataRow(22, 1, DisplayName = "Single")]
        [DataRow(54, 18, DisplayName = "Transition 1")]
        [DataRow(55, 5, DisplayName = "Transition 2")]
        [DataRow(56, 5, DisplayName = "Transition 3")]
        public void TestGet(int input, int expected)
        {
            var result = _testSubject.Get(input);
            Assert.AreEqual(expected, result);
        }
    } 
}
