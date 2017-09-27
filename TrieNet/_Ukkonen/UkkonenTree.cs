using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Gma.DataStructures.StringSearch._Ukkonen
{
    public class UkkonenTree<TValue> : ITrie<TValue>
    {
        private readonly GeneralizedSuffixTree _inner;
        private readonly List<TValue> _values;

        public UkkonenTree()
        {
            _inner = new GeneralizedSuffixTree();
            _values = new List<TValue>();
        }
        
        public IEnumerable<TValue> Retrieve(string query)
        {
            return _inner.search(query).Select((index) => _values[index]);
        }

        public void Add(string key, TValue value)
        {
            var index = _values.Count;
            _values.Add(value);
            _inner.put(key, index);
        }
    }
}