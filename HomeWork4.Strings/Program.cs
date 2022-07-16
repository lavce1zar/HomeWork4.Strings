using System;
using System.Text;
using System.Text.RegularExpressions;

namespace HomeWork4.Strings
{
    internal class Program
    {
        public static string PatternForSentences = @"""?[A-Z][^\..]+[.?!]""?";
        public static string PatternForWords = @"([A-Za-z]+['-][A-Za-z]+)|([A-Za-z]+)";
        public static Regex RegexForSentences = new Regex(PatternForSentences);
        public static Regex RegexForWords = new Regex(PatternForWords);
        

        static void Main(string[] args)
        {
            var filePath = "sample.txt";

            var listOfSentences = new List<Sentence>();
            var letters = new Dictionary<char, int>();
            var words = new Dictionary<string, int>();
            var punctuationMarks = new List<char>();

            using (var sr = new StreamReader(filePath, Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();

                    if (line != null && line.Length > 1)
                    {
                        var matches = RegexForSentences.Matches(line);

                        foreach (Match match in matches)
                        {
                            var sentence = new Sentence(match.Value);
                            SplitSentenceToWords(sentence, words);

                            if (sentence.CountOfWords > 1)
                            {
                                listOfSentences.Add(sentence);
                            }
                        }

                        var selectedLetters = from l in line.ToLowerInvariant()
                                              where char.IsLetter(l)
                                              select l;

                        foreach (var ch in selectedLetters)
                        {
                            if (letters.ContainsKey(ch))
                            {
                                letters[ch]++;
                            }
                            else
                            {
                                letters.Add(ch, 1);
                            }
                        }

                        var selectedPunctuationMarks = from l in line
                                              where char.IsPunctuation(l)
                                              select l;

                        foreach (var ch in selectedPunctuationMarks)
                        {
                            punctuationMarks.Add(ch);
                        }
                    }
                }
            }

            Console.WriteLine(listOfSentences.Count);
            Console.WriteLine(words.Count);
            Console.WriteLine(letters.Count);
            Console.WriteLine(punctuationMarks.Count);

            var pathFileForWords = "sortedWords.txt";
            WriteSortedWordsToFile(pathFileForWords, words);

            var pathFileForSomeData = "secondFileWithData.txt";
            WriteSomeDataToFile(pathFileForSomeData, listOfSentences, letters);
        }

        static void SplitSentenceToWords(Sentence sentence, Dictionary<string, int> words)
        {
            var matches = RegexForWords.Matches(sentence.Content);

            sentence.CountOfWords = matches.Count;

            foreach (Match match in matches)
            {
                if (words.ContainsKey(match.Value.ToLowerInvariant()))
                {
                    words[match.Value.ToLowerInvariant()]++;
                }
                else
                {
                    words.Add(match.Value.ToLowerInvariant(), 1);
                }
            }
        }

        static void WriteSortedWordsToFile(string filePath, Dictionary<string, int> words)
        {
            var sortedWords = words.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            using (var sw = new StreamWriter(filePath))
            {
                foreach (var word in sortedWords)
                {
                    sw.WriteLine($"{word.Key} appears {word.Value} times");
                }
            }
        }

        static void WriteSomeDataToFile(string filePath, List<Sentence> sentences, Dictionary<char, int> letters)
        {
            var sortedListOfSentencesBySymbols = sentences.OrderByDescending(x => x.CountOfSymbols).ToList();
            var sortedListOfSentencesByCountOfWords = sentences.OrderBy(x => x.CountOfWords).ToList();
            var firstLetter = letters.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value).First();

            using (var sw = new StreamWriter(filePath))
            {
                sw.WriteLine($"{sortedListOfSentencesBySymbols[0].Content}");
                sw.WriteLine($"{sortedListOfSentencesByCountOfWords[0].Content}");
                sw.WriteLine($"{firstLetter.Key} - {firstLetter.Value}");
            }
            
            Console.WriteLine($"{sortedListOfSentencesBySymbols[0].Content}");
        }
    }
}