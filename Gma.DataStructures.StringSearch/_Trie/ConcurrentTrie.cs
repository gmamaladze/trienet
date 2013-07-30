using System.Collections.Generic;

namespace Gma.DataStructures.StringSearch
{
    public class ConcurrentTrie<TValue> : ConcurrentTrieNode<TValue>, ITrie<TValue>
    {
        #region ITrie<TValue> Members

        public IEnumerable<TValue> Retrieve(string query)
        {
            return Find(query, 0);
        }

        public void Add(string key, TValue value)
        {
            Add(key, 0, value);
        }

        #endregion
    }
}