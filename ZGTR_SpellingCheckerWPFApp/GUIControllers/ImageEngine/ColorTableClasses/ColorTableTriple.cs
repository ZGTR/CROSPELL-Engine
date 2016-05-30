//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Media;
//using Octree_ZGTR_WPFApp.Engine.EncodingPackage;
//using Color = System.Drawing.Color;

//namespace Octree_ZGTR_WPFApp.Engine.GUI
//{
//    public class ColorTableTriple
//    {
//        public Brush ColorInTable { get; set; }
//        public string Index { get; set; }
//        public string ColorInBinary { set; get; }

//        public ColorTableTriple(int index, Color colorInTable)
//        {
//            ColorInTable =
//                new SolidColorBrush(System.Windows.Media.Color.FromRgb(colorInTable.R, colorInTable.G, colorInTable.B));
//            Index = index.ToString();
//            ColorInBinary = EncodingConverter.ConvertIntToBinStatic(colorInTable.ToArgb());
//        }
//    }
//}
