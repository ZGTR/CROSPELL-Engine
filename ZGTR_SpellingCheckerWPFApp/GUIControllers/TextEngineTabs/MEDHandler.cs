using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using SpellingChecker.Keyboard;
using SpellingChecker.SpellingCheckerEngine.Algorithm;
using SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos;
using SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos.Enums;
using ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.PorterHandler;
using ZGTR_PorterAlgorithmApp;
using ZGTR_SpellingCheckerWPFApp;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.TextEngineTabs
{
    public class MEDHandler
    {
        private List<string> _listOfSourceWords;
        public MainWindow MainWindow { get; set; }
        private List<TextBlock> _listOfTbTarget;
        private List<TextBlock> _listOfTbSource;
        private List<List<CellWrapper>> _currentListOfListOfWrappers;
        private string _currTargetWord;
        private string _currSourceWord;
        private StackPanel _spSource;
        private StackPanel _spTarget;
        private int _pageIndex = 0;
        private double _tbWidth = 20;
        private double _tbHeigh = 20;
        private double _posLeftSpMiddle = 824 / 2;
        private int _currentMin;
        private MEDRegularWithBT _algo;
        private bool _isKeyMap = false;

        public MEDHandler(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            this._currentListOfListOfWrappers = new List<List<CellWrapper>>();
        }

        public void ShowBackTrack()
        {
            try
            {

     
            this.MainWindow.spTxLTBPath.Children.Clear();
            String currentSource = this._listOfSourceWords[_pageIndex];
            List<CellWrapper> currentCells = this._currentListOfListOfWrappers[_pageIndex];
            int insDelCost = Int32.Parse(this.MainWindow.tbTxLInsDelCost.Text);
            int subsCost = Int32.Parse(this.MainWindow.tbTxLSubsCost.Text);
            if (_isKeyMap)
            {
                _algo = new MEDRegularWithBTHeuristics(_currTargetWord.ToArray(),
                                                      currentSource.ToArray(),
                                                      insDelCost,
                                                      KeyboardLanguage.English);
            }
            else
            {
                _algo = new MEDRegularWithBT(_currTargetWord.ToArray(), currentSource.ToCharArray(),
                                                                             subsCost);
            }

            int [,] distanceArray = _algo.GetDistanceArray();
            for (int i = 0; i < distanceArray.GetLength(0); i++)
            {
                StackPanel spCurrent = new StackPanel();
                spCurrent.Orientation = Orientation.Horizontal;
                for (int j = 0; j < distanceArray.GetLength(1); j++)
                {
                    if (isCellInBTPath(i, j, currentCells))
                        spCurrent.Children.Add(GetTextBlock(distanceArray[i, j].ToString(), Colors.LightGreen));
                    else
                        spCurrent.Children.Add(GetTextBlock(distanceArray[i, j].ToString(), Colors.LightGray));
                }
                this.MainWindow.spTxLTBPath.Children.Add(spCurrent);
            }
            }
            catch (Exception)
            {

            }
        }

        private bool isCellInBTPath(int i, int j, List<CellWrapper> currentCells)
        {
            for (int k = 0; k < currentCells.Count; k++)
            {
                if (currentCells[k].I == i && currentCells[k].J == j)
                {
                    return true;
                }
            }
            return false;
        }

        private TextBlock GetTextBlock(string word, Color colorBG)
        {
            TextBlock tb = new TextBlock();
            tb.FontFamily = new FontFamily("Gill Sans MT");
            tb.Background = new SolidColorBrush(colorBG);
            tb.FontSize = 15;
            tb.TextAlignment = TextAlignment.Center;
            tb.Text = word;
            tb.Width = _tbWidth;
            tb.Height = _tbHeigh;
            return tb;
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

        public void FindDistance()
        {
            try
            {
                _pageIndex = 0;
                this._currentListOfListOfWrappers.Clear();
                _currTargetWord = this.MainWindow.tbTxLWordTarget.Text;
                int insDelCost = Int32.Parse(this.MainWindow.tbTxLInsDelCost.Text);
                int subsCost = Int32.Parse(this.MainWindow.tbTxLSubsCost.Text);
                if ((bool)this.MainWindow.cbTxLKeyMapWeights.IsChecked)
                {
                    _isKeyMap = true;
                }
                else
                {
                    _isKeyMap = false;
                }
                for (int i = 0; i < this._listOfSourceWords.Count; i++)
                {
                    _currSourceWord = _listOfSourceWords[i];
                    if (_isKeyMap)
                    {
                        _algo = new MEDRegularWithBTHeuristics(_currTargetWord.ToArray(),
                                                              _currSourceWord.ToArray(),
                                                              insDelCost,
                                                              KeyboardLanguage.English);
                    }
                    else
                    {
                        _algo = new MEDRegularWithBT(_currTargetWord.ToArray(), _currSourceWord.ToArray(),
                                                                                     subsCost);
                    }
                    _currentMin = _algo.GetMED();
                    _currentListOfListOfWrappers.Add(_algo.GetBackTraceArray()[0]);
                }
                _currentListOfListOfWrappers.Sort(new Comparison<List<CellWrapper>>(CompareCellsDistances));
                _currentListOfListOfWrappers.Reverse();
                try
                {
                    ShowDistancesArea();
                    this.MainWindow.tblTxLPage.Text = (_pageIndex + 1) + "/" + this._currentListOfListOfWrappers.Count;
                    ShowMatches();
                }
                catch(Exception)
                {}
            }
            catch (Exception)
            {
            }
        }

        private int CompareCellsDistances(List<CellWrapper> x, List<CellWrapper> y)
        {
            if (x[x.Count-1].D  > y[y.Count-1].D)
            {
                return 1;
            }
            else
            {
                if (x[x.Count - 1].D < y[y.Count - 1].D)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }

        private void ShowDistancesArea()
        {
            this.MainWindow.spTxLTargetsDist.Children.Clear();
            for (int i = 0; i < this._listOfSourceWords.Count; i++)
            {
                GroupBox gBox = new GroupBox();
                TextBlock tb = new TextBlock();
                tb.FontFamily = new FontFamily("Gill Sans MT");
                //tb.TextWrapping = TextWrapping.Wrap;
                //tb.TextAlignment = TextAlignment.Center;
                tb.Text = _listOfSourceWords[i] + ", " +
                          _currentListOfListOfWrappers[i][_currentListOfListOfWrappers[i].Count - 1].D;
                gBox.Content = tb;
                this.MainWindow.spTxLTargetsDist.Children.Add(gBox);
            }
        }

        public void ShowMatches()
        {
            this.MainWindow.spTxLSourceArea.Children.Clear();
            this.MainWindow.spTxLTargetArea.Children.Clear();
            
            List<CellWrapper> currentCells = _currentListOfListOfWrappers[this._pageIndex];
            InitializeStackPanelsItems(currentCells, this._currSourceWord, this._currTargetWord);
            InitializeCellWrappers(currentCells);
            InitializeStackPanels();
            this.MainWindow.spTxLSourceArea.Children.Add(_spSource);
            this.MainWindow.spTxLTargetArea.Children.Add(_spTarget);
            Animate();
        }

        private void InitializeCellWrappers(List<CellWrapper> currentCells)
        {
            this.MainWindow.spTxLCellWrappers.Children.Clear();
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
                this.MainWindow.spTxLCellWrappers.Children.Add(gBox);
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

                ColorAnimation cAnimation;
                SolidColorBrush brush = new SolidColorBrush(Colors.Gold);
                switch (currentCell.UnitIntention)
                {
                    case UnitIntention.Insertion:
                        //_listOfTbSource.Add(GetTextBlock(currentCell.CSWord.ToString()));
                        _listOfTbSource.Add(GetTextBlock("-".ToString()));

                        _listOfTbTarget.Add(GetTextBlock(currentCell.CTWord.ToString()));
                        cAnimation = new ColorAnimation(Colors.Gold, Colors.DarkRed,
                                                        new Duration(TimeSpan.FromSeconds(0.5)));
                        cAnimation.AutoReverse = true;
                        cAnimation.RepeatBehavior = new RepeatBehavior(5);
                        brush.BeginAnimation(SolidColorBrush.ColorProperty, cAnimation);
                        _listOfTbSource[_listOfTbSource.Count - 1].Background = brush;
                        _listOfTbTarget[_listOfTbTarget.Count - 1].Background = brush;
                        break;
                    case UnitIntention.Deletion:
                        _listOfTbSource.Add(GetTextBlock(currentCell.CSWord.ToString()));

                        //_listOfTbTarget.Add(GetTextBlock(currentCell.CTWord.ToString()));
                        _listOfTbTarget.Add(GetTextBlock("-".ToString()));
                        cAnimation = new ColorAnimation(Colors.Gold, Colors.DarkRed,
                                                        new Duration(TimeSpan.FromSeconds(0.5)));
                        cAnimation.AutoReverse = true;
                        cAnimation.RepeatBehavior = new RepeatBehavior(5);
                        brush.BeginAnimation(SolidColorBrush.ColorProperty, cAnimation);
                        _listOfTbSource[_listOfTbSource.Count - 1].Background = brush;
                        _listOfTbTarget[_listOfTbTarget.Count - 1].Background = brush;
                        break;
                    case UnitIntention.Substitution:
                        _listOfTbSource.Add(GetTextBlock(currentCell.CSWord.ToString()));
                        _listOfTbTarget.Add(GetTextBlock(currentCell.CTWord.ToString()));
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
                this.MainWindow.tblTxLPage.Text = (_pageIndex + 1) + "/" + this._currentListOfListOfWrappers.Count;
                ShowMatches();
            }
        }

        public void AddPage()
        {
            if (_pageIndex < this._currentListOfListOfWrappers.Count - 1)
            {
                _pageIndex++;
                this.MainWindow.tblTxLPage.Text = (_pageIndex + 1) + "/" + this._currentListOfListOfWrappers.Count;
                ShowMatches();
            }
        }

        public void Animate()
        {
            var animationOpacity = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(3)));
            this.MainWindow.spTxGBasic.BeginAnimation(StackPanel.OpacityProperty, animationOpacity);

            this._posLeftSpMiddle = this.MainWindow.svTxLSTArea.Width/2;

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
                                                                  new Duration(TimeSpan.FromSeconds(0.5)));
            this._spTarget.BeginAnimation(StackPanel.MarginProperty, thickAnimT);


            Thickness thickFromSource = this._spSource.Margin;
            Thickness thickToSource = thickFromSource;
            thickToSource.Left = toLeftSource;
            ThicknessAnimation thickAnimS = new ThicknessAnimation(thickFromSource, thickToSource,
                                                                  new Duration(TimeSpan.FromSeconds(0.5)));
            this._spSource.BeginAnimation(StackPanel.MarginProperty, thickAnimS);

        }

        public void UploadSourceFromTextFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if ((bool) dialog.ShowDialog())
            {
                var filePath = dialog.FileName;
                _listOfSourceWords =  HelperModule.CrackTextToWords(filePath);
            }
        }

        public void SetSourceWord()
        {
            this._listOfSourceWords = new List<string>() { this.MainWindow.tbTxLWordSource.Text };
        }

        public void UploadSourceFromExcelFile()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    this._listOfSourceWords = ExcelFileManager.ReadColumnInExcelFile(dialog.FileName, "Sheet1", "A");
                }
            }
            catch (Exception)
            {
            }
        }
        
        public void ExportExcelFile()
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    List<string> wordsOutput = GetOrderedOutputWords();
                    ExcelFileManager.WriteToColumnInExcelFile(dialog.FileName, "Sheet1", "A", "Results", wordsOutput);
                }
            }
            catch (Exception)
            {
            }
        }

        private List<string> GetOrderedOutputWords()
        {
            List<string> list = new List<string>();
            StackPanel sp = this.MainWindow.spTxLTargetsDist;
            foreach (var child in sp.Children)
            {
                GroupBox gp = child as GroupBox;
                TextBlock tb = gp.Content as TextBlock;
                list.Add(tb.Text);
            }
            return list;
        }
    }
}
