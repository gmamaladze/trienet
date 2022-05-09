using System;
using System.Collections.Generic;
using System.Linq;

namespace Gma.DataStructures.StringSearch
{
    internal class Node<K, T> where K : IComparable<K>
    {
        private readonly IDictionary<K, Edge<K, T>> _edges;
        private readonly HashSet<T> _data;

        public Node()
        {
            _edges = new EdgeDictionary<K, T>();
            Suffix = null;
            _data = new HashSet<T>();
        }

        public IEnumerable<Node<K, T>> Children() {
            return _edges.Values.Select(e => e.Target);
        }

        public long Size() {
            return Children().Sum(o => o.Size()) + 1;
        }

        public IEnumerable<T> GetData()
        {
            var childData = _edges.Values.Select((e) => e.Target).SelectMany((t) => t.GetData());
            return _data.Concat(childData).Distinct();
        }

        public void AddRef(T value)
        {
            if (_data.Contains(value))
                return;

            _data.Add(value);
            //  add this reference to all the suffixes as well
            var iter = Suffix;
            while (iter != null)
            {
                if (iter._data.Contains(value))
                    break;

                iter._data.Add(value);
                iter = iter.Suffix;
            }
        }

        public void AddEdge(K ch, Edge<K, T> e)
        {
            _edges[ch] = e;
        }

        public Edge<K, T> GetEdge(K ch)
        {
            Edge<K, T> result;
            _edges.TryGetValue(ch, out result);
            return result;
        }

        public IEnumerable<Edge<K, T>> GetEdgesBetween(K min, K max)
        {
            foreach (var ch in _edges.Keys) {
                if (ch.CompareTo(min) >= 0 && ch.CompareTo(max) <= 0) {
                    yield return _edges[ch];
                }
            }
        }

        public Node<K, T> Suffix { get; set; }
    }
}