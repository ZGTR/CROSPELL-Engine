using System;
using SpellingChecker.SpellingCheckerEngine.Algorithm;
using ZGTR_PorterAlgorithmApp;

namespace SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos
{
    public class MEDStalkerWithBTEnhanced : MEDStalkerWithBT
    {
        public MEDStalkerWithBTEnhanced(char[] tWord, int substitutionVal, int marginOfError, int areawidthOfConversion)
            : base(tWord, substitutionVal, marginOfError, areawidthOfConversion)
        {

        }

        public override int GetMED(char[] sWord, ref int currentMinimumDistance)
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
                    if (Math.Abs(i - j) > AreawidthOfConversion)
                    {
                        continue;
                    }
                    int cost = GetElementCost(_tWord[i - 1], _sWord[j - 1]);
                    int min = GetMinAndUpdateBT(j, i, _d[j, i - 1] + 1,
                                     _d[j - 1, i] + 1,
                                     _d[j - 1, i - 1] + cost);
                    _d[j, i] = min;
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