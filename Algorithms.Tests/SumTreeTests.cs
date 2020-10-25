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
        public void TestUpdate()
        {
            _testSubject.Update(11, 1);
            Assert.AreEqual(66, _testSubject.Total);
        }

        [DataTestMethod]
        [DataRow(0, 9, DisplayName = "Lower bound")]
        [DataRow(67, 8, DisplayName = "Upper bound")]
        [DataRow(22, 1, DisplayName = "Single")]
        [DataRow(54, 18, DisplayName = "Transition 1")]
        [DataRow(55, 5, DisplayName = "Transition 1")]
        [DataRow(56, 5, DisplayName = "Transition 1")]
        public void TestGet(int input, int expected)
        {
            var result = _testSubject.Get(input);
            Assert.AreEqual(expected, result);
        }
    } 
}
