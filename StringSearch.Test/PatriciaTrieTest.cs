// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Gma.DataStructures.StringSearch.Test
{
    [TestFixture]
    public class PatriciaTrieTest : BaseTrieTest
    {
        protected override ITrie<int> CreateTrie()
        {
            return new PatriciaTrie<int>();
        }

        [Test]
        public void TestNotExactMatched()
        {
            ITrie<int> trie = new PatriciaTrie<int>();
            trie.Add("aaabbb", 1);
            trie.Add("aaaccc", 2);

            var actual = trie.Retrieve("aab");
            CollectionAssert.AreEquivalent(Enumerable.Empty<int>(), actual);

        }
    }
}