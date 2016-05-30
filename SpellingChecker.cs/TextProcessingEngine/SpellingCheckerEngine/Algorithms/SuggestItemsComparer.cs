using System;
using System.Collections.Generic;

namespace ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.SpellingCheckerEngine.Algorithms
{
    public class SuggestItemsComparer : IEqualityComparer<SuggestItem>
    {
        public bool Equals(SuggestItem x, SuggestItem y)
        {
            if (x.Term == y.Term)
                return true;
            return false;
        }

        public int GetHashCode(SuggestItem obj)
        {
            throw new NotImplementedException();
        }
    }
}
