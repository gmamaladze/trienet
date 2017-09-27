using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Gma.DataStructures.StringSearch._Ukkonen
{
    class Node
    {

        private int[] data;

        private int lastIdx = 0;

        private static int START_SIZE = 0;

        private static int INCREMENT = 1;

        private IDictionary<char, Edge> edges;

        private Node suffix;

        private int resultCount = -1;

        public Node()
        {
            this.edges = new EdgeBag();
            this.suffix = null;
            this.data = new int[START_SIZE];
        }

        public IEnumerable<int> getData()
        {
            return this.getData(-1);
        }

        public IEnumerable<int> getData(int numElements)
        {
            ISet<int> ret = new HashSet<int>();
            foreach (int num in this.data)
            {
                ret.Add(num);
                if ((ret.Count == numElements))
                {
                    return ret;
                }
            }

            //  need to get more matches from child nodes. This is what may waste time
            foreach (Edge e in this.edges.Values)
            {
                if (((-1 == numElements)
                     || (ret.Count < numElements)))
                {
                    foreach (int num in e.getDest().getData())
                    {
                        ret.Add(num);
                        if ((ret.Count == numElements))
                        {
                            return ret;
                        }

                    }

                }

            }

            return ret;
        }

        public void addRef(int index)
        {
            if (this.contains(index))
            {
                return;
            }

            this.addIndex(index);
            //  add this reference to all the suffixes as well
            Node iter = this.suffix;
            while ((iter != null))
            {
                if (iter.contains(index))
                {
                    break;
                }

                iter.addRef(index);
                iter = iter.suffix;
            }

        }

        private bool contains(int index)
        {
            int low = 0;
            int high = lastIdx - 1;

            while (low <= high) {
                int mid = (low + high) >> 1;
                int midVal = data[mid];

                if (midVal < index)
                    low = mid + 1;
                else if (midVal > index)
                    high = mid - 1;
                else
                    return true;
            }
            return false;
            // Java 5 equivalent to
            // return java.util.Arrays.binarySearch(data, 0, lastIdx, index) >= 0;
        }

        public int computeAndCacheCount()
        {
            this.computeAndCacheCountRecursive();
            return this.resultCount;
        }

        private ISet<int> computeAndCacheCountRecursive()
        {
            ISet<int> ret = new HashSet<int>();
            foreach (int num in this.data)
            {
                ret.Add(num);
            }

            foreach (Edge e in this.edges.Values)
            {
                foreach (int num in e.getDest().computeAndCacheCountRecursive())
                {
                    ret.Add(num);
                }

            }

            this.resultCount = ret.Count;
            return ret;
        }

        public int getResultCount()
        {
            if ((-1 == this.resultCount))
            {
                throw new ArgumentException(
                    "getResultCount() shouldn\'t be called without calling computeCount() first");
            }

            return this.resultCount;
        }

        public void addEdge(char ch, Edge e)
        {
            this.edges[ch] = e;
        }

        public Edge getEdge(char ch)
        {
            Edge result = null;
            this.edges.TryGetValue(ch, out result);
            return result;
        }

        IDictionary<char, Edge> getEdges()
        {
            return this.edges;
        }

        public Node getSuffix()
        {
            return this.suffix;
        }

        public void setSuffix(Node suffix)
        {
            this.suffix = suffix;
        }

        private void addIndex(int index)
        {
            if ((this.lastIdx == this.data.Length))
            {
                int[] copy = new int[(this.data.Length + INCREMENT)];
                Array.Copy(this.data, 0, copy, 0, this.data.Length);
                this.data = copy;
            }

            this.data[lastIdx++] = index;
        }
    }
}