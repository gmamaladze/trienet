using System;

namespace Gma.DataStructures.StringSearch
{
    internal class Edge<K, T> where K : IComparable<K>
    {
        public Edge(ReadOnlyMemory<K> label, Node<K, T> target)
        {
            this.Label = label;
            this.Target = target;
        }

        public ReadOnlyMemory<K> Label { get; set; }

        public Node<K, T> Target { get; private set; }
    }
}