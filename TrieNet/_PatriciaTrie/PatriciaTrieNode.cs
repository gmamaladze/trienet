// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Gma.DataStructures.StringSearch
{
    [DebuggerDisplay("'{m_Key}'")]
    public class PatriciaTrieNode<TValue> : TrieNodeBase<TValue>
    {
        private Dictionary<char, PatriciaTrieNode<TValue>> m_Children;
        private StringPartition m_Key;
        private Queue<TValue> m_Values;

        protected PatriciaTrieNode(StringPartition key, TValue value)
            : this(key, new Queue<TValue>(new[] {value}), new Dictionary<char, PatriciaTrieNode<TValue>>())
        {
        }

        protected PatriciaTrieNode(StringPartition key, Queue<TValue> values,
            Dictionary<char, PatriciaTrieNode<TValue>> children)
        {
            m_Values = values;
            m_Key = key;
            m_Children = children;
        }

        protected override int KeyLength
        {
            get { return m_Key.Length; }
        }

        protected override IEnumerable<TValue> Values()
        {
            return m_Values;
        }

        protected override IEnumerable<TrieNodeBase<TValue>> Children()
        {
            return m_Children.Values;
        }


        protected override void AddValue(TValue value)
        {
            m_Values.Enqueue(value);
        }

        internal virtual void Add(StringPartition keyRest, TValue value)
        {
            ZipResult zipResult = m_Key.ZipWith(keyRest);

            switch (zipResult.MatchKind)
            {
                case MatchKind.ExactMatch:
                    AddValue(value);
                    break;

                case MatchKind.IsContained:
                    GetOrCreateChild(zipResult.OtherRest, value);
                    break;

                case MatchKind.Contains:
                    SplitOne(zipResult, value);
                    break;

                case MatchKind.Partial:
                    SplitTwo(zipResult, value);
                    break;
            }
        }


        private void SplitOne(ZipResult zipResult, TValue value)
        {
            var leftChild = new PatriciaTrieNode<TValue>(zipResult.ThisRest, m_Values, m_Children);

            m_Children = new Dictionary<char, PatriciaTrieNode<TValue>>();
            m_Values = new Queue<TValue>();
            AddValue(value);
            m_Key = zipResult.CommonHead;

            m_Children.Add(zipResult.ThisRest[0], leftChild);
        }

        private void SplitTwo(ZipResult zipResult, TValue value)
        {
            var leftChild = new PatriciaTrieNode<TValue>(zipResult.ThisRest, m_Values, m_Children);
            var rightChild = new PatriciaTrieNode<TValue>(zipResult.OtherRest, value);

            m_Children = new Dictionary<char, PatriciaTrieNode<TValue>>();
            m_Values = new Queue<TValue>();
            m_Key = zipResult.CommonHead;

            char leftKey = zipResult.ThisRest[0];
            m_Children.Add(leftKey, leftChild);
            char rightKey = zipResult.OtherRest[0];
            m_Children.Add(rightKey, rightChild);
        }

        protected void GetOrCreateChild(StringPartition key, TValue value)
        {
            PatriciaTrieNode<TValue> child;
            if (!m_Children.TryGetValue(key[0], out child))
            {
                child = new PatriciaTrieNode<TValue>(key, value);
                m_Children.Add(key[0], child);
            }
            else
            {
                child.Add(key, value);
            }
        }

        protected override TrieNodeBase<TValue> GetOrCreateChild(char key)
        {
            throw new NotSupportedException("Use alternative signature instead.");
        }

        protected override TrieNodeBase<TValue> GetChildOrNull(string query, int position)
        {
            if (query == null) throw new ArgumentNullException("query");
            PatriciaTrieNode<TValue> child;
            if (m_Children.TryGetValue(query[position], out child))
            {
                var queryPartition = new StringPartition(query, position, child.m_Key.Length);
                if (child.m_Key.StartsWith(queryPartition))
                {
                    return child;
                }
            }
            return null;
        }

        public string Traversal()
        {
            var result = new StringBuilder();
            result.Append(m_Key);

            string subtreeResult = string.Join(" ; ", m_Children.Values.Select(node => node.Traversal()).ToArray());
            if (subtreeResult.Length != 0)
            {
                result.Append("[");
                result.Append(subtreeResult);
                result.Append("]");
            }

            return result.ToString();
        }

        public override string ToString()
        {
            return 
                string.Format(
                    "Key: {0}, Values: {1} Children:{2}, ", 
                    m_Key, 
                    Values().Count(),
                    String.Join(";", m_Children.Keys));
        }
    }
}