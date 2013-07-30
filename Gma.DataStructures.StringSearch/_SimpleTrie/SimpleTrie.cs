using System.Collections.Generic;

namespace Gma.DataStructures.StringSearch
{
    public class SimpleTrie<TValue> : TrieNode<TValue>, ITrie<TValue>
    {
        public IEnumerable<TValue> Retrieve(string query)
        {
            return Find(query, 0);
        }

        public void Add(string key, TValue value)
        {
            Add(key, 0, value);
        }
    }
}