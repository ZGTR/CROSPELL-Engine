﻿//// CyberSpell: 1000x faster through Symmetric Delete spelling correction algorithm
////
//// The Symmetric Delete spelling correction algorithm reduces the complexity of edit candidate generation and dictionary lookup
//// for a given Damerau-Levenshtein distance. It is three orders of magnitude faster and language independent.
//// Opposite to other algorithms only deletes are required, no transposes + replaces + inserts.
//// Transposes + replaces + inserts of the input term are transformed into deletes of the dictionary term.
//// Replaces and inserts are expensive and language dependent: e.g. Chinese has 70,000 Unicode Han characters!
////
//// Copyright (C) 2012 Wolf Garbe, FAROO Limited
//// Version: 1.6
//// Author: Wolf Garbe <wolf.garbe@faroo.com>
//// Maintainer: Wolf Garbe <wolf.garbe@faroo.com>
//// URL: http://blog.faroo.com/2012/06/07/improved-edit-distance-based-spelling-correction/
//// Description: http://blog.faroo.com/2012/06/07/improved-edit-distance-based-spelling-correction/
////
//// License:
//// This program is free software; you can redistribute it and/or modify
//// it under the terms of the GNU Lesser General Public License,
//// version 3.0 (LGPL-3.0) as published by the Free Software Foundation.
//// http://www.opensource.org/licenses/LGPL-3.0
////
//// Usage: single word + Enter:  Display spelling suggestions
////        Enter without input:  Terminate the program

//using System;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Collections.Generic;
//using System.IO;
//using System.Diagnostics;
//using SpellingChecker.SpellingCheckerEngine;

//static class CyberSpells
//{
//    private static int editDistanceMax = 2;
//    private static int verbose = 1;
//    //0: top suggestion
//    //1: all suggestions of smallest edit distance
//    //2: all suggestions <= editDistanceMax (slower, no early termination)

//    private class DictionaryItem
//    {
//        public string term = "";
//        public List<EditItem> suggestions = new List<EditItem>();
//        public int count = 0;

//        public override bool Equals(object obj)
//        {
//            return Equals(term, ((DictionaryItem)obj).term);
//        }

//        public override int GetHashCode()
//        {
//            return term.GetHashCode();
//        }
//    }

//    private class EditItem
//    {
//        public string Term = "";
//        public int Distance = 0;

//        public override bool Equals(object obj)
//        {
//            return Equals(Term, ((EditItem)obj).Term);
//        }

//        public override int GetHashCode()
//        {
//            return Term.GetHashCode();
//        }
//    }

//    private class SuggestItem
//    {
//        public string Term = "";
//        public int Distance = 0;
//        public int Count = 0;

//        public override bool Equals(object obj)
//        {
//            return Equals(Term, ((SuggestItem)obj).Term);
//        }

//        public override int GetHashCode()
//        {
//            return Term.GetHashCode();
//        }
//    }

//    private static Dictionary<string, DictionaryItem> dictionary = new Dictionary<string, DictionaryItem>();

//    //create a non-unique wordlist from sample text
//    //language independent (e.g. works with Chinese characters)
//    private static IEnumerable<string> parseWords(string text)
//    {
//        return Regex.Matches(text.ToLower(), @"[\w-[\d_]]+")
//                    .Cast<Match>()
//                    .Select(m => m.Value);
//    }

//    //for every word there all deletes with an edit distance of 1..editDistanceMax created and added to the dictionary
//    //every delete entry has a suggestions list, which points to the original term(s) it was created from
//    //The dictionary may be dynamically updated (word frequency and new words) at any time by calling createDictionaryEntry
//    private static bool CreateDictionaryEntry(string key, string language)
//    {
//        bool result = false;
//        DictionaryItem value;
//        if (dictionary.TryGetValue(language + key, out value))
//        {
//            //already exists:
//            //1. word appears several times
//            //2. word1==deletes(word2)
//            value.count++;
//        }
//        else
//        {
//            value = new DictionaryItem();
//            value.count++;
//            dictionary.Add(language + key, value);
//        }

//        //edits/suggestions are created only once, no matter how often word occurs
//        //edits/suggestions are created only as soon as the word occurs in the corpus,
//        //even if the same term existed before in the dictionary as an edit from another word
//        if (string.IsNullOrEmpty(value.term))
//        {
//            result = true;
//            value.term = key;

//            //create deletes
//            foreach (EditItem delete in Edits(key, 0, true))
//            {
//                EditItem suggestion = new EditItem();
//                suggestion.Term = key;
//                suggestion.Distance = delete.Distance;

//                DictionaryItem value2;
//                if (dictionary.TryGetValue(language + delete.Term, out value2))
//                {
//                    //already exists:
//                    //1. word1==deletes(word2)
//                    //2. deletes(word1)==deletes(word2)
//                    if (!value2.suggestions.Contains(suggestion)) AddLowestDistance(value2.suggestions, suggestion);
//                }
//                else
//                {
//                    value2 = new DictionaryItem();
//                    value2.suggestions.Add(suggestion);
//                    dictionary.Add(language + delete.Term, value2);
//                }
//            }
//        }
//        return result;
//    }

//    //create a frequency disctionary from a corpus
//    private static void CreateDictionary(string corpus, string language)
//    {
//        if (!File.Exists(corpus))
//        {
//            Console.Error.WriteLine("File not found: " + corpus);
//            return;
//        }

//        Console.Write("Creating dictionary ...");
//        long wordCount = 0;
//        foreach (string key in parseWords(File.ReadAllText( corpus)))
//        {
//            if (CreateDictionaryEntry(key, language)) wordCount++;
//        }
//        Console.WriteLine("\rDictionary created: " + wordCount.ToString("N0") + " words, " + dictionary.Count.ToString("N0") + " entries, for edit distance=" + editDistanceMax.ToString());
//    }

//    //save some time and space
//    private static void AddLowestDistance(List<EditItem> suggestions, EditItem suggestion)
//    {
//        //remove all existing suggestions of higher distance, if verbose<2
//        if ((verbose < 2) && (suggestions.Count > 0) && (suggestions[0].Distance > suggestion.Distance)) suggestions.Clear();
//        //do not add suggestion of higher distance than existing, if verbose<2
//        if ((verbose == 2) || (suggestions.Count == 0) || (suggestions[0].Distance >= suggestion.Distance)) suggestions.Add(suggestion);
//    }

//    //inexpensive and language independent: only deletes, no transposes + replaces + inserts
//    //replaces and inserts are expensive and language dependent (Chinese has 70,000 Unicode Han characters)
//    private static List<EditItem> Edits(string word, int editDistance, bool recursion)
//    {
//        editDistance++;
//        List<EditItem> deletes = new List<EditItem>();
//        if (word.Length > 1)
//        {
//            for (int i = 0; i < word.Length; i++)
//            {
//                EditItem delete = new EditItem();
//                delete.Term = word.Remove(i, 1);
//                delete.Distance = editDistance;
//                if (!deletes.Contains(delete))
//                {
//                    deletes.Add(delete);
//                    //recursion, if maximum edit distance not yet reached
//                    if (recursion && (editDistance < editDistanceMax))
//                    {
//                        foreach (EditItem edit1 in Edits(delete.Term, editDistance, recursion))
//                        {
//                            if (!deletes.Contains(edit1)) deletes.Add(edit1);
//                        }
//                    }
//                }
//            }
//        }

//        return deletes;
//    }

//    private static int TrueDistance(EditItem dictionaryOriginal, EditItem inputDelete, string inputOriginal)
//    {
//        //We allow simultaneous edits (deletes) of editDistanceMax on on both the dictionary and the input term.
//        //For replaces and adjacent transposes the resulting edit distance stays <= editDistanceMax.
//        //For inserts and deletes the resulting edit distance might exceed editDistanceMax.
//        //To prevent suggestions of a higher edit distance, we need to calculate the resulting edit distance, if there are simultaneous edits on both sides.
//        //Example: (bank==bnak and bank==bink, but bank!=kanb and bank!=xban and bank!=baxn for editDistanceMaxe=1)
//        //Two deletes on each side of a pair makes them all equal, but the first two pairs have edit distance=1, the others edit distance=2.

//        if (dictionaryOriginal.Term == inputOriginal) return 0;
//        else
//            if (dictionaryOriginal.Distance == 0) return inputDelete.Distance;
//            else if (inputDelete.Distance == 0) return dictionaryOriginal.Distance;
//            else return DamerauLevenshteinDistance(dictionaryOriginal.Term, inputOriginal);//adjust distance, if both distances>0
//    }

//    private static List<SuggestItem> Lookup(string input, string language, int editDistanceMax)
//    {
//        List<EditItem> candidates = new List<EditItem>();

//        //add original term
//        EditItem item = new EditItem();
//        item.Term = input;
//        item.Distance = 0;
//        candidates.Add(item);

//        List<SuggestItem> suggestions = new List<SuggestItem>();
//        DictionaryItem value;

//        while (candidates.Count > 0)
//        {
//            EditItem candidate = candidates[0];
//            candidates.RemoveAt(0);

//            //save some time
//            //early termination
//            //suggestion distance=candidate.distance... candidate.distance+editDistanceMax
//            //if canddate distance is already higher than suggestion distance, than there are no better suggestions to be expected
//            if ((verbose < 2) && (suggestions.Count > 0) && (candidate.Distance > suggestions[0].Distance)) goto sort;
//            if (candidate.Distance > editDistanceMax) goto sort;

//            if (dictionary.TryGetValue(language + candidate.Term, out value))
//            {
//                if (!string.IsNullOrEmpty(value.term))
//                {
//                    //correct term
//                    SuggestItem si = new SuggestItem();
//                    si.Term = value.term;
//                    si.Count = value.count;
//                    si.Distance = candidate.Distance;

//                    if (!suggestions.Contains(si))
//                    {
//                        suggestions.Add(si);
//                        //early termination
//                        if ((verbose < 2) && (candidate.Distance == 0)) goto sort;
//                    }
//                }
                
//                //edit term (with suggestions to correct term)
//                DictionaryItem value2;
//                foreach (EditItem suggestion in value.suggestions)
//                {
//                    //save some time
//                    //skipping double items early
//                    if (suggestions.Find(x => x.Term == suggestion.Term) == null)
//                    {
//                        int distance = TrueDistance(suggestion, candidate, input);

//                        //save some time.
//                        //remove all existing suggestions of higher distance, if verbose<2
//                        if ((verbose < 2) && (suggestions.Count > 0) && (suggestions[0].Distance > distance)) suggestions.Clear();
//                        //do not process higher distances than those already found, if verbose<2
//                        if ((verbose < 2) && (suggestions.Count > 0) && (distance > suggestions[0].Distance)) continue;

//                        if (distance <= editDistanceMax)
//                        {
//                            if (dictionary.TryGetValue(language + suggestion.Term, out value2))
//                            {
//                                SuggestItem si = new SuggestItem();
//                                si.Term = value2.term;
//                                si.Count = value2.count;
//                                si.Distance = distance;

//                                suggestions.Add(si);
//                            }
//                        }
//                    }
//                }
//            }//end foreach

//            //add edits
//            if (candidate.Distance < editDistanceMax)
//            {
//                foreach (EditItem delete in Edits(candidate.Term, candidate.Distance, false))
//                {
//                    if (!candidates.Contains(delete)) candidates.Add(delete);
//                }
//            }
//        }//end while

//        sort: suggestions = suggestions.OrderBy(c => c.Distance).ThenByDescending(c => c.Count).ToList();
//        if ((verbose == 0) && (suggestions.Count > 1)) return suggestions.GetRange(0, 1); else return suggestions;
//    }

//    private static void Correct(string input, string language)
//    {
//        List<SuggestItem> suggestions = null;

//        /*
//        //Benchmark: 1000 x Lookup
//        Stopwatch stopWatch = new Stopwatch();
//        stopWatch.Start();
//        for (int i = 0; i < 1000; i++)
//        {
//            suggestions = Lookup(input,language,editDistanceMax);
//        }
//        stopWatch.Stop();
//        Console.WriteLine(stopWatch.ElapsedMilliseconds.ToString());
//        */

//        //check in dictionary for existence and frequency; sort by edit distance, then by word frequency
//        suggestions = Lookup(input, language, editDistanceMax);

//        //display term and frequency
//        foreach (var suggestion in suggestions)
//        {
//            Console.WriteLine(suggestion.Term + " " + suggestion.Distance.ToString() + " " + suggestion.Count.ToString());
//        }
//        if (verbose == 2) Console.WriteLine(suggestions.Count.ToString() + " suggestions");
//    }

//    private static void ReadFromStdIn()
//    {
//        string word;
//        while (!string.IsNullOrEmpty(word = (Console.ReadLine() ?? "").Trim()))
//        {
//            Correct(word, languageString);
//        }
//    }

//    public static void GetCorrectSpellSuggestions()
//    {
//        //e.g. http://norvig.com/big.txt , or any other large text corpus
//        CreateDictionary("big.txt", languageString);
//        ReadFromStdIn();
//    }

//    // Damerau–Levenshtein distance algorithm and code
//    public static Int32 DamerauLevenshteinDistance(String source, String target)
//    {
//        MEDAlgo algo = new MEDAlgo();
//        return algo.GetMED(source.ToList().Select(t => t.ToString()).ToList(),
//                    target.ToList().Select(t => t.ToString()).ToList());

//        //
//        Int32 m = source.Length;
//        Int32 n = target.Length;
//        Int32[,] H = new Int32[m + 2, n + 2];

//        Int32 INF = m + n;
//        H[0, 0] = INF;
//        for (Int32 i = 0; i <= m; i++) { H[i + 1, 1] = i; H[i + 1, 0] = INF; }
//        for (Int32 j = 0; j <= n; j++) { H[1, j + 1] = j; H[0, j + 1] = INF; }

//        SortedDictionary<Char, Int32> sd = new SortedDictionary<Char, Int32>();
//        foreach (Char Letter in (source + target))
//        {
//            if (!sd.ContainsKey(Letter))
//                sd.Add(Letter, 0);
//        }

//        for (Int32 i = 1; i <= m; i++)
//        {
//            Int32 DB = 0;
//            for (Int32 j = 1; j <= n; j++)
//            {
//                Int32 i1 = sd[target[j - 1]];
//                Int32 j1 = DB;

//                if (source[i - 1] == target[j - 1])
//                {
//                    H[i + 1, j + 1] = H[i, j];
//                    DB = j;
//                }
//                else
//                {
//                    H[i + 1, j + 1] = Math.Min(H[i, j], Math.Min(H[i + 1, j], H[i, j + 1])) + 1;
//                }

//                H[i + 1, j + 1] = Math.Min(H[i + 1, j + 1], H[i1, j1] + (i - i1 - 1) + 1 + (j - j1 - 1));
//            }

//            sd[source[i - 1]] = i;
//        }
//        return H[m + 1, n + 1];
//    }
//}