// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System;
using System.Collections.Generic;

namespace Gma.DataStructures.StringSearch
{
    /// <summary>
    /// Interface to be implemented by a data structure 
    /// which allows adding values <see cref="TValue"/> associated with <b>string</b> keys.
    /// The interface allows retrieval of multiple values along with their positions.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface ISuffixTrie<TValue> : ITrie<TValue>
    {
        IEnumerable<WordPosition<TValue>> RetrieveSubstrings(string query);
    }
}