using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SpellingChecker.ImagesProcessingEngine;
using SpellingChecker.SpellingCheckerEngine.Algorithm;
using ZGTR_CROSPELLSpellingCheckerLib.ImagesProcessingEngine;
using ZGTR_SpellingCheckerWPFApp;
using Image = System.Windows.Controls.Image;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.ImageEngine
{
    /// <summary>
    /// Interaction logic for ImagePatchWindow.xaml
    /// </summary>
    public partial class ImagePatchWindow : Window
    {
        private readonly string _imPathSource;
        private readonly string _imPathTarget;
        private ImageContainer imSourceContainer;
        private ImageContainer imTargetContainer;
        private ImageContainer imPatchedContainer;
        private Image imSource;
        private Image imTarget;
        

        public ImagePatchWindow(MainWindow mWindow, string imPathSource, string imPathTarget, string imPathPatched)
        {
            InitializeComponent();

            _imPathSource = imPathSource;
            _imPathTarget = imPathTarget;

            this.imSource = new Image() {Source = new ImageSourceConverter().ConvertFrom(_imPathSource) as ImageSource};
            this.imTarget = new Image() {Source = new ImageSourceConverter().ConvertFrom(_imPathTarget) as ImageSource};

            imSourceContainer = new ImageContainer(this._imPathSource, this.spContainer, "");
            imTargetContainer = new ImageContainer(this._imPathTarget, this.spContainer, "");
            imPatchedContainer = new ImageContainer(imPathPatched, this.spContainer, "");

            this.spContainer.Children.Add(this.imSourceContainer);
            this.spContainer.Children.Add(this.imTargetContainer);
            //Bitmap b = new Bitmap("imageOut0.jpg");
            //b.Save("sss.jpg");
            this.spContainer.Children.Add(this.imPatchedContainer);

        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    Bitmap sBM = new Bitmap(this._imPathSource);
        //    Bitmap tBM = new Bitmap(this._imPathTarget);
        //    ImagePatchFinder imagePatchFinder = new ImagePatchFinder(sBM, tBM);
        //    var list = imagePatchFinder.GetPatchedAreaBitmap(ColorComponent.B, 0, 0, 150, 1, 1, 2, UnitIntention.Insertion);
        //    list[0].Save(@"ImagePatchFinder\output.jpg");
        //}
    }
}
