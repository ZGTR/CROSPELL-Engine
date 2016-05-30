using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpellingChecker.SpellingCheckerEngine.Algorithm;

namespace SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos.Enums
{
    public class CellWrapper
    {
        public char CSWord { get; set; }
        public int I { get; set; }
        public char CTWord { get; set; }
        public int J { get; set; }
        public int D { get; set; }
        public UnitIntention UnitIntention { set; get; }

        public CellWrapper(char cSWord, int currentI, char cTWord, int currentJ, int d)
        {
            CSWord = cSWord;
            I = currentI;
            CTWord = cTWord;
            J = currentJ;
            D = d;
        }
    }
}
