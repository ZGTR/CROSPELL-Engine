using System;
using SpellingChecker.SpellingCheckerEngine.Algorithm;
using ZGTR_PorterAlgorithmApp;

namespace SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos
{
    public class MEDStalkerWithoutBT 
    {
        protected int MarginOfError;
        private int _substitutionVal = 1;
        protected int AreawidthOfConversion;
        protected readonly char[] _tWord;
        protected char[] _sWord;
        public BackTracePointer[,] PtrBTArr;
        protected int _nS;
        protected int _mT;
        protected int[,] _d;

        public MEDStalkerWithoutBT(char[] tWord, int substitutionVal, int marginOfError, int areawidthOfConversion)
        {
            this._substitutionVal = substitutionVal;
            this.MarginOfError = marginOfError;
            this.AreawidthOfConversion = areawidthOfConversion;
            _tWord = tWord;
            _nS = tWord.Length;
            _d = new int[_mT + 1, _nS + 1];
            PtrBTArr = new BackTracePointer[_mT + 1, _nS + 1];
        }

        public int GetMinAndUpdateBT(int j, int i, int iInsert, int iDel, int iSub)
        {
            int min = Math.Min(iSub, Math.Min(iInsert, iDel));
            return min;
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

        protected int GetElementCost(char tWordChar, char sWordChar)
        {
            if (tWordChar.Equals(sWordChar))
            {
                return 0;
            }
            // Substitution
            return _substitutionVal;
        }

        public int GetMED(char[] sWord, ref int currentMinimumDistance)
        {
            this._sWord = sWord;
            _nS = _tWord.Length;
            _mT = _sWord.Length;
            _d = new int[_mT + 1, _nS + 1];
            PtrBTArr = new BackTracePointer[_mT + 1, _nS + 1];
            int minOfThisBM = 0;
            InitializeFirstRow(_d, _nS);
            InitializeFirstColumn(_d, _mT);
            // Scan over
            for (int i = 1; i < _nS + 1; i++)
            {
                for (int j = 1; j < _mT + 1; j++)
                {
                    int cost = GetElementCost(_tWord[i - 1], _sWord[j - 1]);
                    int min = GetMinAndUpdateBT(j, i, _d[j, i - 1] + 1,
                                                _d[j - 1, i] + 1,
                                                _d[j - 1, i - 1] + cost);
                    _d[j, i] = min;

                    //if ((i - j) < AreawidthOfConversion || (j - i) < AreawidthOfConversion)
                    if (Math.Abs(i - j) < AreawidthOfConversion)
                    {
                        if (min > currentMinimumDistance + MarginOfError)
                        {
                            minOfThisBM = min;
                            goto final;
                        }
                    }
                }
            }
            //HelperModule.WriteDistanceMatrixToFile(_d);
            return _d[_mT, _nS];
        final:
            //HelperModule.WriteDistanceMatrixToFile(_d);
            return minOfThisBM;
        }
    }
}