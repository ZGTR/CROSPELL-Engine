using System;
using System.Collections.Generic;
using System.Drawing;
using SpellingChecker.ImagesProcessingEngine;
using SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos;

namespace ZGTR_CROSPELLSpellingCheckerLib.ImagesProcessingEngine
{
    public class ImageChecker
    {
        private int _boundryWidth = 0;
        private int _thresholdBW = 150;
        private List<ImageUnitWrapper> _dicOfBitmapsOrigin;
        private int _substitutionVal;
        private int _marginError;
        private MEDAlgorithmChosen _algo;
        private int _convAreaWidth;
        private int _boundryHeight;
        private List<Bitmap> _bitmapsOrigin;

        public ImageChecker(List<Bitmap> bitmapsOrigin)
        {
            this._bitmapsOrigin = bitmapsOrigin;
        }

        public List<Bitmap> GetClosestImages(Bitmap chosenBitmap, int substitutionVal, int marginError, int convAreaWidth,
            OrderingByColorOption orderingByColorOption, MEDAlgorithmChosen algo, 
            int boundryWidth, int boundryHeight, int thresholdBW)
        {
            this._substitutionVal = substitutionVal;
            this._marginError = marginError;
            this._algo = algo;
            this._convAreaWidth = convAreaWidth;
            this._thresholdBW = thresholdBW;
            this._boundryWidth = boundryWidth;
            this._boundryHeight = boundryHeight;
            List<int> listOfDinstancesR = null;
            List<int> listOfDinstancesG = null;
            List<int> listOfDinstancesB = null;
            List<Bitmap> listOfOrderedBMByDistR = null;
            List<Bitmap> listOfOrderedBMByDistG = null;
            List<Bitmap> listOfOrderedBMByDistB = null;
            InitializListOfBMDic(ref _dicOfBitmapsOrigin, _bitmapsOrigin);
            switch (orderingByColorOption)
            {
                case OrderingByColorOption.ByRedComp:
                    listOfDinstancesR = FindDistances(chosenBitmap, ColorComponent.R);
                    listOfOrderedBMByDistR = ImageHelperModule.GetOrderedBMByDistance(listOfDinstancesR,
                                                                                          _dicOfBitmapsOrigin);
                    break;
                case OrderingByColorOption.ByGreenComp:
                    listOfDinstancesG = FindDistances(chosenBitmap, ColorComponent.G);
                    listOfOrderedBMByDistG = ImageHelperModule.GetOrderedBMByDistance(listOfDinstancesG,
                                                                                          _dicOfBitmapsOrigin);
                    break;
                case OrderingByColorOption.ByBlueComp:
                    listOfDinstancesB = FindDistances(chosenBitmap, ColorComponent.B);
                    listOfOrderedBMByDistB = ImageHelperModule.GetOrderedBMByDistance(listOfDinstancesB,
                                                                                          _dicOfBitmapsOrigin);
                    break;
                case OrderingByColorOption.ByRedAndBlueComp:
                    listOfDinstancesR = FindDistances(chosenBitmap, ColorComponent.R);
                    listOfDinstancesB = FindDistances(chosenBitmap, ColorComponent.B);
                    listOfOrderedBMByDistR = ImageHelperModule.GetOrderedBMByDistance(listOfDinstancesR,
                                                                                          _dicOfBitmapsOrigin);
                    listOfOrderedBMByDistB = ImageHelperModule.GetOrderedBMByDistance(listOfDinstancesB,
                                                                                          _dicOfBitmapsOrigin);
                    break;
                case OrderingByColorOption.ByRedAndGreenComp:
                    listOfDinstancesR = FindDistances(chosenBitmap, ColorComponent.R);
                    listOfDinstancesG = FindDistances(chosenBitmap, ColorComponent.G);
                    listOfOrderedBMByDistR = ImageHelperModule.GetOrderedBMByDistance(listOfDinstancesR,
                                                                                          _dicOfBitmapsOrigin);
                    listOfOrderedBMByDistG = ImageHelperModule.GetOrderedBMByDistance(listOfDinstancesG,
                                                                                          _dicOfBitmapsOrigin);
                    break;
                case OrderingByColorOption.ByBlueAndGreenComp:
                    listOfDinstancesG = FindDistances(chosenBitmap, ColorComponent.G);
                    listOfDinstancesB = FindDistances(chosenBitmap, ColorComponent.B);
                    listOfOrderedBMByDistG = ImageHelperModule.GetOrderedBMByDistance(listOfDinstancesG,
                                                                                          _dicOfBitmapsOrigin);
                    listOfOrderedBMByDistB = ImageHelperModule.GetOrderedBMByDistance(listOfDinstancesB,
                                                                                          _dicOfBitmapsOrigin);
                    break;
                case OrderingByColorOption.ByRedAndBlueAndGreenComp:
                    listOfDinstancesR = FindDistances(chosenBitmap, ColorComponent.R);
                    listOfDinstancesG = FindDistances(chosenBitmap, ColorComponent.G);
                    listOfDinstancesB = FindDistances(chosenBitmap, ColorComponent.B);
                    listOfOrderedBMByDistR = ImageHelperModule.GetOrderedBMByDistance(listOfDinstancesR,
                                                                                          _dicOfBitmapsOrigin);
                    listOfOrderedBMByDistG = ImageHelperModule.GetOrderedBMByDistance(listOfDinstancesG,
                                                                                          _dicOfBitmapsOrigin);
                    listOfOrderedBMByDistB = ImageHelperModule.GetOrderedBMByDistance(listOfDinstancesB,
                                                                                          _dicOfBitmapsOrigin);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("orderingByColorOption");
            }
            return GetListOfOrderingByColorOption(listOfDinstancesR, listOfDinstancesG, listOfDinstancesB,
                                           listOfOrderedBMByDistR, listOfOrderedBMByDistG, listOfOrderedBMByDistB,
                                           orderingByColorOption);
        }
        

        private List<Bitmap> GetListOfOrderingByColorOption(List<int> listDR, List<int> listDG, List<int> listDB,
                    List<Bitmap> listOR, List<Bitmap> listOG, List<Bitmap> listOB,
                    OrderingByColorOption orderingByColorOption)
        {
            switch (orderingByColorOption)
            {
                case OrderingByColorOption.ByRedComp:
                    return listOR;
                    break;
                case OrderingByColorOption.ByBlueComp:
                    return listOB;
                    break;
                case OrderingByColorOption.ByGreenComp:
                    return listOG;
                    break;
                case OrderingByColorOption.ByRedAndBlueComp:
                    return GetOrderByMultibleLists(new List<int>[] { listDR, listDB });
                    break;
                case OrderingByColorOption.ByRedAndGreenComp:
                    return GetOrderByMultibleLists(new List<int>[] { listDR, listDG });
                    break;
                case OrderingByColorOption.ByBlueAndGreenComp:
                    return GetOrderByMultibleLists(new List<int>[] { listDB, listDG });
                    break;
                case OrderingByColorOption.ByRedAndBlueAndGreenComp:
                    return GetOrderByMultibleLists(new List<int>[] { listDR, listDG, listDB });
                    break;
                default:
                    throw new ArgumentOutOfRangeException("orderingByColorOption");
            }
        }

        private List<Bitmap> GetOrderByMultibleLists(List<int>[] arrOfLists)
        {
            var listOfDinstances = new List<int>();
            for (int i = 0; i < arrOfLists[0].Count; i++)
            {
                // Add entry to list; initialize it to a default value = 0.
                listOfDinstances.Add(0);
                for (int j = 0; j < arrOfLists.Length; j++)
                {
                    listOfDinstances[i] += arrOfLists[j][i];
                }
            }
            return ImageHelperModule.GetOrderedBMByDistance(listOfDinstances,
                                                            _dicOfBitmapsOrigin);
        }

        private void InitializListOfBMDic(ref List<ImageUnitWrapper> dicOfBitmapsOrigin, List<Bitmap> bitmapsOrigin)
        {
            dicOfBitmapsOrigin = new List<ImageUnitWrapper>();
            for (int i = 0; i < bitmapsOrigin.Count; i++)
            {
                dicOfBitmapsOrigin.Add(new ImageUnitWrapper(bitmapsOrigin[i], this._boundryWidth, this._boundryHeight, this._thresholdBW));
            }
        }

        #region AlgorithmsChosen

        private List<int> FindDistances(Bitmap chosenBitmap, ColorComponent colorComp)
        {
            switch (_algo)
            {
                case MEDAlgorithmChosen.MEDRegular:
                    return this.FindDistancesForMedRegularAlgo(chosenBitmap, colorComp);
                    break;
                case MEDAlgorithmChosen.MEDStalkerWithBT:
                    return this.FindDistancesForStalkerWithBTAlgo(chosenBitmap, colorComp);
                    break;
                case MEDAlgorithmChosen.MEDStalkerWithoutBT:
                    return this.FindDistancesForStalkerWithoutBTAlgo(chosenBitmap, colorComp);
                    break;
                case MEDAlgorithmChosen.MEDStalkerWithoutBTEnhanced:
                    return this.FindDistancesForStalkerWithoutBTEnhancedAlgo(chosenBitmap, colorComp);
                case MEDAlgorithmChosen.MEDStalkerWithBTEnhanced:
                    return this.FindDistancesForStalkerWithBTEnhancedAlgo(chosenBitmap, colorComp);
                default:
                    throw new ArgumentOutOfRangeException("algo");
            }
        }

        private List<int> FindDistancesForMedRegularAlgo(Bitmap chosenBitmap, ColorComponent colorComp)
        {
            ImageUnitWrapper chosenBitmapInitWrapper = new ImageUnitWrapper(chosenBitmap, this._boundryWidth, this._boundryHeight,
                                                                            this._thresholdBW);
            MEDRegular mAlgo = InitializeMedRegularForColorComp(colorComp);
            List<int> listOfDinstances = new List<int>();
            var chosenStream = chosenBitmapInitWrapper.GetStreamOfColorComp(colorComp).ToArray();
            foreach (ImageUnitWrapper imageUnit in _dicOfBitmapsOrigin)
            {
                listOfDinstances.Add(mAlgo.GetMED(chosenStream,
                                                  imageUnit.GetStreamOfColorComp(colorComp).ToArray()));
            }
            return listOfDinstances;
        }

        private List<int> FindDistancesForStalkerWithBTAlgo(Bitmap chosenBitmap, ColorComponent colorComp)
        {
            ImageUnitWrapper chosenBitmapInitWrapper = new ImageUnitWrapper(chosenBitmap, this._boundryWidth, this._boundryHeight, this._thresholdBW);
            List<int> listOfDinstances = new List<int>();
            int currentMinimumDistance = 10000;
            MEDStalkerWithBT mAlgo = InitializeStalkerWithBTForColorComp(chosenBitmapInitWrapper, colorComp);
            foreach (ImageUnitWrapper imageUnit in _dicOfBitmapsOrigin)
            {

                listOfDinstances.Add(mAlgo.GetMED(imageUnit.GetStreamOfColorComp(colorComp).ToArray(),
                                                     ref currentMinimumDistance));
            }
            return listOfDinstances;
        }

        private List<int> FindDistancesForStalkerWithoutBTAlgo(Bitmap chosenBitmap, ColorComponent colorComp)
        {
            ImageUnitWrapper chosenBitmapInitWrapper = new ImageUnitWrapper(chosenBitmap, this._boundryWidth, this._boundryHeight,
                                                                            this._thresholdBW);
            List<int> listOfDinstances = new List<int>();
            int currentMinimumDistance = 10000;
            MEDStalkerWithoutBT mAlgo = InitializeStalkerWithoutBTForColorComp(chosenBitmapInitWrapper, colorComp);
            foreach (ImageUnitWrapper imageUnit in _dicOfBitmapsOrigin)
            {
                currentMinimumDistance = mAlgo.GetMED(imageUnit.GetStreamOfColorComp(colorComp).ToArray(),
                                                      ref currentMinimumDistance);
                listOfDinstances.Add(currentMinimumDistance);
            }
            return listOfDinstances;
        }

        private List<int> FindDistancesForStalkerWithoutBTEnhancedAlgo(Bitmap chosenBitmap, ColorComponent colorComp)
        {
            ImageUnitWrapper chosenBitmapInitWrapper = new ImageUnitWrapper(chosenBitmap, this._boundryWidth, this._boundryHeight,
                                                                            this._thresholdBW);
            List<int> listOfDinstances = new List<int>();
            int currentMinimumDistance = 10000;
            MEDStalkerWithoutBTEnhanced mAlgo =
                InitializeStalkerWithoutBTEnhancedForColorComp(chosenBitmapInitWrapper, colorComp);
            foreach (ImageUnitWrapper imageUnit in _dicOfBitmapsOrigin)
            {
                currentMinimumDistance = mAlgo.GetMED(imageUnit.GetStreamOfColorComp(colorComp).ToArray(),
                                                      ref currentMinimumDistance);
                listOfDinstances.Add(currentMinimumDistance);
            }
            return listOfDinstances;
        }

        private List<int> FindDistancesForStalkerWithBTEnhancedAlgo(Bitmap chosenBitmap, ColorComponent colorComp)
        {
            ImageUnitWrapper chosenBitmapInitWrapper = new ImageUnitWrapper(chosenBitmap, this._boundryWidth, this._boundryHeight,
                                                                            this._thresholdBW);
            List<int> listOfDinstances = new List<int>();
            int currentMinimumDistance = 10000;
            foreach (ImageUnitWrapper imageUnit in _dicOfBitmapsOrigin)
            {
                MEDStalkerWithBTEnhanced mAlgo =
                InitializeStalkerWithBTEnhancedForColorComp(chosenBitmapInitWrapper, colorComp);
                currentMinimumDistance = mAlgo.GetMED(imageUnit.GetStreamOfColorComp(colorComp).ToArray(),
                                                      ref currentMinimumDistance);
                listOfDinstances.Add(currentMinimumDistance);
            }
            return listOfDinstances;
        }

        private MEDRegular InitializeMedRegularForColorComp(ColorComponent colorComp)
        {
            switch (colorComp)
            {
                case ColorComponent.R:
                    return new MEDRegular(_substitutionVal);
                    break;
                case ColorComponent.G:
                    return new MEDRegular(_substitutionVal);
                    break;
                case ColorComponent.B:
                    return new MEDRegular(_substitutionVal);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("colorComp");
            }
        }

        private MEDStalkerWithBT InitializeStalkerWithBTForColorComp(ImageUnitWrapper chosenBMUnit, ColorComponent colorComp)
        {
            switch (colorComp)
            {
                case ColorComponent.R:
                    return new MEDStalkerWithBT(chosenBMUnit.BitmapStringStreamR.ToArray(), _substitutionVal,
                                                _marginError, _convAreaWidth);
                    break;
                case ColorComponent.G:
                    return new MEDStalkerWithBT(chosenBMUnit.BitmapStringStreamG.ToArray(), _substitutionVal,
                                                _marginError, _convAreaWidth);
                    break;
                case ColorComponent.B:
                    return new MEDStalkerWithBT(chosenBMUnit.BitmapStringStreamB.ToArray(), _substitutionVal,
                                                _marginError, _convAreaWidth);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("colorComp");
            }
        }

        private MEDStalkerWithoutBT InitializeStalkerWithoutBTForColorComp(ImageUnitWrapper chosenBMUnit, 
            ColorComponent colorComp)
        {
            switch (colorComp)
            {
                case ColorComponent.R:
                    return new MEDStalkerWithoutBT(chosenBMUnit.BitmapStringStreamR.ToArray(), _substitutionVal,
                                                   _marginError, _convAreaWidth);
                    break;
                case ColorComponent.G:
                    return new MEDStalkerWithoutBT(chosenBMUnit.BitmapStringStreamG.ToArray(), _substitutionVal,
                                                   _marginError, _convAreaWidth);
                    break;
                case ColorComponent.B:
                    return new MEDStalkerWithoutBT(chosenBMUnit.BitmapStringStreamB.ToArray(), _substitutionVal,
                                                   _marginError, _convAreaWidth);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("colorComp");
            }
        }

        private MEDStalkerWithBTEnhanced InitializeStalkerWithBTEnhancedForColorComp(ImageUnitWrapper chosenBMUnit,
            ColorComponent colorComp)
        {
            switch (colorComp)
            {
                case ColorComponent.R:
                    return new MEDStalkerWithBTEnhanced(chosenBMUnit.BitmapStringStreamR.ToArray(), _substitutionVal,
                                                           _marginError, _convAreaWidth);
                    break;
                case ColorComponent.G:
                    return new MEDStalkerWithBTEnhanced(chosenBMUnit.BitmapStringStreamG.ToArray(), _substitutionVal,
                                                           _marginError, _convAreaWidth);
                    break;
                case ColorComponent.B:
                    return new MEDStalkerWithBTEnhanced(chosenBMUnit.BitmapStringStreamB.ToArray(), _substitutionVal,
                                                           _marginError, _convAreaWidth);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("colorComp");
            }
        }

        private MEDStalkerWithoutBTEnhanced InitializeStalkerWithoutBTEnhancedForColorComp(ImageUnitWrapper chosenBMUnit, 
            ColorComponent colorComp)
        {
            switch (colorComp)
            {
                case ColorComponent.R:
                    return new MEDStalkerWithoutBTEnhanced(chosenBMUnit.BitmapStringStreamR.ToArray(), _substitutionVal,
                                                           _marginError, _convAreaWidth);
                    break;
                case ColorComponent.G:
                    return new MEDStalkerWithoutBTEnhanced(chosenBMUnit.BitmapStringStreamG.ToArray(), _substitutionVal,
                                                           _marginError, _convAreaWidth);
                    break;
                case ColorComponent.B:
                    return new MEDStalkerWithoutBTEnhanced(chosenBMUnit.BitmapStringStreamB.ToArray(), _substitutionVal,
                                                           _marginError, _convAreaWidth);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("colorComp");
            }
        }
        #endregion
    }
}