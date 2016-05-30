using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SpellingChecker.ImagesProcessingEngine;
using SpellingChecker.SpellingCheckerEngine.Algorithm;
using SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos;
using SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos.Enums;
using ZGTR_PorterAlgorithmApp;

namespace ZGTR_CROSPELLSpellingCheckerLib.ImagesProcessingEngine
{
    public class ImagePatchFinder
    {
        private readonly Bitmap _sBM;
        private readonly Bitmap _tBM;

        public ImagePatchFinder(Bitmap sBM, Bitmap tBM)
        {
            _sBM = sBM;
            _tBM = tBM;
        }

        public List<Bitmap> GetPatchedAreaBitmap(ColorComponent colorComponent, int bWidth, int bHeight, int threshold,
            int substitutionVal, int dW, int matchCost, UnitIntention unitIntention)
        {
            MEDSmithWatermanAlgo algo =
                new MEDSmithWatermanAlgo(
                    ImageHelperModule.GetStringStreamFromBitmapCharsList(_sBM, colorComponent, bWidth, bHeight,
                                                                         threshold).ToArray(),
                    ImageHelperModule.GetStringStreamFromBitmapCharsList(_tBM, colorComponent, bWidth, bHeight,
                                                                         threshold).ToArray(), substitutionVal, dW,
                    matchCost);
            algo.GetMED();
            List<List<CellWrapper>> cellsList =  algo.GetBackTraceArray(1000, true);
            List<Bitmap> listOfBitmapsPatched = GetPatchesAreaBM(cellsList, _sBM.Width - bWidth, _sBM.Height - bHeight,
                                                                 unitIntention);
            return listOfBitmapsPatched;
        }

        private List<Bitmap> GetPatchesAreaBM(List<List<CellWrapper>> cellsList, 
            int bWidth, int bHeight, UnitIntention unitIntention)
        {
            List<Bitmap> listOfPatchedBM = new List<Bitmap>();
            for (int i = 0; i < cellsList.Count; i++)
            {
                List<CellWrapper> currentCells = cellsList[i];
                Bitmap currentBM = new Bitmap(bWidth, bHeight);
                FillBMWithPatches(ref currentBM, currentCells, unitIntention);
                listOfPatchedBM.Add(currentBM);
            }
            return listOfPatchedBM;
        }

        private void FillBMWithPatches(ref Bitmap currentBM, List<CellWrapper> cells, UnitIntention unitIntention)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                int[] propIndex = GetProperImageIndex(cells[i], currentBM.Width, currentBM.Height);
                int iProp = propIndex[0];
                int jProp = propIndex[1];
                if (cells[i].UnitIntention == unitIntention)
                {
                    currentBM.SetPixel(iProp, jProp, Color.Blue);
                }
                else
                {
                    currentBM.SetPixel(iProp, jProp, Color.White);
                }
            }
        }

        private int[] GetProperImageIndex(CellWrapper cellWrapper, int width, int height)
        {
            int i = 0;
            int j = 0;
            int[] propIndex = new int[2];

            if (cellWrapper.J == width)
            {
                j = width - 1;
            }
            else
            {
                if (cellWrapper.J > width)
                {
                    i += cellWrapper.J / width;
                    j = cellWrapper.J % width;
                }
                else
                {
                    j = cellWrapper.J;
                }
            }
            //if (cellWrapper.I > height)
            //{
            //    j += cellWrapper.I/height;
            //    i += cellWrapper.I%height;
            //}
            //else
            //{
            //    i += cellWrapper.I;
            //}
            propIndex[0] = j;
            propIndex[1] = i;
            return propIndex;
        }
    }
}
