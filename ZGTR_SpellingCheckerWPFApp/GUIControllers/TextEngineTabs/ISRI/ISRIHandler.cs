using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.PorterHandler;
using ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.ISRI;
using ZGTR_PorterAlgorithmApp;
using ZGTR_SpellingCheckerWPFApp;

using Excel = Microsoft.Office.Interop.Excel;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.ISRI
{
    public class ISRIHandler
    {
        private MainWindow MainWindow;

        public ISRIHandler(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
        }

        public void Stem()
        {
            string word = MainWindow.tbTxIArea1.Text;
            if (word.Contains(','))
            {
                word = word.Replace(',', ' ');
            }
            if (word.Contains(':'))
            {
                word = word.Replace(':', ' ');
            }
            if (word.Contains('؟'))
            {
                word = word.Replace('؟', ' ');
            }
            string[] words = word.Split(' ');
            ISRIAlgo isri = new ISRIAlgo();
            string[] stm = new string[words.Length];
            int i = 0;
            foreach (string s in words)
            {
                stm[i] = isri.Stem(s);
                i++;
            }
            foreach (string s in stm)
            {
                this.MainWindow.tbTxIArea2.Text += s + ' ';
            }
        }

        public void Colorify()
        {
            this.MainWindow.spTxIWords.Children.Clear();
            this.MainWindow.spTxIStems.Children.Clear();
            //WriteToFile(_tempInputFilePath2, this.MainWindow.tbTxPArea1.Text);
            //WriteToFile(_tempOutputFilePath, this.MainWindow.tbTxPArea2.Text);
            //List<String> ls1 = HelperModule.CrackTextToWords(_tempInputFilePath2);
            //List<String> ls2 = HelperModule.CrackTextToWords(_tempOutputFilePath);
            List<String> ls1 = this.MainWindow.tbTxIArea1.Text.Split(new char[] { ' ', ',', '?', '!' }).ToList();
            List<String> ls2 = this.MainWindow.tbTxIArea2.Text.Split(new char[] { ' ', ',', '?', '!' }).ToList();
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
                this.MainWindow.spTxIWords.Children.Add(tb);
            }
            foreach (var textBlock in tbList)
            {
                this.MainWindow.spTxIStems.Children.Add(textBlock);
            }
        }

        public void ExportExcelFile()
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    List<string> wordsOutput = this.MainWindow.tbTxIArea2.Text.Split(new char[] { ' ', ',', '?', '!' }).ToList();
                    ExcelFileManager.WriteToColumnInExcelFile(dialog.FileName, "Sheet1", "A", "Results", wordsOutput);
                }
            }
            catch (Exception)
            {
            }
        }

        public void ClearArea2()
        {
            this.MainWindow.tbTxIArea2.Clear();
        }

        public void UploadExcelFile()
        {
            try
            {
                List<string> words = new List<string>();
                OpenFileDialog dialog = new OpenFileDialog();

                dialog.FileName = "*.xls";

                if (dialog.ShowDialog() == true)
                {

                    Excel.Application xlApp = new Excel.Application();
                    Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(dialog.FileName);
                    Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                    Excel.Range xlRange = xlWorksheet.UsedRange;

                    int rowCount = xlRange.Rows.Count;
                    int colCount = xlRange.Columns.Count;

                    for (int i = 1; i <= rowCount; i++)
                    {
                        for (int j = 1; j <= colCount; j++)
                        {
                            //MessageBox.Show(xlRange.Cells[i, j].Value2.ToString());
                            this.MainWindow.tbTxIArea1.Text += xlRange.Cells[i, j].Value2.ToString() + ' ';
                            words.Add(xlRange.Cells[i, j].Value2.ToString());

                        }
                    }

                }
            }
            catch (Exception)
            {
            }
        }

        public void ClearArea1()
        {
            this.MainWindow.tbTxIArea1.Clear();
        }
    }
}
