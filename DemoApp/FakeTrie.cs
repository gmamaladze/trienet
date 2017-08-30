// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System.Collections.Generic;

namespace Gma.DataStructures.StringSearch.DemoApp
{
    public class FakeTrie<T> : ITrie<T>
    {
        private readonly Stack<KeyValuePair<string, T>> m_Stack;

        public FakeTrie()
        {
            m_Stack = new Stack<KeyValuePair<string, T>>();
        }

        public IEnumerable<T> Retrieve(string query)
        {
            foreach (var keyValuePair in m_Stack)
            {
                string key = keyValuePair.Key;
                T value = keyValuePair.Value;
                if (key.Contains(query)) yield return value;
            }
        }

        public void Add(string key, T value)
        {
            var keyValPair = new KeyValuePair<string, T>(key, value);
            m_Stack.Push(keyValPair);
        }
    }
}