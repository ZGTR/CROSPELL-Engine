using System;
using System.Collections.Generic;
using SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos.Enums;
using ZGTR_CROSPELLSpellingCheckerLib.SpellingCheckerEngine.Algorithms.MEDAlgos.BiologicalComp;

namespace SpellingChecker.SpellingCheckerEngine.Algorithm
{
    public class MEDNeedlemanWunschAlgoMod : MEDNeedlemanWunschAlgo
    {
        public MEDNeedlemanWunschAlgoMod(char[] tWord, char[] sWord, int substitutionVal, int dW, int matchCost)
            :base(tWord, sWord, substitutionVal, dW, matchCost)
        {
        }

        protected override int GetMax(int iInsert, int iDel, int iSub)
        {
            return Math.Max(iSub, Math.Max(iInsert, iDel));
        }

        protected override void InitializeFirstRow(int[,] d, int n)
        {
            for (int j = 0; j <= n; j++)
            {
                d[0, j] = 0;
            }
        }

        protected override void InitializeFirstColumn(int[,] d, int m)
        {
            for (int i = 0; i <= m; i++)
            {
                d[i, 0] =  0;
            }
        }

        public override List<List<CellWrapper>> GetBackTraceArray(int upperBound = 1000, bool isProceed = false)
        {
            List<int[]> ijArr = this.GetMaxIJBorder(upperBound);
            return this.GetBackTraceArrayForPtrArray(this.PtrBTArr, ijArr, isProceed);
        }

        protected List<int[]> GetMaxIJBorder(int upperBound)
        {
            List<int[]> arrPair = new List<int[]>();
            int max = -Int32.MaxValue;
            // find max along last column
            for (int i = 0; i < this._d.GetLength(0); i++)
            {
                // cash value
                int currentCellVal = this._d[i, this._d.GetLength(1) -1];
                if (currentCellVal < upperBound)
                    if (max < currentCellVal)
                    {
                        max = currentCellVal;
                    }
            }

            // find max along last Row
            for (int j = 0; j < this._d.GetLength(1); j++)
            {
                // cash value
                int currentCellVal = this._d[this._d.GetLength(0) - 1, j];
                if (currentCellVal < upperBound)
                    if (max < currentCellVal)
                    {
                        max = currentCellVal;
                    }
            }

            // Repass the array and get all the cells equals to the max value
            // find max along last column);
            for (int i = 0; i < this._d.GetLength(0); i++)
            {
                // cash value
                int currentCellVal = this._d[i, this._d.GetLength(1) - 1];
                if (max == currentCellVal)
                {
                    max = currentCellVal;
                    int[] maxPair = new int[2];
                    maxPair[0] = i;
                    maxPair[1] = this._d.GetLength(1) - 1;
                    arrPair.Add(maxPair);
                }
            }

            // find max along last Row
            for (int j = 0; j < this._d.GetLength(1); j++)
            {
                // cash value
                int currentCellVal = this._d[this._d.GetLength(0) - 1, j];
                if (max == currentCellVal)
                {
                    max = currentCellVal;
                    int[] maxPair = new int[2];
                    maxPair[0] = this._d.GetLength(0) - 1;
                    maxPair[1] = j;
                    arrPair.Add(maxPair);
                }
            }
            return arrPair;
        }
    }
}