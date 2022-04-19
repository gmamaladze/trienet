using System;

namespace Gma.DataStructures.StringSearch
{
    internal class Edge<T>
    {
        public Edge(ReadOnlyMemory<char> label, Node<T> target)
        {
            this.Label = label;
            this.Target = target;
        }

        public ReadOnlyMemory<char> Label { get; set; }

        public Node<T> Target { get; private set; }
    }
}