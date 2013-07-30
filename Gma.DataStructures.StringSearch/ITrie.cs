using System.Collections.Generic;

namespace Gma.DataStructures.StringSearch
{
    public interface ITrie<TValue>
    {
        IEnumerable<TValue> Retrieve(string query);
        void Add(string key, TValue value);
    }
}