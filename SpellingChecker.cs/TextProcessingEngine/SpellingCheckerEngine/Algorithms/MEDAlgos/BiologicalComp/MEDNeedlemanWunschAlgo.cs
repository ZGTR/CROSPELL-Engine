using System;
using System.Collections.Generic;
using SpellingChecker.SpellingCheckerEngine.Algorithm;
using SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos.Enums;
using ZGTR_PorterAlgorithmApp;

namespace ZGTR_CROSPELLSpellingCheckerLib.SpellingCheckerEngine.Algorithms.MEDAlgos.BiologicalComp
{
    public class MEDNeedlemanWunschAlgo
    {
        protected int _substitutionVal = 1;
        protected int _matchCost = 1;
        protected int _dW = 1;
        protected readonly char[] _tWord;
        protected readonly char[] _sWord;
        public BackTracePointer[,] PtrBTArr;
        protected int _nS;
        protected int _mT;
        protected int[,] _d;
        

        public MEDNeedlemanWunschAlgo(char[] tWord, char[] sWord, int substitutionVal, int dW, int matchCost)
        {
            this._substitutionVal = substitutionVal;
            this._dW = dW;
            _tWord = tWord;
            _sWord = sWord;
            _nS = tWord.Length;
            _mT = sWord.Length;
            _matchCost = matchCost;
            _d = new int[_mT + 1, _nS + 1];
            PtrBTArr = new BackTracePointer[_mT + 1, _nS + 1];
        }

        public int GetMED()
        {
            //InitializeFirstKeyboardRow(d, nS);
            //InitializeFirstColumn(d, mT);

            // Scan over
            for (int i = 1; i < this._nS + 1; i++)
            {
                for (int j = 1; j < this._mT + 1; j++)
                {
                    int cost = GetElementCost(_tWord[i - 1], _sWord[j - 1]);
                    int max = GetMaxAndUpdateBT(j, i, _d[j, i - 1] - _dW,
                                     _d[j - 1, i] - _dW,
                                     _d[j - 1, i - 1] + cost);
                    _d[j, i] = max;
                }
            }
            //HelperModule.WriteDistanceMatrixToFile(_d);
            return _d[_mT, _nS];
        }

        protected virtual int GetMax(int iInsert, int iDel, int iSub)
        {
            return Math.Max(iSub, Math.Max(iInsert, iDel));
        }

        protected int GetElementCost(char tWordChar, char sWordChar)
        {
            if (tWordChar.Equals(sWordChar))
            {
                return this._matchCost;
            } 
            // Substitution
            return - this._matchCost;
        }

        protected virtual void InitializeFirstRow(int[,] d, int n)
        {
            for (int j = 0; j <= n; j++)
            {
                d[0, j] = - j * _dW;
            }
        }

        protected virtual void InitializeFirstColumn(int[,] d, int m)
        {
            for (int i = 0; i <= m; i++)
            {
                d[i, 0] =  - i * _dW;
            }
        }

        protected int GetMaxAndUpdateBT(int j, int i, int iInsert, int iDel, int iSub)
        {
            int max = GetMax(iInsert, iDel, iSub);
            if (max == iSub)
            {
                // Substitution
                PtrBTArr[j, i] = BackTracePointer.Diag;
            }
            else
            {
                if (max == iDel)
                {
                    // Deletion
                    PtrBTArr[j, i] = BackTracePointer.Down;
                }
                else
                {
                    // Insertion
                    PtrBTArr[j, i] = BackTracePointer.Left;
                }
            }
            return max;
        }

        // i: Rows - target
        // j: Columns - source

        public virtual List<List<CellWrapper>> GetBackTraceArray(int upperBound = 1000, bool isProceed = false)
        {
            int currentI = PtrBTArr.GetLength(0) - 1, currentJ = PtrBTArr.GetLength(1) - 1;
            int[] arr = new int[2];
            arr[0] = currentI;
            arr[1] = currentJ;
            return GetBackTraceArrayForPtrArray(this.PtrBTArr, new List<int[]>() { arr }, isProceed);
        }

        protected List<List<CellWrapper>> GetBackTraceArrayForPtrArray(BackTracePointer[,] PtrBTArr, List<int[]> ijArr, bool isProceed)
        {
            List<List<CellWrapper>> btArr = new List<List<CellWrapper>>();
            foreach (int[] pair in ijArr)
            {
                List<CellWrapper> arr = new List<CellWrapper>();
                int currentI = pair[0], currentJ = pair[1];
                while (currentI > 0 && currentJ > 0)
                {
                    BackTracePointer currentPtr = PtrBTArr[currentI, currentJ];
                    char CTWord = ' ';
                    try
                    {
                        CTWord = _tWord[currentJ - 1];
                    }
                    catch (Exception)
                    {
                        CTWord = _tWord[currentJ];
                    }
                    char CSWord = ' ';
                    try
                    {
                        CSWord = _sWord[currentI - 1];
                    }
                    catch (Exception)
                    {
                        CSWord = _tWord[currentI];
                    }
                    int d = _d[currentI, currentJ];
                    arr.Add(new CellWrapper(CSWord, currentI - 1, CTWord, currentJ - 1, d));
                    int[] cellDiff = GetCellDifferenceAndUpdateBT(arr, currentPtr);
                    currentI += cellDiff[0];
                    currentJ += cellDiff[1];
                }
                if (isProceed)
                {
                    if (currentI <= 0)
                    {
                        while (currentJ > 0)
                        {
                            currentI = 0;
                            BackTracePointer currentPtr = (PtrBTArr[currentI, currentJ]);
                            char CTWord = _tWord[currentJ - 1];
                            char CSWord = _sWord[currentI];
                            int d = _d[currentI, currentJ];
                            arr.Add(new CellWrapper(CSWord, currentI, CTWord, currentJ - 1, d));
                            int[] cellDiff = GetCellDifferenceAndUpdateBT(arr, currentPtr);
                            currentJ -= 1;
                        }
                    }
                    else
                    {
                        if (currentJ <= 0)
                        {
                            while (currentI > 0)
                            {
                                currentJ = 0;
                                BackTracePointer currentPtr = ReversePtr(PtrBTArr[currentI, currentJ]);
                                char CTWord = _tWord[currentJ];
                                char CSWord = _sWord[currentI - 1];
                                int d = _d[currentI, currentJ];
                                arr.Add(new CellWrapper(CSWord, currentI - 1, CTWord, currentJ, d));
                                int[] cellDiff = GetCellDifferenceAndUpdateBT(arr, currentPtr);
                                currentI -= 1;
                            }
                        }
                    }
                }
                arr.Reverse();
                if (arr.Count > 0)
                    btArr.Add(arr);
            }
            return btArr;
        }

        private BackTracePointer ReversePtr(BackTracePointer backTracePointer)
        {
            switch (backTracePointer)
            {
                case BackTracePointer.Left:
                    return BackTracePointer.Down;
                    break;
                case BackTracePointer.Down:
                    return BackTracePointer.Left;
                    break;
                case BackTracePointer.Diag:
                    return BackTracePointer.Diag;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("backTracePointer");
            }
        }

        protected static int[] GetCellDifferenceAndUpdateBT(List<CellWrapper> btArr, BackTracePointer currentPtr)
        {
            int[] diff = new int[2];
            switch (currentPtr)
            {
                case BackTracePointer.Left:
                    diff[0] = 0;
                    diff[1] = -1;
                    btArr[btArr.Count - 1].UnitIntention = (UnitIntention.Insertion);
                    return diff;
                    break;
                case BackTracePointer.Down:
                    diff[0] = -1;
                    diff[1] = 0;
                    btArr[btArr.Count - 1].UnitIntention = (UnitIntention.Deletion);
                    return diff;
                    break;
                case BackTracePointer.Diag:
                    diff[0] = -1;
                    diff[1] = -1;
                    btArr[btArr.Count - 1].UnitIntention = (UnitIntention.Substitution);
                    return diff;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("currentPtr");
            }
        }
    }
}