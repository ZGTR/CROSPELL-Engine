using System;
using System.Collections.Generic;
using SpellingChecker.SpellingCheckerEngine.Algorithm;
using SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos.Enums;

namespace SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos
{
    public class MEDSmithWatermanAlgo : MEDNeedlemanWunschAlgoMod
    {
        public MEDSmithWatermanAlgo(char[] tWord, char[] sWord, int substitutionVal, int dW, int matchCost)
            :base(tWord, sWord, substitutionVal, dW, matchCost)
        {
        }

        protected override int GetMax(int iInsert, int iDel, int iSub)
        {
            // in Smith-Waterman we add the max of 0 too
            return Math.Max(0, Math.Max(iSub, Math.Max(iInsert, iDel)));
        }

        public override List<List<CellWrapper>> GetBackTraceArray(int upperBound = 1000, bool isProceed = false)
        {
            List<int[]> ijArr = GetMaxIJInAllArea(upperBound);
            return base.GetBackTraceArrayForPtrArray(base.PtrBTArr, ijArr, isProceed);
        }

        protected List<int[]> GetMaxIJInAllArea(int upperBound)
        {
            List<int[]> arrPair = new List<int[]>();
            int max = - Int32.MaxValue;

            // find max along all the matrix
            for (int i = 0; i < this._d.GetLength(0); i++)
            {
                for (int j = 0; j < this._d.GetLength(1); j++)
                {
                    // cash value
                    int currentCellVal = this._d[i, j];
                    if (currentCellVal < upperBound)
                    {
                        if (currentCellVal > max)
                        {
                            max = currentCellVal;
                        }
                    }
                }
            }

            // Repass the array and get all the cells equals to the max value
            for (int i = 0; i < this._d.GetLength(0); i++)
            {
                for (int j = 0; j < this._d.GetLength(1); j++)
                {
                    // cash value
                    int currentCellVal = this._d[i, j];
                    if (max == currentCellVal)
                    {
                        max = currentCellVal;
                        int[] maxPair = new int[2];
                        maxPair[0] = i;
                        maxPair[1] = j;
                        arrPair.Add(maxPair);
                    }
                }
            }
            return arrPair;
        }
    }
}