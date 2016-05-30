using System;
using SpellingChecker.SpellingCheckerEngine.Algorithm;
using ZGTR_PorterAlgorithmApp;

namespace SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos
{
    public class MEDStalkerWithBT : MEDRegularWithBT
    {
        protected int MarginOfError;
        protected int AreawidthOfConversion;
        public MEDStalkerWithBT(char[] tWord, int substitutionVal, int marginOfError, int areawidthOfConversion)
            : base(tWord, null, substitutionVal)
        {
            this.MarginOfError = tWord.Length * marginOfError / 100;
            this.AreawidthOfConversion = areawidthOfConversion;
        }

        public virtual int GetMED(char[] sWord, ref int currentMinimumDistance)
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