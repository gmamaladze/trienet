using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Gma.DataStructures.StringSearch._Ukkonen
{
    public class UkkonenTree<TValue> : ITrie<TValue>
    {
        private readonly int _minSuffixLength;
        private readonly GeneralizedSuffixTree _inner;
        private readonly List<TValue> _values;

        public UkkonenTree() : this(3)
        {
           
        }
        
        public UkkonenTree(int minSuffixLength)
        {
            //if (minSuffixLength < 3) throw new ArgumentOutOfRangeException(nameof(minSuffixLength), "Minimum 3");
            _minSuffixLength = minSuffixLength;
            _inner = new GeneralizedSuffixTree();
            _values = new List<TValue>();
        }
        
        public IEnumerable<TValue> Retrieve(string query)
        {
            if (query.Length <= _minSuffixLength) return Enumerable.Empty<TValue>();
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