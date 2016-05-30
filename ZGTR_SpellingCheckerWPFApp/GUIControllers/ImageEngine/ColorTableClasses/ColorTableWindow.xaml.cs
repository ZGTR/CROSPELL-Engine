using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using FontFamily = System.Windows.Media.FontFamily;
using Image = System.Windows.Controls.Image;
using Point = ZGTR_SpellingCheckerWPFApp.GUIControllers.Point;
using Rectangle = System.Windows.Shapes.Rectangle;
using Color = System.Drawing.Color;

namespace ZGTR_SpellingCheckerWPFApp.GUIControllers
{
    /// <summary>
    /// Interaction logic for ColorTableWindow.xaml
    /// </summary>
    public partial class ColorTableWindow : Window
    {
        //public ObservableCollection<ColorTableTriple> ColorsInTable = new ObservableCollection<ColorTableTriple>();
        private Image _image;
        private System.Drawing.Bitmap _bitmap;
        private List<System.Drawing.Color> _listOfUniqueColors;
        private List<int> _arrOfOccurences;

        public ColorTableWindow(System.Drawing.Bitmap bitmap)
        {
            this._bitmap = bitmap;
            InitializeColorUniqueList(bitmap);
            InitializeComponent();
            BuildColorTable();
            BuildPlotter();
            this.Show();
        }

        private List<Color> InitializeColorUniqueList(Bitmap bitmap)
        {
            _arrOfOccurences = new List<int>();
            _listOfUniqueColors = new List<System.Drawing.Color>();

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color currentColor = bitmap.GetPixel(i, j);
                    bool appearedBefore = false;
                    for (int k = 0; k < _listOfUniqueColors.Count; k++)
                    {
                        appearedBefore = false;
                        if (_listOfUniqueColors[k].R == currentColor.R 
                                   && _listOfUniqueColors[k].G == currentColor.G 
                                   && _listOfUniqueColors[k].B == currentColor.B)
                        {
                            _arrOfOccurences[k]++;
                            appearedBefore = true;
                            break;
                        }
                    }
                    if (!appearedBefore)
                    {
                        _listOfUniqueColors.Add(currentColor);
                        _arrOfOccurences.Add(1);
                    }
                }
            }
            return _listOfUniqueColors;
        }

        public bool TwoColorComparer(Color color, Color currentColor)
        {
            if (color.R == currentColor.R && color.G == currentColor.G &&
                                        color.B == currentColor.B)
            {
                return true;
            }
            else
                return false;
        }

        private void BuildPlotter()
        {
            List<Point> listOfPoints = BuildPointsList();
            SolidColorBrush brush = new SolidColorBrush(Colors.Red);
            PlotPoints(listOfPoints, brush);
        }

        private void PlotPoints(IEnumerable<Point> points, System.Windows.Media.Brush brush)
        {
            var marker = new CirclePointMarker();
            marker.Fill = brush;
            var ds = new EnumerableDataSource<Point>(points);
            ds.SetXMapping(p => p.X);
            ds.SetYMapping(p => p.Y);
            var markerGraph = new MarkerPointsGraph
            {
                DataSource = ds,
                Marker = marker
            };
            this.chartPlotterHistogram.Children.Add(markerGraph);
        }

        private List<Point> BuildPointsList()
        {
            //for (int i = 0; i < _bitmap.Height; i++)
            //{
            //    for (int j = 0; j < _bitmap.Width; j++)
            //    {
            //        System.Drawing.Color currentColor = _bitmap.GetPixel(j, i);
            //        System.Drawing.Color colorMatched = (from color in _listOfUniqueColors
            //                 where color.R == currentColor.R && color.G == currentColor.G && color.B == currentColor.B
            //                 select color).First();
            //        int index = _listOfUniqueColors.IndexOf(colorMatched);
            //        _arrOfOccurences[index]++;
            //    }
            //}
            List<Point> listOfPoints = new List<Point>();
            for (int i = 0; i < _arrOfOccurences.Count(); i++)
            {
                listOfPoints.Add(new Point(i, _arrOfOccurences[i]));
            }
            return listOfPoints;
        }

        private void BuildColorTable()
        {
            for (int i = 0; i < this._listOfUniqueColors.Count; i++)
            {
                StackPanel spSmall = new StackPanel();
                spSmall.Orientation = Orientation.Horizontal;
                spSmall.VerticalAlignment = VerticalAlignment.Center;
                spSmall.HorizontalAlignment = HorizontalAlignment.Center;
                spSmall.Margin = new Thickness(1);
                System.Drawing.Color c = this._listOfUniqueColors[i];
                TextBlock t = new TextBlock() {Text = i.ToString()};
                t.VerticalAlignment = VerticalAlignment.Center;
                t.FontFamily = new FontFamily("Century Gothic");
                t.Width = 30;
                t.Height = 30;
                Rectangle tColor = new Rectangle() {};
                tColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(c.R, c.G, c.B));
                tColor.Width = 60;
                tColor.Height = 30;
                spSmall.Children.Add(t);
                spSmall.Children.Add(tColor);
                this.stackPanelColorTable.Children.Add(spSmall);
            }
        }

        //private void BuildObesrvable()
        //{
        //    ColorsInTable = new ObservableCollection<ColorTableTriple>();
        //    for (int i = 0; i < this._colorTable.Table.Length; i++)
        //    {
        //        ColorsInTable.Add(new ColorTableTriple(i, this._colorTable.Table[i]));
        //    }
        //}
    }
}
