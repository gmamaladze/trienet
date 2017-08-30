// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using Gma.DataStructures.StringSearch.Test.TestCaseGeneration;
using NUnit.Framework;

namespace Gma.DataStructures.StringSearch.Test
{
    [TestFixture]
    [Explicit]
    public class PerformanceCaomparisonTests
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            m_Writer = File.CreateText(s_StatsFileName);
            m_Vocabualry = NonsenseGeneration.GetVocabulary();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            if (m_Writer == null) return;
            m_Writer.Close();
            m_Writer.Dispose();
            m_Writer = null;
        }

        private string[] m_Vocabualry;
        private StreamWriter m_Writer;
        private const string s_StatsFileName = "c:\\temp\\stats.txt";

        private enum TrieType
        {
            List,
            Simple,
            Patricia
        }

        [TestCase("List", 10000, 1000)]
        [TestCase("List", 100000, 1000)]
        [TestCase("List", 1000000, 1000)]
        [TestCase("List", 10000000, 1000)]

        [TestCase("Simple", 10000, 1000)]
        [TestCase("Simple", 100000, 1000)]
        [TestCase("Simple", 1000000, 1000)]
        [TestCase("Simple", 10000000, 1000)]

        [TestCase("Patricia", 10000, 1000)]
        [TestCase("Patricia", 100000, 1000)]
        [TestCase("Patricia", 1000000, 1000)]
        [TestCase("Patricia", 10000000, 1000)]
        public void TestX(string trieTypeName, int wordCount, int lookupCount)
        {
            string[] randomText = NonsenseGeneration.GetRandomWords(m_Vocabualry, wordCount).ToArray();
            string[] lookupWords = NonsenseGeneration.GetRandomWords(m_Vocabualry, lookupCount).ToArray();
            var trie = CreateTrie<string>(trieTypeName);
            TimeSpan buildUp;
            TimeSpan avgLookUp;
            Mesure(trie, randomText, lookupWords, out buildUp, out avgLookUp);
            Console.WriteLine("Build-up time: {0}", buildUp);
            Console.WriteLine("Avg. look-up time: {0}", avgLookUp);
            m_Writer.WriteLine("{0};{1};{2};{3}", trieTypeName, wordCount, buildUp, avgLookUp);
        }

        private ITrie<T> CreateTrie<T>(string trieTypeName)
        {
            var trieType = (TrieType)Enum.Parse(typeof (TrieType), trieTypeName);
            switch (trieType)
            {
                case TrieType.List:
                    return new FakeTrie<T>();
                
                case TrieType.Patricia:
                    return new PatriciaSuffixTrie<T>(3);

                case TrieType.Simple:
                    return new SuffixTrie<T>(3);

                default:
                    throw new NotSupportedException();
            }
        }

        private void Mesure(ITrie<string> trie, IEnumerable<string> randomText, IEnumerable<string> lookupWords,
            out TimeSpan buildUp, out TimeSpan avgLookUp)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (string word in randomText)
            {
                trie.Add(word, word);
            }
            stopwatch.Stop();
            buildUp = stopwatch.Elapsed;


            int lookupCount = 0;
            stopwatch.Reset();
            foreach (string lookupWord in lookupWords)
            {
                lookupCount++;
                stopwatch.Start();
                string[] found = trie.Retrieve(lookupWord).ToArray();
                stopwatch.Stop();
            }
            avgLookUp = new TimeSpan(stopwatch.ElapsedTicks / lookupCount);
        }
    }
}