// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gma.DataStructures.StringSearch.Test.TestCaseGeneration
{
    public static class NonsenseGeneration
    {
        public const int DefaultAverageWordCount = 15;
        public const string VocabularyFileName = "TestCaseGeneration//english-words.txt";

        public static string[] GetVocabulary()
        {
            return GetWords(VocabularyFileName).ToArray();
        }

        public static IEnumerable<IEnumerable<string>> GetRandomSentences(string[] vocabulary,
                                                                          int sentenceCount,
                                                                          int averageWordCount = DefaultAverageWordCount)
        {
            var random = new Random();
            int wordCount = random.Next(2 * averageWordCount) + 1;
            for (int i = 0; i < sentenceCount; i++)
            {
                yield return GetRandomWords(vocabulary, wordCount, random);
            }
        }

        public static IEnumerable<string> GetRandomWords(string[] vocabulary, int wordCount)
        {
            return GetRandomWords(vocabulary, wordCount, new Random());
        }

        public static IEnumerable<string> GetRandomNeighbourWordGroups(string[] vocabulary, int wordCount)
        {
            var words = new Stack<string>();
            Random random = new Random();
            while (words.Count<wordCount)
            {
                foreach (var neighbour in GetRandomNeighbourWords(vocabulary, random, 3))
                {
                    words.Push(neighbour);
                }
            }
            return words.Take(wordCount);
        }

        public static IEnumerable<string> GetRandomWords(string[] vocabulary, int wordCount, Random random)
        {
            for (int i = 0; i < wordCount; i++)
            {
                var randomWord = GetRandomWord(vocabulary, random);
                if (string.IsNullOrEmpty(randomWord)) continue;
                yield return randomWord;
            }
        }

        private static string GetRandomWord(string[] vocabulary, Random random)
        {
            int randomIndex = random.Next(vocabulary.Length);
            return vocabulary[randomIndex];
        }

        private static IEnumerable<string> GetRandomNeighbourWords(string[] vocabulary, Random random, int neighbourCount)
        {
            int randomIndex = random.Next(vocabulary.Length);
            for (int i = 0; i < neighbourCount; i++)
            {
                yield return vocabulary[randomIndex];
                randomIndex = randomIndex + random.Next(5);
                if (randomIndex>=vocabulary.Length)
                {
                    yield break;
                }
            }
            
        }

        public static IEnumerable<string> GetWords(string fileName)
        {
            using (StreamReader file = File.OpenText(fileName))
            {
                var word = new StringBuilder();
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine() ?? String.Empty;
                    foreach (char ch in line)
                    {
                        if (Char.IsLetterOrDigit(ch))
                        {
                            word.Append(ch);
                        }
                        else
                        {
                            yield return word.ToString();
                            word.Clear();
                        }
                    }
                    if (word.Length == 0) continue;
                    yield return word.ToString();
                    word.Clear();
                }
            }
        }
    }
}