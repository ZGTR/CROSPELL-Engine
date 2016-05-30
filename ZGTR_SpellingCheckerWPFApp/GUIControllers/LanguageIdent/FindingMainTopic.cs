using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using ZGTR_CROSPELLSpellingCheckerLib.LangIdent;
using ZGTR_SpellingCheckerWPFApp;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.LanguageIdentification
{
    public class FindingMainTopic
    {
         private MainWindow MainWindow;
        public FindingMainTopic(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            InitializeWords();
        }
        Language _language;
        public void buttonCheck_Click()
        {
            this.MainWindow.lbTP.Items.Clear();

            string strText = this.MainWindow.tbInputAll.Text;
            string[] textWords = strText.Split(' ');
            Dictionary<string, int> fields = FindingTopic(textWords);
            foreach (KeyValuePair<string, int> field in fields)
            {
                StringBuilder fieldString = new StringBuilder(field.Key);
                string firstLetter = fieldString[0].ToString().ToUpper();
                //fieldString=fieldString.Replace(fieldString[0],firstLetter[0]);
                fieldString[0] = Convert.ToChar(firstLetter);
                this.MainWindow.lbTP.Items.Add("[" + field.Value + "]" + ' ' + fieldString);    
            }
            
        }
        Dictionary<string, int> FindingTopic(string[] textWords)
        {
            Dictionary<string, int> d = _language.testSpecializedWordsInLanguage(textWords);
            var orderedDictionary = d.OrderBy(x => x.Value).Reverse().ToDictionary(x => x.Key, x => x.Value);
            return orderedDictionary;
        }
        void InitializeWords()
        {
            _language = new Language("عربي");

            string[] txtFiles = Directory.GetFiles("Common words\\ClassifyingWords", "*.txt");
            foreach (string txtFileName in txtFiles)
            {

                string[] str = txtFileName.Split('\\');
                string[] txtName = str[str.Length - 1].Split('.');
                _language.SpecializedWords[txtName[0]] = _language.IntializeSpecializedWords(txtFileName);

            }
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
    }
}
