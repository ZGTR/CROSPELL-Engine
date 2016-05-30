using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpellingChecker.ImagesProcessingEngine
{
    public static class ImageHelperModule
    {
        public static List<String> GetStringStreamFromBitmapStringsList(Bitmap bitmap, 
            ColorComponent colorComponent, int boundryWidth, int boundryHeight,  int threshold)
        {
            return
                GetDoubleVectorFromBitmap(bitmap, colorComponent, boundryWidth, boundryHeight,
                threshold).Select(d => d.ToString()).
                    ToList();
        }

        public static List<char> GetStringStreamFromBitmapCharsList(Bitmap bitmap,
            ColorComponent colorComponent, int boundryWidth, int boundryHeight, int threshold)
        {
            return
                GetDoubleVectorFromBitmap(bitmap, colorComponent, boundryWidth, boundryHeight, threshold).Select(
                    t => Convert.ToChar(t.ToString())).ToList();
        }

        public static double[] GetDoubleVectorFromBitmap(Bitmap bitmap, ColorComponent colorComponent, 
            int boundryWidth,
            int boundryHeight,
            int threshold)
        {
            Bitmap bTest = new Bitmap(bitmap.Width, bitmap.Height);
            List<double> listOfDoubles = new List<double>();
            for (int i = boundryHeight; i < bitmap.Height - boundryHeight; i++)
            {
                for (int j = boundryWidth; j < bitmap.Width - boundryWidth; j++)
                {
                    int rVal = GetPixelOnComponent(bitmap, j, i, colorComponent);
                    if (rVal > threshold)
                    {
                        listOfDoubles.Add(1);
                        bTest.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                    }
                    else
                    {
                        listOfDoubles.Add(0);
                        bTest.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                    }
                }
            }
            //bTest.Save(@"ImagePatchFinder\imOut" + counter + ".jpg");
            //counter++;
            return listOfDoubles.ToArray();
        }
        public static int counter = 0 ;

        private static int GetPixelOnComponent(Bitmap bitmap, int i, int j, ColorComponent colorComponent)
        {
            switch (colorComponent)
            {
                case ColorComponent.R:
                    return bitmap.GetPixel(i, j).R;
                    break;
                case ColorComponent.G:
                    return bitmap.GetPixel(i, j).G;
                    break;
                case ColorComponent.B:
                    return bitmap.GetPixel(i, j).B;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("colorComponent");
            }
        }

        public static List<Bitmap> GetOrderedBMByDistance(List<int> listOfDinstances, List<ImageUnitWrapper> dicOfBitmapsOrigin)
        {
            List<ImageUnitWrapper> dicOfBitmapsOriginManipulated = new List<ImageUnitWrapper>(dicOfBitmapsOrigin);
            List<int> listOfDinstancesManipulated = new List<int>(listOfDinstances);
            List<Bitmap> orderedBMList = new List<Bitmap>();
            while(listOfDinstancesManipulated.Count > 0)
            {
                int currentMin = listOfDinstancesManipulated.Min();
                int indexInList = listOfDinstancesManipulated.IndexOf(currentMin);
                orderedBMList.Add(dicOfBitmapsOriginManipulated[indexInList].Bitmap);
                listOfDinstancesManipulated.RemoveAt(indexInList);
                dicOfBitmapsOriginManipulated.RemoveAt(indexInList); 
            }
            return orderedBMList;
        }
    }
}
