// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System;
using System.Collections.Generic;

namespace Gma.DataStructures.StringSearch
{
    public class Trie<TValue> : TrieNode<TValue>, ITrie<TValue>
    {
        public IEnumerable<TValue> Retrieve(string query)
        {
            return Retrieve(query, 0);
        }

        public void Add(string key, TValue value)
        {
            Add(key, 0, value);
        }

        public void Remove(string key)
        {
            Remove(key, 0);
        }

        public void Update(string key, TValue[] values)
        {
            Update(key, 0, values);
        }
    }
}