using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Color = System.Windows.Media.Color;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.GreetingTab
{
    public class HelperGeometry
    {
        public static Point3D GetSummedPoint(Point3D p1, Point3D p2)
        {
            Point3D myPoint = new Point3D();
            // Manipulate Coordinates
            myPoint.X = p1.X + p2.X;
            myPoint.Y = p1.Y + p2.Y;
            myPoint.Z = p1.Z + p2.Z;
            return myPoint;
        }

        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        public static System.Windows.Controls.Image ConvertDrawingImageToWPFImage(System.Drawing.Image gdiImg)
        {
            if (gdiImg != null)
            {
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();

                //convert System.Drawing.Image to WPF image
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(gdiImg);
                IntPtr hBitmap = bmp.GetHbitmap();
                System.Windows.Media.ImageSource WpfBitmap =
                    System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty,
                                                                                 BitmapSizeOptions.FromEmptyOptions());

                img.Source = WpfBitmap;
                img.Width = 500;
                img.Height = 600;
                img.Stretch = System.Windows.Media.Stretch.Fill;
                return img;
            }
            return null;
        }

        public static double GetDistance(Point3D p1, Point3D p2)
        {
            // Manipulate Coordinates
            double x = Math.Abs(p1.X - p2.X);
            double y = Math.Abs(p1.Y - p2.Y);
            double z = Math.Abs(p1.Z - p2.Z);
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
        }
    }
}
