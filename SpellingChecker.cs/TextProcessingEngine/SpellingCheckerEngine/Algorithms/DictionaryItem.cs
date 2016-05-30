using System;
using System.Collections.Generic;
using ZGTR_CROSPELLSpellingCheckerLib.SpellingCheckerEngine.Algorithms;

namespace ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.SpellingCheckerEngine.Algorithms
{
    [Serializable]
    public class DictionaryItem
    {
        public string Term = "";
        public List<EditItem> Suggestions = new List<EditItem>();
        public int Count = 0;
        public string Stem;

        public override bool Equals(object obj)
        {
            return Equals(Term, ((DictionaryItem)obj).Term);
        }

        public override int GetHashCode()
        {
            return Term.GetHashCode();
        }
    }
}
