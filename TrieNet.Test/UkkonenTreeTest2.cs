using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Gma.DataStructures.StringSearch.Test
{
    public class UkkonenTreeTest2 {
        private CharUkkonenTrie<int> trie = null!;

        [OneTimeSetUp]
        public void Setup() {
            trie =  new CharUkkonenTrie<int>(0);
            trie.Add("aabacdefac", 0);
        }

        [TestCase("a", new[] { 0, 1, 3, 8 })]
        [TestCase("b", new[] { 2 })]
        [TestCase("c", new[] { 4, 9 })]
        [TestCase("d", new[] { 5 })]
        [TestCase("e", new[] { 6 })]
        [TestCase("f", new[] { 7 })]
        [TestCase("ac", new[] { 3, 8 })]
        [TestCase("bac", new[] { 2 })]
        [TestCase("abac", new[] { 1 })]
        [TestCase("aabac", new[] { 0 })]
        public void TestFuzzyExact(string query, IEnumerable<int> expected) {
            IEnumerable<WordPosition<int>> actual = trie.RetrieveSubstringsRange(query.AsMemory(), query.AsMemory());
            CollectionAssert.AreEquivalent(expected, actual.Select(o => o.CharPosition));
        }

        [TestCase("a", "b", new[] { 0, 1, 2, 3, 8 })]
        [TestCase("a", "c", new[] { 0, 1, 2, 3, 4, 8, 9 })]
        [TestCase("a", "d", new[] { 0, 1, 2, 3, 4, 5, 8, 9 })]
        [TestCase("a", "e", new[] { 0, 1, 2, 3, 4, 5, 6, 8, 9 })]
        [TestCase("a", "f", new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })]
        [TestCase("aaaaaaaaaa", "ffffffffff", new[] { 0 })]
        [TestCase("ab", "af", new[] { 1, 3, 8 })]
        public void TestFuzzy(string min, string max, IEnumerable<int> expected) {
            IEnumerable<WordPosition<int>> actual = trie.RetrieveSubstringsRange(min.AsMemory(), max.AsMemory());
            CollectionAssert.AreEquivalent(expected, actual.Select(o => o.CharPosition));
        }
    }
}