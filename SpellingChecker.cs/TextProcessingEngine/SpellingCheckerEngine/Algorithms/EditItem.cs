using System;

namespace ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.SpellingCheckerEngine.Algorithms
{
    [Serializable]
    public class EditItem
    {
        public string Term = "";
        public int Distance = 0;
        
        public override bool Equals(object obj)
        {
            return Equals(Term, ((EditItem)obj).Term);
        }

        public override int GetHashCode()
        {
            return Term.GetHashCode();
        }
    }
}
