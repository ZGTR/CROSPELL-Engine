using System;
using System.Linq;
using System.Collections.Generic;
using SpellingChecker.Keyboard;
using ZGTR_CROSPELLSpellingCheckerLib.SpellingCheckerEngine.Algorithms;
using ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.ISRI;
using ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.Porter;

namespace ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.SpellingCheckerEngine.Algorithms
{
    [Serializable]
    public class CyberSpellMod : CyberSpell
    {
        private Porter.PorterStemmer _porterStemmer;
        private ISRI.ISRIAlgo _isriAlgo;
        public CyberSpellMod(string corpusPath, string languageId, KeyboardLanguage keyboardlang, bool isFast)
            :base (corpusPath, languageId, keyboardlang, isFast)
        {
            _porterStemmer = new PorterStemmer();
            _isriAlgo = new ISRIAlgo();
        }

        public override bool CreateDictionaryEntry(string key, string language)
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
                value.Stem = GetStem(key);
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

        private string GetStem(string key)
        {
            if (this.KeyboardLang == KeyboardLanguage.Arabic)
            {
                return this._isriAlgo.Stem(key);
            }
            return this._porterStemmer.GetStem(key);
        }

        protected override List<SuggestItem> Lookup(string input, string language, int editDistanceMax)
        {
            List<EditItem> candidates = new List<EditItem>();

            //add original term
            EditItem item = new EditItem();
            item.Term = input;
            item.Distance = 0;
            candidates.Add(item);

            List<SuggestItem> suggestions = new List<SuggestItem>();
            DictionaryItem value;
            // if word is correct
            if (_dictionary.TryGetValue(input, out value))
            {
                if (!string.IsNullOrEmpty(value.Term))
                {
                    //correct term
                    SuggestItem si = new SuggestItem();
                    si.Term = value.Term;
                    si.Count = value.Count;
                    suggestions.Add(si);
                }
            }
            else
            {
                //string wrongStem = 
                
            }

             suggestions = suggestions.OrderBy(c => c.Distance).ThenByDescending(c => c.Count).ToList();
            if ((Verbose == 0) && (suggestions.Count > 1)) return suggestions.GetRange(0, 1); else return suggestions;
        }
    }
}