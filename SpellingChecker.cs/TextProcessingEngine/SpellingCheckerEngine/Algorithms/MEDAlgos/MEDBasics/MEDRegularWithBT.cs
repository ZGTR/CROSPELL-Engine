using System;
using System.Collections.Generic;
using SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos.Enums;
using ZGTR_CROSPELLSpellingCheckerLib.SpellingCheckerEngine.Algorithms.MEDAlgos.MEDBasics;
using ZGTR_PorterAlgorithmApp;

namespace SpellingChecker.SpellingCheckerEngine.Algorithm
{
    public class MEDRegularWithBT : MEDBase
    {
        // i: Rows
        // j: Columns

        protected readonly char[] _tWord;
        protected char[] _sWord;
        public BackTracePointer[,] PtrBTArr;
        protected int _nS;
        protected int _mT;
        protected int[,] _d;

        public MEDRegularWithBT(char[] tWord, char[] sWord, int substitutionVal)
            : base(substitutionVal)
        {
            try
            {
                _tWord = tWord;
                _sWord = sWord;
                _nS = tWord.Length;
                _mT = sWord.Length;
                _d = new int[_mT + 1, _nS + 1];
                PtrBTArr = new BackTracePointer[_mT + 1, _nS + 1];
            }
            catch (Exception)
            {
            }
        }

        protected void UpdateBT(int min, int j, int i, int iInsert, int iDel, int iSub)
        {
            if (min == iSub)
            {
                // Substitution
                PtrBTArr[j, i] = BackTracePointer.Diag;
            }
            else
            {
                if (min == iDel)
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
        }

        public virtual int GetMED()
        {
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
                }
            }
            return _d[_mT, _nS];
        }

        public int[,] GetDistanceArray()
        {
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
                }
            }
            return _d;
        }

        protected virtual int GetMinAndUpdateBT(int j, int i, int iInsert, int iDel, int iSub)
        {
            int min = Math.Min(iSub, Math.Min(iInsert, iDel));
            if (min == iSub)
            {
                // Substitution
                PtrBTArr[j, i] = BackTracePointer.Diag;
            }
            else
            {
                if (min == iDel)
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
            return min;
        }

        // i: Rows - target
        // j: Columns - source

        //public UnitIntention[] GetBackTraceArray()
        //{
        //    return MEDRegularWithBT.GetBackTraceArray(this.PtrBTArr);
        //}

        //public static UnitIntention[] GetBackTraceArray(BackTracePointer[,] PtrBTArr)
        //{
        //    List<UnitIntention> btArr = new List<UnitIntention>();
        //    int currentI = PtrBTArr.GetLength(0) - 1, currentJ = PtrBTArr.GetLength(1) - 1;
        //    while (currentI > 0 || currentJ > 0)
        //    {
        //        BackTracePointer currentPtr = PtrBTArr[currentI, currentJ];
        //        //char c1 = _tWord[currentJ - 1];
        //        //if (currentI > 0)
        //        //char c2 = _sWord[currentI - 1];
        //        //int k = _d[currentI, currentJ];
        //        int[] cellDiff = GetCellDifferenceAndUpdateBT(btArr, currentPtr);
        //        currentI += cellDiff[0];
        //        currentJ += cellDiff[1];
        //    }
        //    btArr.Reverse();
        //    return btArr.ToArray();
        //}

        public virtual List<List<CellWrapper>> GetBackTraceArray(int upperBound = 1000)
        {
            int currentI = PtrBTArr.GetLength(0) - 1, currentJ = PtrBTArr.GetLength(1) - 1;
            int[] arr = new int[2];
            arr[0] = currentI;
            arr[1] = currentJ;
            return GetBackTraceArrayForPtrArray(this.PtrBTArr, new List<int[]>() { arr });
        }

        protected List<List<CellWrapper>> GetBackTraceArrayForPtrArray(BackTracePointer[,] PtrBTArr, List<int[]> ijArr)
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