using System;
using System.Collections.Generic;

namespace Gma.DataStructures.StringSearch
{
    internal class Node
    {
        private const int StartSize = 0;
        private const int Increment = 1;
        
        private readonly IDictionary<char, Edge> _edges;
        private int[] _data;
        private int _lastIdx;
        private int _resultCount = -1;

        public Node()
        {
            _edges = new EdgeBag();
            Suffix = null;
            _data = new int[StartSize];
        }

        public IEnumerable<int> GetData()
        {
            return GetData(-1);
        }

        public IEnumerable<int> GetData(int numElements)
        {
            var ret = new HashSet<int>();
            foreach (var num in _data)
            {
                ret.Add(num);
                if (ret.Count == numElements)
                    return ret;
            }

            //  need to get more matches from child nodes. This is what may waste time
            foreach (var e in _edges.Values)
                if (-1 == numElements || ret.Count < numElements)
                    foreach (var num in e.Target.GetData())
                    {
                        ret.Add(num);
                        if (ret.Count == numElements)
                            return ret;
                    }

            return ret;
        }

        public void AddRef(int index)
        {
            if (Contains(index))
                return;

            AddIndex(index);
            //  add this reference to all the suffixes as well
            var iter = Suffix;
            while (iter != null)
            {
                if (iter.Contains(index))
                    break;

                iter.AddRef(index);
                iter = iter.Suffix;
            }
        }

        private bool Contains(int index)
        {
            return Array.BinarySearch(_data, index) >= 0;
        }

        public int ComputeAndCacheCount()
        {
            ComputeAndCacheCountRecursive();
            return _resultCount;
        }

        private ISet<int> ComputeAndCacheCountRecursive()
        {
            ISet<int> ret = new HashSet<int>();
            foreach (var num in _data)
                ret.Add(num);

            foreach (var e in _edges.Values)
            foreach (var num in e.Target.ComputeAndCacheCountRecursive())
                ret.Add(num);

            _resultCount = ret.Count;
            return ret;
        }

        public int GetResultCount()
        {
            if (-1 == _resultCount)
                throw new InvalidOperationException(
                    "getResultCount() shouldn\'t be called without calling computeCount() first");

            return _resultCount;
        }

        public void AddEdge(char ch, Edge e)
        {
            _edges[ch] = e;
        }

        public Edge GetEdge(char ch)
        {
            Edge result = null;
            _edges.TryGetValue(ch, out result);
            return result;
        }

        public Node Suffix { get; set; }

        private void AddIndex(int index)
        {
            if (_lastIdx == _data.Length)
            {
                var copy = new int[_data.Length + Increment];
                Array.Copy(_data, 0, copy, 0, _data.Length);
                _data = copy;
            }

            _data[_lastIdx++] = index;
        }
    }
}