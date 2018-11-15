// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System;
using System.Collections.Generic;
using System.Linq;

namespace Gma.DataStructures.StringSearch
{
    public abstract class TrieNodeBase<TValue>
    {
        protected abstract int KeyLength { get; }

        protected abstract IEnumerable<TValue> Values();

        protected abstract IEnumerable<TrieNodeBase<TValue>> Children();

        public void Add(string key, int position, TValue value)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (EndOfString(position, key))
            {
                AddValue(value);
                return;
            }

            TrieNodeBase<TValue> child = GetOrCreateChild(key[position]);
            child.Add(key, position + 1, value);
        }

        protected abstract void AddValue(TValue value);

        protected abstract TrieNodeBase<TValue> GetOrCreateChild(char key);

        protected virtual IEnumerable<TValue> Retrieve(string query, int position)
        {
            return
                EndOfString(position, query)
                    ? ValuesDeep()
                    : SearchDeep(query, position);
        }

        protected virtual IEnumerable<TValue> SearchDeep(string query, int position)
        {
            TrieNodeBase<TValue> nextNode = GetChildOrNull(query, position);
            return nextNode != null
                       ? nextNode.Retrieve(query, position + nextNode.KeyLength)
                       : Enumerable.Empty<TValue>();
        }

        protected abstract TrieNodeBase<TValue> GetChildOrNull(string query, int position);

        private static bool EndOfString(int position, string text)
        {
            return position >= text.Length;
        }

        private IEnumerable<TValue> ValuesDeep()
        {
            return 
                Subtree()
                    .SelectMany(node => node.Values());
        }

        protected IEnumerable<TrieNodeBase<TValue>> Subtree()
        {
            return
                Enumerable.Repeat(this, 1)
                    .Concat(Children().SelectMany(child => child.Subtree()));
        }

        /// <summary>
        /// Remove the value belongs to a key, if key exists
        /// </summary>
        /// <param name="key"></param>
        /// <param name="position"></param>
        /// <returns>boolean indicaties if the function shall continue with deletion process</returns>
        public bool Remove(string key, int position)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (EndOfString(position, key))
            {
                // check if the key is part of longer key
                if (BelongsToLongerKey(key, position))
                {
                    // if true remove only the value
                    RemoveValue();
                    // prevent the backward removal of the key
                    return false;
                }
                // the key is either a unique key or contains a shorter key
                // return true so the function can continue deleting
                return true;
            }

            // had to us stack approach since no backward approach 

            bool removeRecursively = Remove(key, position + 1);
            // check if a delete signal from the previous call
            if (removeRecursively)
            {
                // discontinue the delete process if a subkey has a value
                return !HasValue(key, position);
            }
            return false;
        }

        /// <summary>
        /// remove the current Node value
        /// </summary>
        protected abstract void RemoveValue();

        /// <summary>
        /// check if the current node belongs to a longer key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        protected abstract bool BelongsToLongerKey(string key, int position);

        /// <summary>
        /// check if the current node has a value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        protected abstract bool HasValue(string key, int position);
    }
}