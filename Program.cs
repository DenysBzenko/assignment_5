using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        while (true)
        {
            
            string[] words = File.ReadAllLines("C:\\Users\\User\\T9\\T9A\\T9A\\words_list.txt");

           
            List<string> wordList = new List<string>(words);

            Console.Write("Enter a sentence: ");
            string input = Console.ReadLine();

            
            char[] separators = new char[] { ' ', '.', ',', ';', ':', '!', '?' };
            string[] sentence = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

           
            List<string> unknownWords = new List<string>();
            foreach (string word in sentence)
            {
                if (!wordList.Contains(word.ToLower()))
                {
                    List<string> fiveOfferedWords = GetFiveOfferedWords(word.ToLower(), wordList);
                    Console.Write($"Did you mean '{word}'? Try these words instead: ");
                    Console.WriteLine(string.Join(", ", fiveOfferedWords));
                    unknownWords.Add(word);
                }
            }

           
            if (unknownWords.Count > 0)
            {
                Console.WriteLine($"Looks like you have typos in {unknownWords.Count} words.");
            }
            else
            {
                Console.WriteLine("All words are correct!");
            }

            Console.WriteLine("Press any key to continue or 'q' to quit.");
            var key = Console.ReadKey();
            if (key.KeyChar == 'q')
            {
                break;
            }
            Console.Clear();
        }
    }
    
    
    private static List<string> GetFiveOfferedWords(string wrongWord, List<string> glossary)
    {
        var suggestedWords = new List<string>();
        int minDistance = int.MaxValue;
        foreach (var correctWord in glossary)
        {
            int distance = LevenshteinDistance(wrongWord, correctWord);
            if (distance < minDistance)
            {
                suggestedWords.Clear();
                suggestedWords.Add(correctWord);
                minDistance = distance;
            }
            else if (distance == minDistance)
            {
                suggestedWords.Add(correctWord);
            }
        }

        var sortedWords = suggestedWords.OrderBy(w => w);
        return sortedWords.Take(5).ToList();
    }
    
    
  
    private static int LevenshteinDistance(string s1, string s2)
    {
        
        int[,] distances = new int[s1.Length + 1, s2.Length + 1];
        for (int i = 0; i <= s1.Length; i++)
        {
            distances[i, 0] = i;
        }

        for (int j = 0; j <= s2.Length; j++)
        {
            distances[0, j] = j;
        }
       
        for (int i = 1; i <= s1.Length; i++)
        {
            for (int j = 1; j <= s2.Length; j++)
            {
                
                int cost = s1[i - 1] == s2[j - 1] ? 0 : 1;
               
                distances[i, j] = Math.Min(
                    Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                    distances[i - 1, j - 1] + cost);
            }
        }

        return distances[s1.Length, s2.Length];
    }
}

