using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using SpellingChecker.ImagesProcessingEngine;
using ZGTR_CROSPELLSpellingCheckerApp;
using ZGTR_CROSPELLSpellingCheckerApp.GUIControllers;
using ZGTR_CROSPELLSpellingCheckerLib.ImagesProcessingEngine;
using Image = System.Windows.Controls.Image;

namespace ZGTR_SpellingCheckerWPFApp.GUIControllers
{
    public class CROSPELLImageTHandler
    {
        private Bitmap _chosenBM;
        private List<Bitmap> _listOfImageInputSet;
        private List<Bitmap> _listOfOutputImages;
        private string _currentTimeTaken;

        public MainWindow MainWindow { get; set; }

        public CROSPELLImageTHandler(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            _listOfImageInputSet = new List<Bitmap>();
            _currentTimeTaken = String.Empty;
            DeletePreviousOutputImagesInHDD();
        }

        private void DeletePreviousOutputImagesInHDD()
        {
            bool proceed = true;
            try
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (proceed)
                    {
                        File.Delete("imageOut" + i.ToString() + ".jpg");
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                proceed = false;
            }
        }

        public void ChooseInputImage()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if ((bool) dialog.ShowDialog())
            {
                this._chosenBM = new Bitmap(dialog.FileName);
                System.Windows.Controls.Image imageInput = new Image();
                imageInput.Source = new BitmapImage(new Uri(dialog.FileName, UriKind.RelativeOrAbsolute));
                ImageContainer imageContainer = new ImageContainer(dialog.FileName, this.MainWindow.spImageIn, 0.ToString());
                this.MainWindow.spImageIn.Children.Add(imageContainer);
                //double contWidth = this.MainWindow.spImageIn.Width;
                //double contHeight = this.MainWindow.spImageIn.Height;
                //double imWidth = this.MainWindow.imageInput.ActualWidth;
                //double imHeight = this.MainWindow.imageInput.Width;
            }
        }



        public void ChooseImageSet()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "(*.*) | *.*";
            _listOfImageInputSet = new List<Bitmap>();
            if ((bool)openFileDialog.ShowDialog())
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    this._listOfImageInputSet.Add(new Bitmap(fileName));
                }
            }
        }

        public void FindAndOrderImages()
        {
            try
            {
                ImageChecker checker = new ImageChecker(this._listOfImageInputSet);
                var subsCost = Int32.Parse(this.MainWindow.tbImSubsCost.Text);
                var errorMargin = Int32.Parse(this.MainWindow.tbImErrorMargin.Text);
                var converArea = Int32.Parse(this.MainWindow.tbImConverArea.Text);
                var boundryWidth = Int32.Parse(this.MainWindow.tbImBoundWidth.Text);
                var boundryHeight = Int32.Parse(this.MainWindow.tbImBoundHeight.Text);
                var thresholdBW = Int32.Parse(this.MainWindow.tbImThreshold.Text);
                OrderingByColorOption orderingByColorOption = GetOrderingOption(this.MainWindow);
                MEDAlgorithmChosen algorithmChosen = GetMEDAlgoChosen();
                DateTime d1 = DateTime.Now;
                _listOfOutputImages = checker.GetClosestImages(_chosenBM, subsCost, errorMargin,
                                                                converArea,
                                                                orderingByColorOption,
                                                                algorithmChosen,
                                                                boundryWidth,
                                                                boundryHeight,
                                                                thresholdBW);
                DateTime d2 = DateTime.Now;
                _currentTimeTaken = (d2 - d1).TotalSeconds.ToString();
                ShowImagesInStackPanel();
            }
            catch (Exception)
            {
            }
        }

        private int iTimer = 0;
        private int iCounter = 0;
        void timerOfAdding_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Bitmap currentBM = _listOfOutputImages[iCounter];
                lock (currentBM)
                {
                    currentBM.Save("imageOut" + iTimer + ".jpg");
                }
                this.MainWindow.Dispatcher.
                Invoke((Action)(() =>
                {
                    lock (this.MainWindow.spImagesOut)
                    {
                        this.MainWindow.spImagesOut.Children.Add(
                            new ImageContainerPatches(this.MainWindow, "imageOut" + iTimer + ".jpg", this.MainWindow.spImagesOut,
                                               _currentTimeTaken));
                    }
                }));
                iTimer++;
                iCounter++;
                if (iTimer == _listOfOutputImages.Count)
                {
                    (sender as Timer).Enabled = false;
                }
            }
            catch (Exception)
            {
            }
        }

        private void ShowImagesInStackPanel()
        {
            iCounter = 0;
            this.MainWindow.spImagesOut.Children.Clear();
            Timer timerOfAdding = new Timer(100);
            timerOfAdding.Elapsed += new ElapsedEventHandler(timerOfAdding_Elapsed);
            timerOfAdding.Enabled = true;

            //for (int i = 0; i < listOfOutputImages.Count; i++)
            //{
            //    Bitmap currentBM = listOfOutputImages[i];
            //    currentBM.Save("imageOut" + i + ".jpg");
            //    this.MainWindow.spImagesOut.Children.Add(new ImageContainer(this.MainWindow.spImagesOut,
            //                                                                "imageOut" + i + ".jpg"));
            //}
        }

        private MEDAlgorithmChosen GetMEDAlgoChosen()
        {
            MEDAlgorithmChosen algorithmChosen = MEDAlgorithmChosen.MEDStalkerWithoutBTEnhanced;
            if ((bool)MainWindow.rbImMEDRegular.IsChecked)
            {
                algorithmChosen = MEDAlgorithmChosen.MEDRegular;
            }
            else
            {
                if ((bool) MainWindow.rbImMEDStalkerWithBT.IsChecked)
                {
                    algorithmChosen = MEDAlgorithmChosen.MEDStalkerWithBT;
                }
                else
                {
                    if ((bool) MainWindow.rbImMEDStalkerWithBTEnhanced.IsChecked)
                    {
                        algorithmChosen = MEDAlgorithmChosen.MEDStalkerWithBTEnhanced;
                    }
                    else
                    {
                        if ((bool) MainWindow.rbImMEDStalkerWithoutBT.IsChecked)
                        {
                            algorithmChosen = MEDAlgorithmChosen.MEDStalkerWithoutBT;
                        }
                        else
                        {
                            if ((bool) MainWindow.rbImMEDStalkerWithoutBTEnhanced.IsChecked)
                            {
                                algorithmChosen = MEDAlgorithmChosen.MEDStalkerWithoutBTEnhanced;
                            }
                        }
                    }
                }
            }
            return algorithmChosen;
        }

        public static OrderingByColorOption GetOrderingOption(MainWindow MainWindow)
        {
            OrderingByColorOption chosen = OrderingByColorOption.ByGreenComp;
            if ((bool) MainWindow.rbImOrdCompR.IsChecked)
            {
                chosen = OrderingByColorOption.ByRedComp;
            }
            else
            {
                if ((bool)MainWindow.rbImOrdCompG.IsChecked)
                {
                    chosen = OrderingByColorOption.ByGreenComp;
                }
                else
                {
                    if ((bool)MainWindow.rbImOrdCompB.IsChecked)
                    {
                        chosen = OrderingByColorOption.ByBlueComp;
                    }
                    else
                    {
                        if ((bool)MainWindow.rbImOrdCompRG.IsChecked)
                        {
                            chosen = OrderingByColorOption.ByRedAndGreenComp;
                        }
                        else
                        {
                            if ((bool)MainWindow.rbImOrdCompRB.IsChecked)
                            {
                                chosen = OrderingByColorOption.ByRedAndBlueComp;
                            }
                            else
                            {
                                if ((bool)MainWindow.rbImOrdCompGB.IsChecked)
                                {
                                    chosen = OrderingByColorOption.ByBlueAndGreenComp;
                                }
                                else
                                {
                                    chosen = OrderingByColorOption.ByRedAndBlueAndGreenComp;
                                }
                            }
                        }
                    }
                }
            }
            return chosen;
        }

        public void ExtractAllOuputImages()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if ((bool)dialog.ShowDialog())
            {
                int counter = 0;
                foreach (Bitmap bitmap in _listOfOutputImages)
                {
                    bitmap.Save(dialog.FileName + counter + ".jpg");
                }
            }
        }
    }
}
