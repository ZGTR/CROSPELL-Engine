using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using SpellingChecker.Keyboard;
using SpellingChecker.SpellingCheckerEngine.Algorithm;
using SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos;
using ZGTR_CROSPELLSpellingCheckerLib.SpellingCheckerEngine.Algorithms;
using ZGTR_PorterAlgorithmApp;

namespace ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.SpellingCheckerEngine.Algorithms
{
    [Serializable]
    public class CyberSpell
    {
        public int EditDistanceMax { set; get; }
        public int Verbose { set; get; }
        private readonly string _keyboardLanguageString = "";
        protected readonly KeyboardLanguage KeyboardLang;
        //0: top suggestion
        //1: all suggestions of smallest edit distance
        //2: all suggestions <= editDistanceMax (slower, no early termination)

        protected readonly Dictionary<string, DictionaryItem> _dictionary;
        protected bool _isKeyMap;
        
        public CyberSpell(string corpusPath, string languageId, KeyboardLanguage keyboardlang, bool isFast)
        {
            this._dictionary = new Dictionary<string, DictionaryItem>();
            this.EditDistanceMax = 2;
            if (isFast)
                this.Verbose = 1;
            else
                this.Verbose = 2;
            this._keyboardLanguageString = languageId;
            this.KeyboardLang = keyboardlang;
            CreateDictionary(corpusPath, languageId);
        }
        
        public List<SuggestItem> GetCorrectSpellSuggestionsForWord(string wordToCheck, bool isKeyMap)
        {
            this._isKeyMap = isKeyMap;
            return Correct(wordToCheck, _keyboardLanguageString);
        }

        public string GetCorrectSpellSuggestionsForText(string fullText)
        {
            List<string> listOfWordsIn = (fullText).Split(' ').ToList();
            List<string> listOfWordsOut = new List<string>();
            foreach (string word in listOfWordsIn)
            {
                try
                {
                    listOfWordsOut.Add(Correct(word, _keyboardLanguageString)[0].Term);
                }
                catch (Exception)
                {
                    listOfWordsOut.Add(word);
                }
            }
            string stringOut = "";
            listOfWordsOut.ForEach(t => stringOut += " " +  t);
            return stringOut;
        }

        //create a non-unique wordlist from sample text
        //language independent (e.g. works with Chinese characters)
        private static IEnumerable<string> ParseWordsNonArabic(string text)
        {
            return Regex.Matches(text.ToLower(), @"[\w-[\d_]]+")
                .Cast<Match>()
                .Select(m => m.Value);
        }

        private static IEnumerable<string> ParseWordsArabic(string text)
        {
            return text.Split(' ').ToList();
        }

        //create a frequency disctionary from a corpus
        private void CreateDictionary(string corpusFilePath, string language)
        {
            Console.Write("Creating dictionary...");
            long wordCount = 0;
            if (this.KeyboardLang != KeyboardLanguage.Arabic)
            {
                foreach (
                    string key in ParseWordsNonArabic(File.ReadAllText(corpusFilePath, System.Text.Encoding.GetEncoding("Windows-1256")))
                    )
                {
                    if (CreateDictionaryEntry(key, language)) wordCount++;
                }
            }
            else
            {
                string arabFile = String.Empty;
                ReadFileIntoDictionary("ArabicCorpus\\capr", 1604, language, ref wordCount);
                ReadFileIntoDictionary("ArabicCorpus\\caug", 1814, language, ref wordCount);
                ReadFileIntoDictionary("ArabicCorpus\\cdec", 1075, language, ref wordCount);
                ReadFileIntoDictionary("ArabicCorpus\\cfeb", 1857, language, ref wordCount);
                //ReadFileIntoDictionary("ArabicCorpus\\cjan", 2202, language, ref wordCount);
                //ReadFileIntoDictionary("ArabicCorpus\\cjul", 1647, language, ref wordCount);
                //ReadFileIntoDictionary("ArabicCorpus\\cjun", 1636, language, ref wordCount);
                //ReadFileIntoDictionary("ArabicCorpus\\cmar", 1739, language, ref wordCount);
                //ReadFileIntoDictionary("ArabicCorpus\\cmay", 1882, language, ref wordCount);
                //ReadFileIntoDictionary("ArabicCorpus\\cnov", 1220, language, ref wordCount);
                //ReadFileIntoDictionary("ArabicCorpus\\coct", 1446, language, ref wordCount);
                //ReadFileIntoDictionary("ArabicCorpus\\csep", 1604, language, ref wordCount);
            }
            Console.WriteLine("\rDictionary created: " + wordCount.ToString("N0") + " words, " +
                              _dictionary.Count.ToString("N0") + " entries, for edit distance=" +
                              EditDistanceMax.ToString());
        }

        private void ReadFileIntoDictionary(string arabiccorpusCapr, 
            int boundFileNr, 
            string language, 
            ref long wordCount)
        {
            var arabFile = String.Empty;
            for (int i = 1; i < boundFileNr; i++)
            {
                arabFile = arabiccorpusCapr + i.ToString() + ".html";
                foreach (
                    string key in HelperModule.CrackTextToWords(arabFile))
                {
                    if (CreateDictionaryEntry(key, language)) wordCount++;
                }
            }
        }

        //for every word there all deletes with an edit distance of 1..editDistanceMax created and added to the dictionary
        //every delete entry has a suggestions list, which points to the original term(s) it was created from
        //The dictionary may be dynamically updated (word frequency and new words) at any time by calling createDictionaryEntry
        virtual public bool CreateDictionaryEntry(string key, string language)
        {
            bool result = false;
            DictionaryItem value;
            if (_dictionary.TryGetValue(language + key, out value))
            {
                //already exists:
                //1. word appears several times
                //2. word1==deletes(word2)
                value.Count++;
            }
            else
            {
                value = new DictionaryItem();
                value.Count++;
                _dictionary.Add(language + key, value);
            }

            //edits/suggestions are created only once, no matter how often word occurs
            //edits/suggestions are created only as soon as the word occurs in the corpus,
            //even if the same term existed before in the dictionary as an edit from another word
            if (string.IsNullOrEmpty(value.Term))
            {
                result = true;
                value.Term = key;

                //create deletes
                foreach (EditItem delete in Edits(key, 0, true))
                {
                    EditItem suggestion = new EditItem();
                    suggestion.Term = key;
                    suggestion.Distance = delete.Distance;

                    DictionaryItem value2;
                    if (_dictionary.TryGetValue(language + delete.Term, out value2))
                    {
                        //already exists:
                        //1. word1==deletes(word2)
                        //2. deletes(word1)==deletes(word2)
                        if (!value2.Suggestions.Contains(suggestion)) AddLowestDistance(value2.Suggestions, suggestion);
                    }
                    else
                    {
                        value2 = new DictionaryItem();
                        value2.Suggestions.Add(suggestion);
                        _dictionary.Add(language + delete.Term, value2);
                    }
                }
            }
            return result;
        }

        //save some time and space
        protected void AddLowestDistance(List<EditItem> suggestions, EditItem suggestion)
        {
            //remove all existing suggestions of higher distance, if verbose<2
            if ((Verbose < 2) && (suggestions.Count > 0) && (suggestions[0].Distance > suggestion.Distance)) suggestions.Clear();
            //do not add suggestion of higher distance than existing, if verbose<2
            if ((Verbose == 2) || (suggestions.Count == 0) || (suggestions[0].Distance >= suggestion.Distance)) suggestions.Add(suggestion);
        }

        //inexpensive and language independent: only deletes, no transposes + replaces + inserts
        //replaces and inserts are expensive and language dependent (Chinese has 70,000 Unicode Han characters)
        protected List<EditItem> Edits(string word, int editDistance, bool recursion)
        {
            editDistance++;
            List<EditItem> deletes = new List<EditItem>();
            if (word.Length > 1)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    EditItem delete = new EditItem();
                    delete.Term = word.Remove(i, 1);
                    delete.Distance = editDistance;
                    if (!deletes.Contains(delete))
                    {
                        deletes.Add(delete);
                        //recursion, if maximum edit distance not yet reached
                        if (recursion && (editDistance < EditDistanceMax))
                        {
                            foreach (EditItem edit1 in Edits(delete.Term, editDistance, recursion))
                            {
                                if (!deletes.Contains(edit1)) deletes.Add(edit1);
                            }
                        }
                    }
                }
            }

            return deletes;
        }

        protected int TrueDistance(EditItem dictionaryOriginal, EditItem inputDelete, string inputOriginal)
        {
            //We allow simultaneous edits (deletes) of editDistanceMax on on both the dictionary and the input term.
            //For replaces and adjacent transposes the resulting edit distance stays <= editDistanceMax.
            //For inserts and deletes the resulting edit distance might exceed editDistanceMax.
            //To prevent suggestions of a higher edit distance, we need to calculate the resulting edit distance, if there are simultaneous edits on both sides.
            //Example: (bank==bnak and bank==bink, but bank!=kanb and bank!=xban and bank!=baxn for editDistanceMaxe=1)
            //Two deletes on each side of a pair makes them all equal, but the first two pairs have edit distance=1, the others edit distance=2.

            if (dictionaryOriginal.Term == inputOriginal) return 0;
            else
                if (dictionaryOriginal.Distance == 0) return inputDelete.Distance;
                else if (inputDelete.Distance == 0) return dictionaryOriginal.Distance;
                else return DamerauLevenshteinDistance(dictionaryOriginal.Term, inputOriginal, this._isKeyMap);//adjust distance, if both distances>0
        }

        protected virtual List<SuggestItem> Lookup(string input, string language, int editDistanceMax)
        {
            List<EditItem> candidates = new List<EditItem>();

            //add original term
            EditItem item = new EditItem();
            item.Term = input;
            item.Distance = 0;
            candidates.Add(item);

            List<SuggestItem> suggestions = new List<SuggestItem>();
            DictionaryItem value;

            while (candidates.Count > 0)
            {
                EditItem candidate = candidates[0];
                candidates.RemoveAt(0);

                //save some time
                //early termination
                //suggestion distance=candidate.distance... candidate.distance+editDistanceMax
                //if canddate distance is already higher than suggestion distance, than there are no better suggestions to be expected
                if ((Verbose < 2) && (suggestions.Count > 0) && (candidate.Distance > suggestions[0].Distance)) goto sort;
                if (candidate.Distance > editDistanceMax) goto sort;

                if (_dictionary.TryGetValue(language + candidate.Term, out value))
                {
                    if (!string.IsNullOrEmpty(value.Term))
                    {
                        //correct term
                        SuggestItem si = new SuggestItem();
                        si.Term = value.Term;
                        si.Count = value.Count;
                        si.Distance = candidate.Distance;

                        if (!suggestions.Contains(si))
                        {
                            suggestions.Add(si);
                            //early termination
                            if ((Verbose < 2) && (candidate.Distance == 0)) goto sort;
                        }
                    }
                
                    //edit term (with suggestions to correct term)
                    DictionaryItem value2;
                    foreach (EditItem suggestion in value.Suggestions)
                    {
                        //save some time
                        //skipping double items early
                        if (suggestions.Find(x => x.Term == suggestion.Term) == null)
                        {
                            int distance = TrueDistance(suggestion, candidate, input);

                            //save some time.
                            //remove all existing suggestions of higher distance, if verbose<2
                            if ((Verbose < 2) && (suggestions.Count > 0) && (suggestions[0].Distance > distance)) suggestions.Clear();
                            //do not process higher distances than those already found, if verbose<2
                            if ((Verbose < 2) && (suggestions.Count > 0) && (distance > suggestions[0].Distance)) continue;

                            if (distance <= editDistanceMax)
                            {
                                if (_dictionary.TryGetValue(language + suggestion.Term, out value2))
                                {
                                    SuggestItem si = new SuggestItem();
                                    si.Term = value2.Term;
                                    si.Count = value2.Count;
                                    si.Distance = distance;

                                    suggestions.Add(si);
                                }
                            }
                        }
                    }
                }//end foreach

                //add edits
                if (candidate.Distance < editDistanceMax)
                {
                    foreach (EditItem delete in Edits(candidate.Term, candidate.Distance, false))
                    {
                        if (!candidates.Contains(delete)) candidates.Add(delete);
                    }
                }
            }//end while

            sort: suggestions = suggestions.OrderBy(c => c.Distance).ThenByDescending(c => c.Count).ToList();
            if ((Verbose == 0) && (suggestions.Count > 1)) return suggestions.GetRange(0, 1); else return suggestions;
        }

        protected virtual List<SuggestItem> Correct(string input, string language)
        {
            List<SuggestItem> suggestions = null;

            /*
        //Benchmark: 1000 x Lookup
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        for (int i = 0; i < 1000; i++)
        {
            suggestions = Lookup(input,language,editDistanceMax);
        }
        stopWatch.Stop();
        Console.WriteLine(stopWatch.ElapsedMilliseconds.ToString());
        */

            //check in dictionary for existence and frequency; sort by edit distance, then by word frequency
            suggestions = Lookup(input, language, EditDistanceMax);

            //display term and frequency
            foreach (var suggestion in suggestions)
            {
                Console.WriteLine(suggestion.Term + " " + suggestion.Distance.ToString() + " " + suggestion.Count.ToString());
            }
            if (Verbose == 2) Console.WriteLine(suggestions.Count.ToString() + " suggestions");
            return suggestions;
        }

        //private void ReadFromStdIn()
        //{
        //    string word;
        //    while (!string.IsNullOrEmpty(word = (Console.ReadLine() ?? "").Trim()))
        //    {
        //        Correct(word, _keyboardLanguageString);
        //    }
        //}

        // Damerau–Levenshtein distance algorithm and code
        public Int32 DamerauLevenshteinDistance(String source, String target, bool isKeyMap)
        {
            if (isKeyMap)
            {
                MEDRegularWithBTHeuristics algoMED = new MEDRegularWithBTHeuristics(source.ToCharArray(),
                                                                                    target.ToCharArray(), 1,
                                                                                    this.KeyboardLang);
                int dist = algoMED.GetMED();
                return dist;
            }
            else
            {
                MEDRegular algo = new MEDRegular(1);
                return algo.GetMED(source.ToList().Select(Convert.ToChar).ToArray(),
                                   target.ToList().Select(Convert.ToChar).ToArray());
            }

            //
            Int32 m = source.Length;
            Int32 n = target.Length;
            Int32[,] H = new Int32[m + 2, n + 2];

            Int32 INF = m + n;
            H[0, 0] = INF;
            for (Int32 i = 0; i <= m; i++) { H[i + 1, 1] = i; H[i + 1, 0] = INF; }
            for (Int32 j = 0; j <= n; j++) { H[1, j + 1] = j; H[0, j + 1] = INF; }

            SortedDictionary<Char, Int32> sd = new SortedDictionary<Char, Int32>();
            foreach (Char Letter in (source + target))
            {
                if (!sd.ContainsKey(Letter))
                    sd.Add(Letter, 0);
            }

            for (Int32 i = 1; i <= m; i++)
            {
                Int32 DB = 0;
                for (Int32 j = 1; j <= n; j++)
                {
                    Int32 i1 = sd[target[j - 1]];
                    Int32 j1 = DB;

                    if (source[i - 1] == target[j - 1])
                    {
                        H[i + 1, j + 1] = H[i, j];
                        DB = j;
                    }
                    else
                    {
                        H[i + 1, j + 1] = Math.Min(H[i, j], Math.Min(H[i + 1, j], H[i, j + 1])) + 1;
                    }

                    H[i + 1, j + 1] = Math.Min(H[i + 1, j + 1], H[i1, j1] + (i - i1 - 1) + 1 + (j - j1 - 1));
                }

                sd[source[i - 1]] = i;
            }
            return H[m + 1, n + 1];
        }
    }
}