// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gma.DataStructures.StringSearch.Test.TestCaseGeneration;
using NUnit.Framework;

namespace Gma.DataStructures.StringSearch.Test
{
    [TestFixture]
    public class TestCaseGenerator
    {
        [TestCase(40, Explicit = true)]
        public void GenerateParallelAddTestCases(int count)
        {
            var vocabulary = NonsenseGeneration.GetVocabulary();
            var phrases = new string[40];
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                var words = NonsenseGeneration.GetRandomWords(vocabulary, 30, random);
                phrases[i] = string.Join(string.Empty, words);

            }
            using (var output = File.CreateText(string.Format("ParallelAddTestCases{0}.txt", count)))
            {
                WriteArrayDeclaration(count.ToString(), phrases, output);
            }

        }


        [TestCase(10, Explicit = true)]
        [TestCase(40, Explicit = true)]
        [TestCase(100, Explicit = true)]
        public void GeneratePrefixSearchTestCases(int count)
        {
            var vocabulary = NonsenseGeneration.GetVocabulary();
            var words = NonsenseGeneration.GetRandomNeighbourWordGroups(vocabulary, count).ToArray();
            var idsAndWords = new KeyValuePair<int, string>[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                idsAndWords[i] = new KeyValuePair<int, string>(i, words[i]);
            }

            using (var output = File.CreateText(string.Format("PrefixSearchTestCases{0}.txt", count)))
            {
                WriteArrayDeclaration(count.ToString(), words, output);

                foreach (var keyValuePair in idsAndWords)
                {
                    var word = keyValuePair.Value;
                    foreach (var query in GetAllPrefixes(word))
                    {
                        string query1 = query;
                        var actual =
                            idsAndWords
                                .Where(idWordPair => idWordPair.Value.StartsWith(query1))
                                .Select(idWordPair => idWordPair.Key);

                        string array = string.Join(",", actual.Select(id => id.ToString()));
                        output.WriteLine("[TestCase(\"{0}\", new[] {{{1}}})]", query, array);
                    }
                }
            }
        }

        private static IEnumerable<string> GetAllPrefixes(string word)
        {
            for (int i = 1; i <= word.Length; i++)
            {
                yield return word.Substring(0, i);
            }
        }


        [TestCase(10, Explicit = true)]
        [TestCase(20, Explicit = true)]
        [TestCase(100, Explicit = true)]
        public void GenerateSuffixSearchTestCases(int count)
        {
            var vocabulary = NonsenseGeneration.GetVocabulary();
            var words = NonsenseGeneration.GetRandomNeighbourWordGroups(vocabulary, count).ToArray();
            var idsAndWords = new KeyValuePair<int, string>[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                idsAndWords[i] = new KeyValuePair<int, string>(i, words[i]);
            }

            using (var output = File.CreateText(string.Format("SuffixSearchTestCases{0}.txt", count)))
            {
                WriteArrayDeclaration(count.ToString(), words, output);

                foreach (var keyValuePair in idsAndWords)
                {
                    var word = keyValuePair.Value;
                    foreach (var query in GetAllSubstrings(word))
                    {
                        string query1 = query;
                        var actual =
                            idsAndWords
                                .Where(idWordPair => idWordPair.Value.Contains(query1))
                                .Select(idWordPair => idWordPair.Key);

                        string array = string.Join(",", actual.Select(id => id.ToString()));
                        output.WriteLine("[TestCase(\"{0}\", new[] {{{1}}})]", query, array);
                    }
                }
            }

        }

        private static IEnumerable<string> GetAllSubstrings(string word)
        {
            for (int i = 0; i < word.Length - 1; i++)
            {
                for (int j = i + 1; j < word.Length; j++)
                {
                    yield return word.Substring(i, j - i);
                }
            }
        }


        private static void WriteArrayDeclaration(string name, string[] words, StreamWriter output)
        {
            output.WriteLine(string.Format("public string[] Words{0} = new[] {{", name));
            for (int index = 0; index < words.Length - 1; index++)
            {
                var word = words[index];
                output.WriteLine(string.Format("\"{0}\",", word));
            }
            output.WriteLine(string.Format("\"{0}\"", words[words.Length - 1]));
            output.WriteLine("}};");
            output.WriteLine();
        }
    }
}
