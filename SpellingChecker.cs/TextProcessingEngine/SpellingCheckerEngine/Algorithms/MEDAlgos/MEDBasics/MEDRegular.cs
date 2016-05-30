using SpellingChecker.SpellingCheckerEngine.Algorithm;
using ZGTR_CROSPELLSpellingCheckerLib.SpellingCheckerEngine.Algorithms.MEDAlgos.MEDBasics;
using ZGTR_PorterAlgorithmApp;

namespace SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos
{
    public class MEDRegular : MEDBase
    {
        public MEDRegular(int substitutionVal)
            : base(substitutionVal)
        {
        }

        public virtual int GetMED(char[] tWord, char[] sWord)
        {
            int nS = tWord.Length;
            int mT = sWord.Length;
            int[,] d = new int[mT + 1, nS + 1];
            InitializeFirstRow(d, nS);
            InitializeFirstColumn(d, mT);

            // Scan over
            for (int i = 1; i < nS + 1; i++)
            {
                for (int j = 1; j < mT + 1; j++)
                {
                    int cost = GetElementCost(tWord[i - 1], sWord[j - 1]);
                    int min = GetMin(d[j, i - 1] + 1,
                                     d[j - 1, i] + 1,
                                     d[j - 1, i - 1] + cost);
                    d[j, i] = min;
                }
            }
            //HelperModule.WriteDistanceMatrixToFile(d);
            return d[mT, nS];
        }
    }
}