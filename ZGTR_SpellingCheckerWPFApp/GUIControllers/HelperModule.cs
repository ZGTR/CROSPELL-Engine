using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SpellingChecker.SpellingCheckerEngine.Algorithm;
using Brushes = System.Windows.Media.Brushes;
using FontFamily = System.Windows.Media.FontFamily;
using Image = System.Windows.Controls.Image;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers
{
    class HelperModule
    {
        public static List<String> CrackTextToWords(String filePath)
        {
            List<String> listOut = new List<string>();
            try
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                StringBuilder currentWord = new StringBuilder();
                while (true)
                {
                    int newByte = fileStream.ReadByte();
                    char ch = Char.ToLower((char)newByte);
                    if (Char.IsLetter(ch))
                    {
                        while (true)
                        {
                            currentWord.Append(ch);
                            newByte = fileStream.ReadByte();
                            ch = Char.ToLower((char)newByte);
                            if (!Char.IsLetter(ch))
                            {
                                listOut.Add(currentWord.ToString());
                                currentWord = new StringBuilder();
                                break;
                            }
                        }
                    }
                    if (newByte < 0)
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error while reading the file.");
            }
            return listOut;
        }

        public static List<TextBlock> ColorifyStemsFromOrigins(List<String> originText, List<String> stemmedText)
        {
            List<TextBlock> listOfTb = new List<TextBlock>();
            try
            {
                for (int i = 0; i < originText.Count; i++)
                {
                    listOfTb.Add(GetColoredText(originText[i], stemmedText[i]));
                }
            }
            catch (Exception)
            {
                //throw new Exception("The two texts have no similar length.");
            }
            return listOfTb;
        }

        private static TextBlock GetColoredText(string s1, string s2)
        {
            TextBlock tbOut = new TextBlock();
            tbOut.FontFamily = new FontFamily("Gill Sans MT");
            tbOut.FontSize = 14;
            tbOut.HorizontalAlignment = HorizontalAlignment.Center;
            tbOut.Foreground = Brushes.White;
            tbOut.Height = 20;
            try
            {
                for (int i = 0; i < s1.Count(); i++)
                {
                    if (s1[i] != s2[i])
                    {
                        Run newRun = new Run(s2[i].ToString());
                        newRun.Background = Brushes.LightBlue;
                        tbOut.Inlines.Add(newRun);
                    }
                    else
                    {
                        tbOut.Text += (s2[i].ToString());
                    }
                }
            }
            catch (Exception)
            {
            }
            return tbOut;
        }

        public static string GetUnitIntentionString(UnitIntention unitIntention)
        {
            switch (unitIntention)
            {
                case UnitIntention.Insertion:
                    return "Insert.";
                    break;
                case UnitIntention.Deletion:
                    return "Del.";
                    break;
                case UnitIntention.Substitution:
                    return "Subs.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("unitIntention");
            }
        }

        public static void SaveJpegImage(Image image, String targetFilePath)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)image.Source.Width,
                                                (int)image.Source.Height,
                                                100, 100, PixelFormats.Default);
            renderTargetBitmap.Render(image);
            JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
            jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            using (FileStream fileStream = new FileStream(targetFilePath, FileMode.Create))
            {
                jpegBitmapEncoder.Save(fileStream);
                fileStream.Flush();
                fileStream.Close();
            }
        }

        public static void SaveImageToHDD(string pathSource, string pathDestination)
        {
            Bitmap bitmap = new Bitmap(pathSource);
            bitmap.Save(pathDestination);
        }
    }
}
