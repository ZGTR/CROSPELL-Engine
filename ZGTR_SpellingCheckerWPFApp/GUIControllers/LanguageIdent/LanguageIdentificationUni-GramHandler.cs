using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Win32;
using ZGTR_CROSPELLSpellingCheckerLib.LangIdent;
using ZGTR_SpellingCheckerWPFApp;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.LanguageIdentification
{
    public class LanguageIdentificationUni_GramHandler
    {
        private MainWindow MainWindow;
        public LanguageIdentificationUni_GramHandler(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            InitializeWords();
        }
        List<Language> _languages = new List<Language>();
        void InitializeWords()
        {
            _languages.Add(new Language("عربي"));
            _languages.Add(new Language("فارسي"));
            _languages.Add(new Language("أوردو"));
            _languages.Add(new Language("انجليزي"));
            string[] txtFiles = Directory.GetFiles("Common words", "*.txt");
            foreach (Language language in _languages)
            {
                foreach (string txtFileName in txtFiles)
                {
                    string[] str = txtFileName.Split('\\');
                    string[] txtName = str[1].Split('.');
                    if (language.LanguageName == txtName[0])
                    {
                        language.IntializeWords(txtFileName);
                        break;
                    }
                }
            }
        }
        public void buttonCheck_Click()
        {
            this.MainWindow.lbLIBasicLang.Items.Clear();
            this.MainWindow.lbLIOtherLang.Items.Clear();
            string strText = this.MainWindow.tbInputAll.Text;
            string[] textWords = strText.Split(' ');
            FindingBasicLanguage(textWords);
            List<string> OtherLangList = TestingMultipleLaguages(_languages[0]);
            foreach (string s in OtherLangList)
            {
                this.MainWindow.lbLIOtherLang.Items.Add(s + ' ');
            }
            this.MainWindow.lbLIBasicLang.Items.Add(  _languages[0].LanguageName);
        }
        public void buttonUpload_Click()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text Files(*.txt)|*.txt";
            if (dialog.ShowDialog() == true)
            {
                string str = File.ReadAllText(dialog.FileName, Encoding.Default);
                this.MainWindow.tbInputAll.Text = str;
            }
        }

        public void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            TextBox txtB = ((TextBox)sender);
            if (txtB.Text != "")
            {
                char str = txtB.Text[txtB.Text.Length - 1];
                if (str == ',' || str == '.' || str == '!' || str == '?' || str == ' ')
                {
                    this.MainWindow.lbLIBasicLang.Items.Clear();
                    this.MainWindow.lbLIOtherLang.Items.Clear();
                    string strText = this.MainWindow.tbInputAll.Text;
                    strText = CleanString(strText);
                    string[] textWords = strText.Split(' ');
                    FindingBasicLanguage(textWords);
                    List<string> OtherLangList = TestingMultipleLaguages(_languages[0]);
                    foreach (string s in OtherLangList)
                    {
                        this.MainWindow.lbLIOtherLang.Items.Add(s + ' ');
                    }
                    this.MainWindow.lbLIBasicLang.Items.Add( _languages[0].LanguageName);
                }
            }
        }
        public void CleanLists()
        {
            foreach (Language language in _languages)
            {
                language.WordsList=new List<string>();
                language.LanguageWordCounter = 0;
            }
        }

        public void FindingBasicLanguage(string[] textWords)
        {
            List<bool> ifChanged = new List<bool>();
            foreach (Language language in _languages)
            {
                ifChanged.Add(language.TestLanguage(textWords));
            }
            if (ifChanged.Contains(true))
            {
                _languages.Sort(delegate(Language l1, Language l2)
                {
                    return l1.LanguageWordCounter.CompareTo(l2.LanguageWordCounter);
                });
                _languages.Reverse();
            }

        }

         List<string> TestingMultipleLaguages(Language currentLanguage)
        {
            List<string> otherLanguages = new List<string>();
            foreach (Language language in _languages)
            {
                if (language != currentLanguage)
                {
                    foreach (string s in language.WordsList)
                    {
                        if (!currentLanguage.WordsList.Contains(s))
                        {
                            otherLanguages.Add(language.LanguageName);
                            break;
                        }

                    }
                }
            }
            return otherLanguages;

        }
        public static string CleanString(string word)
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
            foreach (var number in numbers)
            {
                while (word.Contains(number))
                {
                    word = word.Remove(word.IndexOf(number), 1);
                }
            }
            return word;
        }

    }
}
