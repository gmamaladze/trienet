using System;
using System.Linq;
using NUnit.Framework;

namespace Gma.DataStructures.StringSearch.Test
{
    public class TrieTest : BaseTrieTest
    {
        protected override ITrie<int> CreateTrie()
        {
            return new Trie<int>();
        }

        [Test]
        [ExpectedException(typeof(AggregateException))]
        [Explicit]
        public void ExhaustiveParallelAddFails()
        {
            while (true)
            {
                ITrie<int> trie = CreateTrie();
                LongPhrases40
                    .AsParallel()
                    .ForAll(phrase => trie.Add(phrase, phrase.GetHashCode()));
            }
        }
    }
}