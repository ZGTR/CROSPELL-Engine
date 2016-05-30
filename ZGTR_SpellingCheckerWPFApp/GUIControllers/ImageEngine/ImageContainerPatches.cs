using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using SpellingChecker.ImagesProcessingEngine;
using SpellingChecker.SpellingCheckerEngine.Algorithm;
using ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.ImageEngine;
using ZGTR_CROSPELLSpellingCheckerLib.ImagesProcessingEngine;
using ZGTR_SpellingCheckerWPFApp;
using ZGTR_SpellingCheckerWPFApp.GUIControllers;
using Color = System.Windows.Media.Color;
using Image = System.Windows.Controls.Image;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers
{
    class ImageContainerPatches : StackPanel
    {
        private int _eScrollValue = 70;
        private MainWindow _mainWindow;
        private static int _counterOfImageContainerObjects = 0;
        private Image _image;
        private Bitmap _bitmap;
        //private MainWindow _mainWindow;
        //private readonly StackPanel _spContainer;
        private readonly string _imagePath;
        private readonly string _timeTaken;
        private StackPanel _spContainer;
        //private string _currentImageString;
        //public Bitmap _bitmapImage;

        public ImageContainerPatches(MainWindow mainWindow, String imagePath, StackPanel spContainer, String timeTaken)
        {
            //_mainWindow = mainWindow;
            _mainWindow = mainWindow;
            _spContainer = spContainer;
            _imagePath = imagePath;
            _timeTaken = timeTaken;
            this._image = new Image();
            this._image.Source = new ImageSourceConverter().ConvertFrom(_imagePath) as ImageSource;
            this._bitmap = new Bitmap(_imagePath);
            InitializeStackPanel();
            _counterOfImageContainerObjects++;
        }

        private void InitializeStackPanel()
        {
            StackPanel sp = new StackPanel();
            this._image.Margin = new Thickness(2);
            this._image.VerticalAlignment = VerticalAlignment.Center;
            this._image.HorizontalAlignment = HorizontalAlignment.Center;
            this._image.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(ImageMouseWheel);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Image Nr." + _counterOfImageContainerObjects.ToString() + Environment.NewLine + "Time processing: ";
            double value = Double.Parse(_timeTaken);
            textBlock.Text += value.ToString("00.00", CultureInfo.InvariantCulture);
            textBlock.Margin = new Thickness(2);
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;

            Button buttonExtract = new Button();
            buttonExtract.Content = "Extract to file";
            buttonExtract.Width = 100;
            buttonExtract.Height = 30;
            buttonExtract.Margin = new Thickness(2);
            buttonExtract.VerticalAlignment = VerticalAlignment.Center;
            buttonExtract.HorizontalAlignment = HorizontalAlignment.Center;
            buttonExtract.Click += new RoutedEventHandler(ButtonExtractClick);

            Button buttonColorTable = new Button();
            buttonColorTable.Content = "Show Color Table";
            buttonColorTable.Width = 100;
            buttonColorTable.Height = 20;
            buttonColorTable.Margin = new Thickness(2);
            buttonColorTable.VerticalAlignment = VerticalAlignment.Center;
            buttonColorTable.HorizontalAlignment = HorizontalAlignment.Left;
            buttonColorTable.Click += new RoutedEventHandler(buttonColorTable_Click);

            Button buttonExit = new Button();
            buttonExit.Content = "X";
            buttonExit.Width = 20;
            buttonExit.Height = 20;
            buttonExit.Margin = new Thickness(2);
            buttonExit.VerticalAlignment = VerticalAlignment.Center;
            buttonExit.HorizontalAlignment = HorizontalAlignment.Right;
            buttonExit.Click += new RoutedEventHandler(ButtonExitClick);

            Button bShowPatches = new Button();
            bShowPatches.Content = "Show Patches";
            bShowPatches.Width = 125;
            bShowPatches.Height = 20;
            bShowPatches.Margin = new Thickness(2);
            bShowPatches.VerticalAlignment = VerticalAlignment.Center;
            bShowPatches.HorizontalAlignment = HorizontalAlignment.Center;
            bShowPatches.Click += new RoutedEventHandler(bShowPatches_Click);



            StackPanel spSmall1 = new StackPanel();
            spSmall1.Orientation = Orientation.Horizontal;
            spSmall1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            spSmall1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            spSmall1.Children.Add(buttonColorTable);
            spSmall1.Children.Add(buttonExit);
            
            

            Border border = new Border();
            border.BorderThickness = new Thickness(2);
            border.Padding = new Thickness(2);
            border.BorderBrush = new SolidColorBrush(Colors.DarkGray);
            sp.Children.Add(spSmall1);
            sp.Children.Add(bShowPatches);
            sp.Children.Add(textBlock);
            sp.Children.Add(_image);
            sp.Children.Add(buttonExtract);
            border.Child = sp;

            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            this.Children.Add(border);

            var animation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(4)));
            //animation.BeginTime = TimeSpan.FromSeconds(0.5);
            this._image.BeginAnimation(Image.OpacityProperty, animation);
            //Thread.Sleep(400);
        }

        void ButtonExitClick(object sender, RoutedEventArgs e)
        {
            _spContainer.Children.Remove(this);
        }

        private static int iImPatches = 0;
        void bShowPatches_Click(object sender, RoutedEventArgs e)
        {
            var subsCost = Int32.Parse(this._mainWindow.tbImSubsCost.Text);
            var boundryWidth = Int32.Parse(this._mainWindow.tbImBoundWidth.Text);
            var boundryHeight = Int32.Parse(this._mainWindow.tbImBoundHeight.Text);
            var thresholdBW = Int32.Parse(this._mainWindow.tbImThreshold.Text);
            //OrderingByColorOption orderingByColorOption = CROSPELLImageTHandler.GetOrderingOption(this._mainWindow);
            ColorComponent comp = GetColorComponentOption(this._mainWindow);
            UnitIntention unitIntention = GetUnitInsetionOption(this._mainWindow);

            Bitmap sBM = new Bitmap((this._mainWindow.spImageIn.Children[0] as ImageContainer)._imagePath);
            Bitmap tBM = new Bitmap(this._imagePath);
            ImagePatchFinder imagePatchFinder = new ImagePatchFinder(sBM, tBM);
            var list = imagePatchFinder.GetPatchedAreaBitmap(comp,
                boundryWidth, boundryHeight, thresholdBW, subsCost, 1, 2, unitIntention);
            var imagePatched = "outputPatchImage" + iImPatches.ToString() +  ".jpg";
            list[0].Save(imagePatched);

            iImPatches++;
            new ImagePatchWindow(this._mainWindow,
                (this._mainWindow.spImageIn.Children[0] as ImageContainer)._imagePath,
                this._imagePath,
                imagePatched).Show();
        }

        private ColorComponent GetColorComponentOption(MainWindow mainWindow)
        {
            ColorComponent comp = ColorComponent.R;
            if ((bool)mainWindow.rbPFBlue.IsChecked)
            {
                comp = ColorComponent.B;
            }
            else
            {
                if ((bool)mainWindow.rbPFGreen.IsChecked)
                {
                    comp = ColorComponent.G;
                }
            }
            return comp;
        }

        private UnitIntention GetUnitInsetionOption(MainWindow mainWindow)
        {
            UnitIntention unit = UnitIntention.Insertion;
            if ((bool)mainWindow.rbPFDeletion.IsChecked)
            {
                unit = UnitIntention.Deletion;
            }
            else
            {
                if ((bool)mainWindow.rbPFSubstitution.IsChecked)
                {
                    unit = UnitIntention.Substitution;
                }
            }
            return unit;
        }

        void buttonColorTable_Click(object sender, RoutedEventArgs e)
        {
            new ColorTableWindow(this._bitmap);
        }

        void ImageMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            try
            {
                if (this._image.ActualWidth > _eScrollValue)
                {
                    if (e.Delta > 0)
                    _image.BeginAnimation(System.Windows.Controls.Image.WidthProperty,
                                          new DoubleAnimation(this._image.ActualWidth,
                                                              this._image.ActualWidth + _eScrollValue,
                                                              new Duration(TimeSpan.FromSeconds(1))));
                    else
                    {
                        _image.BeginAnimation(System.Windows.Controls.Image.WidthProperty,
                            new DoubleAnimation(this._image.ActualWidth,
                         this._image.ActualWidth - _eScrollValue,
                         new Duration(TimeSpan.FromSeconds(1))));
                    }
  
                }
                else
                {
                    _image.BeginAnimation(System.Windows.Controls.Image.WidthProperty,
                     new DoubleAnimation(this._image.ActualWidth,
                                         this._image.ActualWidth + _eScrollValue,
                                         new Duration(TimeSpan.FromSeconds(1))));

                }
            }
            catch (Exception)
            {
                

            }
        }

        void ButtonExtractClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            if ((bool)sd.ShowDialog())
            {
                HelperModule.SaveImageToHDD(this._imagePath, sd.FileName);
            }
        }
    }
}

