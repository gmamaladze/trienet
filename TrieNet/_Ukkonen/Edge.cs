namespace Gma.DataStructures.StringSearch
{
    internal class Edge
    {
        public Edge(string label, Node target)
        {
            this.Label = label;
            this.Target = target;
        }

        public string Label { get; set; }

        public Node Target { get; private set; }
    }
}