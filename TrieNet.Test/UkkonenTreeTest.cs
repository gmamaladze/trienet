using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Gma.DataStructures.StringSearch.Test
{
    [TestFixture]
    public class UkkonenTreeTest : SuffixTrieTest
    {
        protected override ISuffixTrie<int> CreateTrie()
        {
            return new CharUkkonenTrie<int>(0);
        }
    }
}