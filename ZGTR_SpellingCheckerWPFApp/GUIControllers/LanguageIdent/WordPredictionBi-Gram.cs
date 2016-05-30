using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZGTR_CROSPELLSpellingCheckerLib.LangIdent;
using ZGTR_SpellingCheckerWPFApp;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.LanguageIdentification
{
    public class WordPredictionBi_Gram
    {
        private MainWindow MainWindow;
        public WordPredictionBi_Gram(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            InitializeWords();
        }
        List<Language> _languages = new List<Language>();
        public void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            TextBox txtB = ((TextBox)sender);
            if (txtB.Text != "")
            {

                this.MainWindow.lbWP.Items.Clear();

                string strText = this.MainWindow.tbInputAll.Text;
                if (strText.EndsWith(" "))
                    strText = strText.Remove(strText.Length - 1);
                string[] textWords = strText.Split(' ');
                string[] str2 = new string[textWords.Length];
                for (int i = 0; i < str2.Length; i++)
                {
                    string s = CleanString(textWords[i]);
                    if (s != "" || s != " ")
                        str2[i] = s;
                }
                Dictionary<string, int> predictedWords = FindingPredictedWords(str2);
                try
                {
                    if (predictedWords.Count >= 20)
                    {
                        int i = 0;
                        foreach (KeyValuePair<string, int> predictedWord in predictedWords)
                        {
                            this.MainWindow.lbWP.Items.Add(predictedWord.Key);
                            i++;
                            if (i == 20)
                                break;
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, int> predictedWord in predictedWords)
                        {
                            this.MainWindow.lbWP.Items.Add(predictedWord.Key);
                        }
                    }
                }
                catch (Exception)
                {
                    
                    
                }
            }
        }

        private Dictionary<string,int> FindingPredictedWords(string[] textWords)
        {
            Dictionary<string, int> Predictedwords = null;
            foreach (Language language in _languages)
            {
                Predictedwords = language.LanguageBiGramWordPrediction(textWords);
            }
            return Predictedwords;
        }

        void InitializeWords()
        {
            _languages.Add(new Language("عربي"));

            string[] txtFiles = Directory.GetFiles("Common words\\biGram", "*.txt");
            foreach (Language language in _languages)
            {
                foreach (string txtFileName in txtFiles)
                {
                    string[] str = txtFileName.Split('\\');
                    string[] txtName = str[str.Length - 1].Split('.');
                    if (language.LanguageName == txtName[0])
                    {
                        language.IntializeBiGramWords(txtFileName, false);
                        break;
                    }
                }
            }
        }

        
        static string CleanString(string word)
        {
            List<string> numbers = new List<string>();
            for (int i = 0; i < 9; i++)
            {
                numbers.Add(i.ToString());
            }
            while (word.Contains('-'))
            {
                word = word.Remove(word.IndexOf('-'), 1);
            }
            while (word.Contains('\u060C'))
            {
                word = word.Remove(word.IndexOf('\u060C'), 1);
            }
            while (word.Contains(','))
            {
                word = word.Remove(word.IndexOf(','), 1);
            }
            while (word.Contains(':'))
            {
                word = word.Remove(word.IndexOf(':'), 1);
            }
            while (word.Contains('؟'))
            {
                word = word.Remove(word.IndexOf('؟'), 1);
            }
            while (word.Contains('('))
            {
                word = word.Remove(word.IndexOf('('), 1);
            }
            while (word.Contains(')'))
            {
                word = word.Remove(word.IndexOf(')'), 1);
            }
            while (word.Contains('.'))
            {
                word = word.Remove(word.IndexOf('.'), 1);
            }
            while (word.Contains('!'))
            {
                word = word.Remove(word.IndexOf('!'), 1);
            }
            //foreach (var number in numbers)
            //{
            //    while (word.Contains(number))
            //    {
            //        word = word.Remove(word.IndexOf(number), 1);
            //    }
            //}
            return word;
        }
    }
}
