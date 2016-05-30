using System;
using System.Collections.Generic;
using SpellingChecker.cs;
using SpellingChecker.Keyboard;
using SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos.Enums;
using ZGTR_CROSPELLSpellingCheckerLib.SpellingCheckerEngine.Algorithms.MEDAlgos.MEDBasics;

namespace SpellingChecker.SpellingCheckerEngine.Algorithm
{
    public class MEDRegularWithBTHeuristics : MEDRegularWithBT
    {
        private KeyboardHandler _handler;
        private int _insDelCost;

        public MEDRegularWithBTHeuristics(char[] tWord, char[] sWord, int insDelCost, KeyboardLanguage keyboardlang)
            : base(tWord, sWord, 1)
        {
            _handler = new KeyboardHandler(keyboardlang);
            this._insDelCost = insDelCost;
        }

        public override int GetMED()
        {
            InitializeFirstRow(_d, _nS);
            InitializeFirstColumn(_d, _mT);

            // Scan over
            for (int i = 1; i < _nS + 1; i++)
            {
                for (int j = 1; j < _mT + 1; j++)
                {
                    
                    int cost = GetElementCost(_tWord[i - 1], _sWord[j - 1]);
                    int min = GetMinAndUpdateBT(j, i, _d[j, i - 1] + _insDelCost,
                                     _d[j - 1, i] + _insDelCost,
                                     _d[j - 1, i - 1] + cost);
                    _d[j, i] = min;
                }
            }
            return _d[_mT, _nS];
        }

        protected int GetElementCost(char tWordChar, char sWordChar)
        {
            if (tWordChar.Equals(sWordChar))
            {
                return 0;
            }
            else
            {
                return _handler.GetDistanceBetweenKeys(tWordChar, sWordChar);
            }
        }
    }
}