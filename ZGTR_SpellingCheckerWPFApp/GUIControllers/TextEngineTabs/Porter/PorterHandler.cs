using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.Porter;
using ZGTR_PorterAlgorithmApp;
using ZGTR_SpellingCheckerWPFApp;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.PorterHandler
{
    public class PorterHandler
    {
        private MainWindow MainWindow;
        private String _tempInputFilePath1 = "tempInputFile1.txt";
        private String _tempInputFilePath2 = "tempInputFile2.txt";
        private string _tempOutputFilePath = "tempOutputFile.txt";
        private String _currentStemmedText;

        public PorterHandler(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
        }

        public void Stem()
        {
            File.WriteAllText(_tempInputFilePath1, this.MainWindow.tbTxPArea1.Text);
            _currentStemmedText = PorterStemmer.GetStemmedTextForFile(_tempInputFilePath1);
            this.MainWindow.tbTxPArea2.Text = _currentStemmedText;
        }

        public void ClearAreaInput()
        {
            this.MainWindow.tbTxPArea1.Clear();
        }

        public void ClearAreaOutput()
        {
            this.MainWindow.tbTxPArea2.Clear();
        }

        private void WriteToFile(string path, string text)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(path);
                streamWriter.Write(text);
                streamWriter.Close();
            }
            catch (Exception)
            {
            }
        }

        public void ColorifyStems()
        {
            this.MainWindow.spTxPWords.Children.Clear();
            this.MainWindow.spTxPStems.Children.Clear();
            WriteToFile(_tempInputFilePath2, this.MainWindow.tbTxPArea1.Text);
            WriteToFile(_tempOutputFilePath, this.MainWindow.tbTxPArea2.Text);
            List<String> ls1 = HelperModule.CrackTextToWords(_tempInputFilePath2);
            List<String> ls2 = HelperModule.CrackTextToWords(_tempOutputFilePath);
            List<TextBlock> tbList = HelperModule.ColorifyStemsFromOrigins(ls1, ls2);
            for (int i = 0; i < ls1.Count; i++)
            {
                TextBlock tb = new TextBlock();
                tb.Text = ls1[i];
                tb.FontFamily = new FontFamily("Gill Sans MT");
                tb.FontSize = 14;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.Foreground = Brushes.White;
                tb.Height = 20;
                this.MainWindow.spTxPWords.Children.Add(tb);
            }
            foreach (var textBlock in tbList)
            {
                this.MainWindow.spTxPStems.Children.Add(textBlock);
            }
        }

        public void UploadTextFileInput()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog()
                {
                    Filter = "Text Files(*.txt)|*.txt"
                };
                if (dialog.ShowDialog() == true)
                {
                    this.MainWindow.tbTxPArea1.Text = File.ReadAllText(dialog.FileName);
                }
            }
            catch (Exception)
            {
            }
        }

        public void UploadExcelFile()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    var listOfWords = ExcelFileManager.ReadColumnInExcelFile(dialog.FileName, "Sheet1", "A");
                    foreach (var word in listOfWords)
                    {
                        this.MainWindow.tbTxPArea1.Text += word + Environment.NewLine;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public void ExportToExcelFile()
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    List<string> wordsOutput = GetListOfWordsOutput();
                    ExcelFileManager.WriteToColumnInExcelFile(dialog.FileName, "Sheet1", "A", "Results", wordsOutput);
                }
            }
            catch (Exception)
            {
            }
        }

        private List<string> GetListOfWordsOutput()
        {
            return this.MainWindow.tbTxPArea2.Text.Split('\n').ToList();
        }
    }
}
