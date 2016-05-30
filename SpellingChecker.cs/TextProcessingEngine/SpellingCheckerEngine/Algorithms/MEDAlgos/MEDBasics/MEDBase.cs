using System;

namespace ZGTR_CROSPELLSpellingCheckerLib.SpellingCheckerEngine.Algorithms.MEDAlgos.MEDBasics
{
    public abstract class MEDBase
    {
        private int _substitutionVal = 1;

        public MEDBase(int substitutionVal)
        {
            _substitutionVal = substitutionVal;
        }

        protected virtual int GetMin(int iInsert, int iDel, int iSub)
        {
            return Math.Min(iSub, Math.Min(iInsert, iDel));
        }

        protected int GetElementCost(char tWordChar, char sWordChar)
        {
            if (tWordChar.Equals(sWordChar))
            {
                return 0;
            }
            // Substitution
            return _substitutionVal;
        }

        protected void InitializeFirstRow(int[,] d, int n)
        {
            for (int j = 0; j <= n; j++)
            {
                d[0, j] = j;
            }
        }

        protected void InitializeFirstColumn(int[,] d, int m)
        {
            for (int i = 0; i <= m; i++)
            {
                d[i, 0] = i;
            }
        }

        // i, n: Rows - target
        // j, m: Columns - source

        
    }
}