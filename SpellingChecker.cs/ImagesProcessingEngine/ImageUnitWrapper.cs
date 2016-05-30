using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpellingChecker.ImagesProcessingEngine
{
    public class ImageUnitWrapper
    {
        public List<char> BitmapStringStreamR { private set; get; }
        public List<char> BitmapStringStreamG { private set; get; }
        public List<char> BitmapStringStreamB { private set; get; }
        public Bitmap Bitmap { private set; get; }

        public ImageUnitWrapper(Bitmap imageBM, int boundryWidth, int boundryHeight, int threshold)
        {
            this.Bitmap = imageBM;
            this.BitmapStringStreamR = ImageHelperModule.GetStringStreamFromBitmapCharsList(this.Bitmap,
                                                                                            ColorComponent.R,
                                                                                            boundryWidth,
                                                                                            boundryHeight, 
                                                                                            threshold);
            this.BitmapStringStreamG = ImageHelperModule.GetStringStreamFromBitmapCharsList(this.Bitmap,
                                                                                            ColorComponent.G,
                                                                                            boundryWidth,
                                                                                            boundryHeight,
                                                                                            threshold);
            this.BitmapStringStreamB = ImageHelperModule.GetStringStreamFromBitmapCharsList(this.Bitmap,
                                                                                            ColorComponent.B,
                                                                                            boundryWidth,
                                                                                            boundryHeight,
                                                                                            threshold);
        }

        public List<char> GetStreamOfColorComp(ColorComponent colorComp)
        {
            switch (colorComp)
            {
                case ColorComponent.R:
                    return this.BitmapStringStreamR;
                    break;
                case ColorComponent.G:
                    return this.BitmapStringStreamG;
                    break;
                case ColorComponent.B:
                    return this.BitmapStringStreamB;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("colorComp");
            }
        }
    }
}
