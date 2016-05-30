using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;
using ZGTR_SpellingCheckerWPFApp;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.LanguageIdentification
{
   public class ArabicEnglishDictionary
    {
        private MainWindow MainWindow;
        Dictionary<string, string> wordDictionary = new Dictionary<string, string>();
        public List<String> Synonyms { get; set; }
        public List<String> Antonyms { get; set; }
        public List<String> RelatedWords { get; set; }
        public ArabicEnglishDictionary(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            intialize();
        }
        void intialize()
        {
            StreamReader corpusPage = new StreamReader("dictionary.txt");
            string sentences = string.Empty;
            while ((sentences = corpusPage.ReadLine()) != null)
            {
                string[] s1 = sentences.Split('\t');
                
                if (!wordDictionary.ContainsKey(s1[0]))
                {
                    wordDictionary.Add(s1[0], s1[1]);

                }
            }
        }

        public void button1_Click()
        {
            string word = MainWindow.tbDicInput.Text;

            if (wordDictionary.ContainsKey(word))
            {
                string englishWord = wordDictionary[word];
                Thesaurus(englishWord);
                MainWindow.tbDicOutput.Text = word + ": " + englishWord;
                if (Synonyms != null)
                {
                    if (Synonyms.Count != 0)
                    {
                        MainWindow.tbDicOutput.Text += "\r\n" + "[SYNONYMS]" + "\r\n";
                        foreach (string synonym in Synonyms)
                        {
                            MainWindow.tbDicOutput.Text += synonym + ", ";
                        }
                    }

                    if (Antonyms.Count != 0)
                    {
                        MainWindow.tbDicOutput.Text += "\r\n" + "[ANTONYMS]" + "\r\n";
                        foreach (string Antonym in Antonyms)
                        {
                            MainWindow.tbDicOutput.Text += Antonym + ", ";
                        }
                    }

                    if (RelatedWords.Count != 0)
                    {
                        MainWindow.tbDicOutput.Text += "\r\n" + "[RELATED WORDS]" + "\r\n";
                        foreach (string RelatedWord in RelatedWords)
                        {
                            MainWindow.tbDicOutput.Text += RelatedWord + ", ";
                        }

                    }
                }
            }
        }

        static string CleanString(string word)
        {
            List<string> numbers = new List<string>();
            for (int i = 0; i < 10; i++)
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
            //if(word[0]=='و')
            //{
            //    word = word.Remove(word.IndexOf('و'), 1);
            //}
            foreach (var number in numbers)
            {
                while (word.Contains(number))
                {
                    word = word.Remove(word.IndexOf(number), 1);
                }
            }
            return word;
        }

        public void Thesaurus(String baseWord)
        {
            var _apiKey = "ffb0ba9c5992d768433aeba36f7a577f";
            try
            {
                var xmlSource = XDocument.Load(@"http://words.bighugelabs.com/api/2/" + _apiKey + "/" + baseWord + "/xml");
                var _synonymQuery = from _word in xmlSource.Descendants("w")
                                    where (string)_word.Attribute("r").Value == "syn"
                                    select (string)_word;
                Synonyms = _synonymQuery.ToList();
                var _antonymQuery = from _word in xmlSource.Descendants("w")
                                    where (string)_word.Attribute("r").Value == "ant"
                                    select (string)_word;
                Antonyms = _antonymQuery.ToList();

                var _relatedQuery = from _word in xmlSource.Descendants("w")
                                    where (string)_word.Attribute("r").Value == "rel"
                                    select (string)_word;
                RelatedWords = _relatedQuery.ToList();
            }
            catch (Exception)
            {}

        }

    }
}
