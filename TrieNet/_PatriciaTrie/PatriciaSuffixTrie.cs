// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gma.DataStructures.StringSearch
{
    [Serializable]
    public class PatriciaSuffixTrie<TValue> : ISuffixTrie<TValue>
    {
        private readonly int m_MinQueryLength;
        private readonly PatriciaTrie<WordPosition<TValue>> m_InnerTrie;

        public PatriciaSuffixTrie(int minQueryLength)
            : this(minQueryLength, new PatriciaTrie<WordPosition<TValue>>())
        {
            
        }

        internal PatriciaSuffixTrie(int minQueryLength, PatriciaTrie<WordPosition<TValue>> innerTrie)
        {
            m_MinQueryLength = minQueryLength;
            m_InnerTrie = innerTrie;
        }

        protected int MinQueryLength
        {
            get { return m_MinQueryLength; }
        }

        public long Size {
            get {
                return m_InnerTrie.Size();
            }
        }

        public IEnumerable<TValue> Retrieve(string word)
        {
            return RetrieveSubstrings(word).Select(o => o.Value).Distinct();
        }

        public IEnumerable<WordPosition<TValue>> RetrieveSubstrings(string query)
        {
            return
                m_InnerTrie
                    .Retrieve(query)
                    .Distinct();
        }

        public void Add(string key, TValue value)
        {
            foreach ((StringPartition currentSuffix, int position) in GetAllSuffixes(MinQueryLength, key))
            {
                m_InnerTrie.Add(currentSuffix, new WordPosition<TValue>(position, value));
            }
        }

        private static IEnumerable<Tuple<StringPartition, int>> GetAllSuffixes(int minSuffixLength, string word)
        {
            for (int i = word.Length - minSuffixLength; i >= 0; i--)
            {
                yield return new Tuple<StringPartition, int>(new StringPartition(word, i), i);
            }
        }
    }
}