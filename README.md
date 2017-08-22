# Usage
Once code snippet is worth then then 1024 words.
For more usage examples please refer demo app and texts in source code repository.

## Console App
This console application indexes all words in a text file storing line numbers as keys.
It's output looks like this:

![](Documentation_UsageExample_ConsoleOutpu.PNG)

## Code Snippet

{code:c#}
    internal class Program
    {
        private static void Main(string[]() args)
        {
            ITrie<int> trie = new SuffixTrie<int>(3);
            //You can replace it with other trie data structures too 
            //ITrie<int> trie = new Trie<int>();
            //ITrie<int> trie = new PatriciaSuffixTrie<int>(3);

            //Build-up
            BuildUp("sample.txt", trie);

            //Look-up
            LookUp("overs", trie);
            LookUp("porta", trie);
            LookUp("supercalifragilisticexpialidocious", trie);

            Console.WriteLine("-------------Press any key to quit--------------");
            Console.ReadKey();
        }

        private static void BuildUp(string fileName, ITrie<int> trie)
        {
            IEnumerable<WordAndLine> allWordsInFile = GetWordsFromFile(fileName);
            foreach (WordAndLine wordAndLine in allWordsInFile)
            {
                trie.Add(wordAndLine.Word, wordAndLine.Line);
            }
        }

        private static void LookUp(string searchString, ITrie<int> trie)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Look-up for string '{0}'", searchString);
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            int[]() result = trie.Retrieve(searchString).ToArray();
            stopWatch.Stop();

            string matchesText = String.Join(",", result);
            int matchesCount = result.Count();
           
            if (matchesCount == 0)
            {
                Console.WriteLine("No matches found.\tTime: {0}", stopWatch.Elapsed);
            }
            else
            {
                Console.WriteLine(" {0} matches found. \tTime: {1}\tLines: {2}", 
                              matchesCount, 
                              stopWatch.Elapsed,
                              matchesText);
            }
        }


        private static IEnumerable<WordAndLine> GetWordsFromFile(string file)
        {
            using (StreamReader reader = File.OpenText(file))
            {
                Console.WriteLine("Processing file {0} ...", file);
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                int lineNo = 0;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    lineNo ++;
                    IEnumerable<string> words = GetWordsFromLine(line);
                    foreach (string word in words)
                    {
                        yield return new WordAndLine {Line = lineNo, Word = word};
                    }
                }
                stopWatch.Stop();
                Console.WriteLine("Lines:{0}\tTime:{1}", lineNo, stopWatch.Elapsed);
            }
        }

        private static IEnumerable<string> GetWordsFromLine(string line)
        {
            var word = new StringBuilder();
            foreach (char ch in line)
            {
                if (char.IsLetter(ch))
                {
                    word.Append(ch);
                }
                else
                {
                    if (word.Length == 0) continue;
                    yield return word.ToString();
                    word.Clear();
                }
            }
        }

        private struct WordAndLine
        {
            public int Line;
            public string Word;
        }
    }
{code:c#}
