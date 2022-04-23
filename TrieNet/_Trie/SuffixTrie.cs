// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gma.DataStructures.StringSearch
{
    [Serializable]
    public class SuffixTrie<T> : ISuffixTrie<T>
    {
        private readonly Trie<WordPosition<T>> m_InnerTrie;
        private readonly int m_MinSuffixLength;

        public SuffixTrie(int minSuffixLength)
            : this(new Trie<WordPosition<T>>(), minSuffixLength)
        {
        }

        private SuffixTrie(Trie<WordPosition<T>> innerTrie, int minSuffixLength)
        {
            m_InnerTrie = innerTrie;
            m_MinSuffixLength = minSuffixLength;
        }

        public long Size {
            get {
                return m_InnerTrie.Size();
            }
        }

        public IEnumerable<T> Retrieve(string word)
        {
            return RetrieveSubstrings(word).Select(o => o.Value).Distinct();
        }

        public IEnumerable<WordPosition<T>> RetrieveSubstrings(string query)
        {
            return
                m_InnerTrie
                    .Retrieve(query)
                    .Distinct();
        }

        public void Add(string key, T value)
        {
            foreach ((string suffix, int position) in GetAllSuffixes(m_MinSuffixLength, key))
            {
                m_InnerTrie.Add(suffix, new WordPosition<T>(position, value));
            }
        }

        private static IEnumerable<Tuple<string, int>> GetAllSuffixes(int minSuffixLength, string word)
        {
            for (int i = word.Length - minSuffixLength; i >= 0; i--)
            {
                var partition = new StringPartition(word, i);
                yield return new Tuple<string, int>(partition.ToString(), i);
            }
        }
    }
}