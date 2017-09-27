using Gma.DataStructures.StringSearch._Ukkonen;
using NUnit.Framework;

namespace Gma.DataStructures.StringSearch.Test
{
    [TestFixture]
    public class UkkonenTreeTest : BaseTrieTest
    {
        protected override ITrie<int> CreateTrie()
        {
            return new UkkonenTree<int>(0);
        }
    }
}