// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System;
using System.Collections.Generic;

namespace Gma.DataStructures.StringSearch
{
    public class PatriciaTrie<TValue> :
        PatriciaTrieNode<TValue>,
        ITrie<TValue>
    {
        public PatriciaTrie()
            : base(
                new StringPartition(string.Empty),
                new Queue<TValue>(),
                new Dictionary<char, PatriciaTrieNode<TValue>>())
        {
        }

        public IEnumerable<TValue> Retrieve(string query)
        {
            return Retrieve(query, 0);
        }

        public virtual void Add(string key, TValue value)
        {
            if (key == null) throw new ArgumentNullException("key");
            Add(new StringPartition(key), value);
        }

        internal override void Add(StringPartition keyRest, TValue value)
        {
            GetOrCreateChild(keyRest, value);
        }
    }
}