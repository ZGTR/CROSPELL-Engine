using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using SpellingChecker.ImagesProcessingEngine;
using SpellingChecker.SpellingCheckerEngine.Algorithm;
using SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos;
using SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZGTR_CROSPELLSpellingCheckerApp;
using ZGTR_CROSPELLSpellingCheckerApp.GUIControllers;
using ZGTR_SpellingCheckerWPFApp.GUIControllers;


namespace ZGTR_SpellingCheckerWPFApp
{
    public class GenomeCipherAreaHandler
    {
        public MainWindow MainWindow { get; set; }
        private List<TextBlock> _listOfTbTarget;
        private List<TextBlock> _listOfTbSource;
        private int _currentMax;
        private List<List<CellWrapper>> _currentListOfListOfWrappers;
        private string _currTargetWord;
        private string _currSourceWord;
        private StackPanel _spSource;
        private StackPanel _spTarget;
        private int _pageIndex = 0;
        private double _tbWidth = 20;
        private double _tbHeigh = 20;
        private double _posLeftSpMiddle = 824 / 2;

        public GenomeCipherAreaHandler(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            this._currentListOfListOfWrappers = new List<List<CellWrapper>>();
            this._currentMax = 0;
        }

        public void MatchGenomes()
        {
            _currTargetWord = this.MainWindow.tbTxGWordTarget.Text;
            _currSourceWord = this.MainWindow.tbTxGWordSource.Text;
            int subsCost = Int32.Parse(this.MainWindow.tbTxGSubsCost.Text);
            int dw = Int32.Parse(this.MainWindow.tbTxGDw.Text);
            int matchCost = Int32.Parse(this.MainWindow.tbTxGMatchCost.Text);
            int upperBound = Int32.Parse(this.MainWindow.tbTxGupperBound.Text);
            bool isProceed = false;
            if ((bool)this.MainWindow.cbTxGIsProceed.IsChecked)
            {
                isProceed = true;
            }

            if ((bool)MainWindow.rbTxGRegularBT.IsChecked)
            {
                MEDRegularWithBT algo = new MEDRegularWithBT(_currTargetWord.ToArray(),
                                     _currSourceWord.ToArray(), subsCost);
                _currentMax = algo.GetMED();
                _currentListOfListOfWrappers = algo.GetBackTraceArray(upperBound);
            }
            else
            {
                if ((bool) MainWindow.rbTxGNeedlemanWunsch.IsChecked)
                {
                    MEDNeedlemanWunschAlgoMod algo = new MEDNeedlemanWunschAlgoMod(_currTargetWord.ToArray(),
                                                         _currSourceWord.ToArray(),
                                                         subsCost, dw, matchCost);
                    _currentMax = algo.GetMED();
                    _currentListOfListOfWrappers = algo.GetBackTraceArray(upperBound, isProceed);
                }
                else
                {
                    if ((bool) MainWindow.rbTxGSmithWaterman.IsChecked)
                    {
                        MEDSmithWatermanAlgo algo = new MEDSmithWatermanAlgo(_currTargetWord.ToArray(),
                                                                               _currSourceWord.ToArray(),
                                                                               subsCost, dw, matchCost);
                        _currentMax = algo.GetMED();
                        _currentListOfListOfWrappers = algo.GetBackTraceArray(upperBound, isProceed);
                    }
                }
            }
            try
            {
                this._pageIndex = 0;
                this.MainWindow.tblTxGPage.Text = (_pageIndex + 1) + "/" + this._currentListOfListOfWrappers.Count;
                ShowMatches();
            }
            catch(Exception)
            {}
        }

        public void ShowMatches()
        {
            this.MainWindow.spTxGSourceArea.Children.Clear();
            this.MainWindow.spTxGTargetArea.Children.Clear();
            
            List<CellWrapper> currentCells = _currentListOfListOfWrappers[this._pageIndex];
            InitializeStackPanelsItems(currentCells, this._currSourceWord, this._currTargetWord);
            InitializeCellWrappers(currentCells);
            InitializeStackPanels();
            this.MainWindow.spTxGSourceArea.Children.Add(_spSource);
            this.MainWindow.spTxGTargetArea.Children.Add(_spTarget);
            Animate();
        }

        private void InitializeCellWrappers(List<CellWrapper> currentCells)
        {
            this.MainWindow.spTxGCellWrappers.Children.Clear();
            for (int i = 0; i < currentCells.Count; i++)
            {
                CellWrapper currentCell = currentCells[i];
                GroupBox gBox = new GroupBox();
                TextBlock tb = new TextBlock();
                tb.FontFamily = new FontFamily("Gill Sans MT");
                //tb.TextWrapping = TextWrapping.Wrap;
                //tb.TextAlignment = TextAlignment.Center;
                tb.Text = currentCell.CSWord + " to " + currentCell.CTWord + "\n"
                          + "Distance = " + currentCell.D + "\n"
                           + "Position[" + currentCell.I + " ," + currentCell.J + "]" + "\n"
                           + "Pointer: " + HelperModule.GetUnitIntentionString(currentCell.UnitIntention);
                gBox.Content = tb;
                this.MainWindow.spTxGCellWrappers.Children.Add(gBox);
            }
        }

        private void InitializeStackPanels()
        {
            _spSource = new StackPanel();
            _spSource.Orientation = Orientation.Horizontal;
            _spTarget = new StackPanel();
            _spTarget.Orientation = Orientation.Horizontal;
            foreach (var textBlock in _listOfTbSource)
            {
                _spSource.Children.Add(textBlock);
            }
            foreach (var textBlock in _listOfTbTarget)
            {
                _spTarget.Children.Add(textBlock);
            }
        }

        public void InitializeStackPanelsItems(List<CellWrapper> cells, string wordTarget, string wordSource)
        {
            this._listOfTbSource = new List<TextBlock>();
            this._listOfTbTarget = new List<TextBlock>();
            //PopulateTextBlocks(wordSource);
            //PopulateTextBlocks(wordSource);

            for (int i = 0; i < cells.Count; i++)
            {
                CellWrapper currentCell = cells[i];
                int iSource = currentCell.I;
                int jTarget = currentCell.J;

                switch (currentCell.UnitIntention)
                {
                    case UnitIntention.Insertion:
                        //_listOfTbSource.Add(GetTextBlock(currentCell.CSWord.ToString()));
                        _listOfTbSource.Add(GetTextBlock("-".ToString()));

                        _listOfTbTarget.Add(GetTextBlock(currentCell.CTWord.ToString()));
                        break;
                    case UnitIntention.Deletion:
                        _listOfTbSource.Add(GetTextBlock(currentCell.CSWord.ToString()));

                        //_listOfTbTarget.Add(GetTextBlock(currentCell.CTWord.ToString()));
                        _listOfTbTarget.Add(GetTextBlock("-".ToString()));
                        break;
                    case UnitIntention.Substitution:
                        _listOfTbSource.Add(GetTextBlock(currentCell.CSWord.ToString()));
                        _listOfTbTarget.Add(GetTextBlock(currentCell.CTWord.ToString()));
                                        SolidColorBrush brush = new SolidColorBrush(Colors.Gold);
                        _listOfTbSource[_listOfTbSource.Count - 1].Background = brush;
                        _listOfTbTarget[_listOfTbTarget.Count - 1].Background = brush;
                        ColorAnimation cAnimation = new ColorAnimation(Colors.Gold, Colors.DarkRed,
                                                                       new Duration(TimeSpan.FromSeconds(0.5)));
                        cAnimation.AutoReverse = true;
                        cAnimation.RepeatBehavior = new RepeatBehavior(5);
                        brush.BeginAnimation(SolidColorBrush.ColorProperty, cAnimation);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void SubPage()
        {
            if (_pageIndex > 0)
            {
                _pageIndex--;
                this.MainWindow.tblTxGPage.Text = (_pageIndex+1) + "/" + this._currentListOfListOfWrappers.Count;
                ShowMatches();
            }
        }

        public void AddPage()
        {
            if (_pageIndex < this._currentListOfListOfWrappers.Count - 1)
            {
                _pageIndex++;
                this.MainWindow.tblTxGPage.Text = (_pageIndex + 1) + "/" + this._currentListOfListOfWrappers.Count;
                ShowMatches();
            }
        }

        public void Animate()
        {
            var animationOpacity = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(3)));
            this.MainWindow.spTxGBasic.BeginAnimation(StackPanel.OpacityProperty, animationOpacity);

            this._posLeftSpMiddle = this.MainWindow.svTxGGenomesArea.Width/2;

            // x1 for Target - Up
            double x1 = (this._currentListOfListOfWrappers[_pageIndex][0].J + 1) * this._tbWidth;
            double toLeftTarget = this._posLeftSpMiddle - x1;

            // x2 for Source - Down
            double x2 = (this._currentListOfListOfWrappers[_pageIndex][0].I + 1) * this._tbWidth;
            double toLeftSource = this._posLeftSpMiddle - x2;


            Thickness thickFromTarget = this._spTarget.Margin;
            Thickness thickToTarget = thickFromTarget;
            thickToTarget.Left = toLeftTarget;

            ThicknessAnimation thickAnimT = new ThicknessAnimation(thickFromTarget, thickToTarget,
                                                                  new Duration(TimeSpan.FromSeconds(1)));
            this._spTarget.BeginAnimation(StackPanel.MarginProperty, thickAnimT);


            Thickness thickFromSource = this._spSource.Margin;
            Thickness thickToSource = thickFromSource;
            thickToSource.Left = toLeftSource;
            ThicknessAnimation thickAnimS = new ThicknessAnimation(thickFromSource, thickToSource,
                                                                  new Duration(TimeSpan.FromSeconds(1)));
            this._spSource.BeginAnimation(StackPanel.MarginProperty, thickAnimS);

        }

        private List<TextBlock> PopulateTextBlocks(string word)
        {
            List<TextBlock> list = new List<TextBlock>();
            char[] wordArr = word.ToArray();
            for (int i = 0; i < wordArr.Count(); i++)
            {
                TextBlock tb = GetTextBlock(wordArr[i].ToString());
                list.Add(tb);
            }
            return list;
        }

        private TextBlock GetTextBlock(string word)
        {
            TextBlock tb = new TextBlock();
            tb.FontFamily = new FontFamily("Gill Sans MT");
            tb.FontSize = 15;
            tb.TextAlignment = TextAlignment.Center;
            tb.Text = word;
            tb.Width = _tbWidth;
            tb.Height = _tbHeigh;
            return tb;
        }

        public void LoadPreDefineGenome()
        {
            // load Jufersky Genomes Ciphers
            this.MainWindow.tbTxGWordSource.Text = "CTATCACCTGACCTCCAGGCCGATGCCCCTTCCGGC";
            this.MainWindow.tbTxGWordTarget.Text = "GCGAGTTCATCTATCACGACCGCGGTCG";
        }
    }
}
