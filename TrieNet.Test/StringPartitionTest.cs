// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using NUnit.Framework;

namespace Gma.DataStructures.StringSearch.Test
{
    [TestFixture]
    public class StringPartitionTest
    {
        [TestCase("hello", "aaa", false)]
        [TestCase("hello", "h", true)]
        [TestCase("hello", "", true)]
        [TestCase("hello", "hello", true)]
        [TestCase("hello", "hel", true)]
        [TestCase("hello", "halloa", false)]
        [TestCase("hello", "halla", false)]
        [TestCase("hello", "haXlo", false)]
        public void StartsWithTest(string inputThis, string inputOther, bool expectedResult)
        {
            var target = new StringPartition(inputThis);
            var other = new StringPartition(inputOther);

            var actual = target.StartsWith(other);
            Assert.AreEqual(expectedResult, actual);
        }

        [TestCase("", "", "", "", "")]
        [TestCase("a", "a", "a", "", "")]
        [TestCase("ab", "a", "a", "b", "")]
        [TestCase("a", "ab", "a", "", "b")]
        [TestCase("ab", "ac", "a", "b", "c")]
        [TestCase("a", "b", "", "a", "b")]
        [TestCase("hello", "hell", "hell", "o", "")]
        [TestCase("hello", "helios", "hel", "lo", "ios")]
        [TestCase("hell", "hello", "hell", "", "o")]
        public void ZipTest(string inputThis, string inputOther, string expectedCommonHead, string expectedThisRest,
                            string expectedOtherRest)
        {
            var target = new StringPartition(inputThis);
            var other = new StringPartition(inputOther);
            var actual = target.ZipWith(other);

            Assert.AreEqual(expectedCommonHead, actual.CommonHead.ToString());
            Assert.AreEqual(expectedThisRest, actual.ThisRest.ToString());
            Assert.AreEqual(expectedOtherRest, actual.OtherRest.ToString());
        }

        [TestCase("ab", 0, 2, 1, "a", "b")]
        [TestCase("a", 0, 1, 0, "", "a")]
        [TestCase("a", 0, 1, 1, "a", "")]
        [TestCase("XabX", 1, 2, 1, "a", "b")]
        public void SplitTest(string origin, int start, int length, int splitAt, string expectedHead,
                              string expectedRest)
        {
            var target = new StringPartition(origin, start, length);
            var actual = target.Split(splitAt);
            Assert.AreEqual(expectedHead, actual.Head);
            Assert.AreEqual(expectedRest, actual.Rest);
        }
    }
}