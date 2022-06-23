using System;
using System.Collections.Generic;

namespace Gma.DataStructures.StringSearch
{
    internal class EdgeDictionary<K, T> : Dictionary<K, Edge<K, T>> where K : IComparable<K>
    {
        //TODO Consider using sorted list based implementation to save memory
    }
}