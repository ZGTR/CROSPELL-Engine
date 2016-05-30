using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZGTR_CROSPELLSpellingCheckerLib.LangIdent
{
    public class Language
    {
        public string LanguageName { private set; get; }
        public int LanguageWordCounter {  set; get; }
        public int LanguageBiGramWordCounter { private set; get; }
        public List<string> LanguageWordsList { private set; get; }
        public Dictionary<string,int> LanguageBiGramWordsDictionary { private set; get; }
        
        public List<string> WordsList {  set; get; }
        public List<string> WordsBiGramList { set; get; }
        public List<string> WordsTriGramList { private set; get; }
        public Dictionary<string, int> LanguageTriGramWordsDictionary { private set; get; }
        public List<string> WordsFourGramList { private set; get; }
        public Dictionary<string, int> LanguageFourGramWordsDictionary { private set; get; }
        public Dictionary<string, List<string >> SpecializedWords { private set; get; }

        public Language(string languageName)
        {
            LanguageName = languageName;
            LanguageWordCounter = 0;
            LanguageWordsList = new List<string>();
            WordsBiGramList=new List<string>();
            WordsTriGramList=new List<string>();
            LanguageBiGramWordsDictionary=new Dictionary<string,int>();
            LanguageTriGramWordsDictionary=new Dictionary<string, int>();
            WordsFourGramList = new List<string>();
            LanguageFourGramWordsDictionary = new Dictionary<string, int>();
            WordsList=new List<string>();
            SpecializedWords=new Dictionary<string, List<string>>();
        }
        public void IntializeBiGramWords(string wordsFilePath,bool flag)
        {
            string line;
            StreamReader readFile =
                   new StreamReader(wordsFilePath);
            while ((line = readFile.ReadLine()) != null)
            {
                string[] str = line.Split('\t');
                if(!flag)
                {
                    if (str[1] != "1" && str[1] != "2" && str[1] != "3")
                        LanguageBiGramWordsDictionary.Add(str[0], Int32.Parse(str[1]));
                }
                else
                {
                    LanguageBiGramWordsDictionary.Add(str[0], Int32.Parse(str[1]));
                }
                
            }
           
            readFile.Close();
        }
        public void IntializeWords(string wordsFilePath)
        {
            string line;
            StreamReader readFile =
                   new StreamReader(wordsFilePath, Encoding.GetEncoding("Windows-1256"));
            while ((line = readFile.ReadLine()) != null)
            {
                string[] str = line.Split('\t');
                LanguageWordsList.Add(str[0]);
            }
            LanguageWordsList.Reverse();
            readFile.Close();
        }
        public List<string > IntializeSpecializedWords(string wordsFilePath)
        {
            string line;
            List<string > words=new List<string>();
            StreamReader readFile =
                   new StreamReader(wordsFilePath);
            while ((line = readFile.ReadLine()) != null)
            {
                string[] str = line.Split('\t');
                words.Add(str[1]);
            }
            words.Reverse();
            readFile.Close();
            return words;
        }
        public bool TestLanguage(string[] testScript)
        {
            List<string> wordsStringList = new List<string>();
            bool flag = false;
            for (int i = 0; i < testScript.Length; i++)
            {
                if (LanguageWordsList.Contains(testScript[i]) && (!wordsStringList.Contains(testScript[i])) && (!WordsList.Contains(testScript[i])))
                {
                    LanguageWordCounter += 1;
                    wordsStringList.Add(testScript[i]);
                }
            }

            foreach (string s in wordsStringList)
            {
                  WordsList.Add( s);
                  flag = true;
            }

            return flag;

        }
        public Dictionary<string,int> LanguageBiGramWordPrediction(string[] testScripto)
        {
            string str;
            Dictionary<string,int> wordsStringList = new Dictionary<string,int>();
            string[] testScript = null;
            for (int i = 0; i < testScripto.Length; i++)
            {
                int k = 0;
                testScript = new string[testScripto.Length - i];
                for (int j = i; j < testScripto.Length; j++)
                {
                    testScript[k] = testScripto[j];
                    k++;
                }
                if (testScript.Length > 1)
                {
                    str = testScript[testScript.Length - 2] + ' ' + testScript[testScript.Length - 1];
                }
                else
                {
                    str = testScript[0];
                }
                foreach (KeyValuePair<string, int> keyValuePair in LanguageBiGramWordsDictionary)
                {
                    string CorpusWord = keyValuePair.Key;
                    if (CorpusWord.StartsWith(str)&& (!wordsStringList.ContainsKey(CorpusWord)))
                    {
                        wordsStringList.Add(CorpusWord,1);
                    }
                }
            }
            return wordsStringList;
        }
        public bool TestLanguageBiGram(string[] testScript)
        {
            List<string> wordsStringList = new List<string>();
            bool flag = false;
            for (int i = 0; i < testScript.Length-1; i++)
            {
                string str = testScript[i] + ' ' + testScript[i + 1];
                if (LanguageBiGramWordsDictionary.ContainsKey(str) && (!wordsStringList.Contains(str)) && (!WordsBiGramList.Contains(str)))
                {
                    LanguageWordCounter += 1;
                    wordsStringList.Add(str);
                }
            }

            foreach (string s in wordsStringList)
            {
                WordsBiGramList.Add(s);
                flag = true;
            }

            return flag;

        }
        public Dictionary<string ,int> testSpecializedWordsInLanguage(string []testScript)
        {
            List<string> list = new List<string>(SpecializedWords.Keys);
            Dictionary<string ,int >dictionary=new Dictionary<string, int>();
            

            foreach (string field in list)
            {
                int counter = 1;
                for (int i = 0; i < testScript.Length; i++)
                {
                    if (SpecializedWords[field].Contains(testScript[i]))
                    {
                        if(!dictionary.ContainsKey(field))
                            dictionary.Add(field, 1);
                        else
                        {
                            counter=counter+1;
                            dictionary[field] = counter;
                        }
                        
                    }
                }
            }
            return dictionary;
        }

        public Dictionary<string, int> LanguagetriGramWordPrediction(string[] testScripto)
        {
            string str;
            Dictionary<string, int> wordsStringList = new Dictionary<string, int>();
            string[] testScript = null;
            
            for (int i = 0; i < testScripto.Length; i++)
            {
                int k = 0;
                testScript = new string[testScripto.Length - i];
                for (int j = i; j < testScripto.Length; j++)
                {
                    testScript[k] = testScripto[j];
                    k++;
                }

                if (testScript.Length > 2)
                {
                    str = testScript[testScript.Length - 3] + ' ' + testScript[testScript.Length - 2] + ' ' +
                          testScript[testScript.Length - 1];
                }
                else
                {
                    if (testScript.Length == 2)
                    {
                        str = testScript[testScript.Length - 2] + ' ' + testScript[testScript.Length - 1];
                    }
                    else
                    {
                        str = testScript[0];
                    }
                }

                foreach (KeyValuePair<string, int> keyValuePair in LanguageTriGramWordsDictionary)
                {
                    string CorpusWord = keyValuePair.Key;
                    if (CorpusWord.StartsWith(str) && (!wordsStringList.ContainsKey(CorpusWord)))
                    {
                        wordsStringList.Add(CorpusWord,1);
                    }
                }
            }
            return wordsStringList;
        }

        public void IntializetriGramWords(string txtFileName,bool flag)
        {
            string line;
            StreamReader readFile =
                   new StreamReader(txtFileName);
            while ((line = readFile.ReadLine()) != null)
            {
                string[] str = line.Split('\t');
                if (!flag)
                {
                    if (str[1] != "1" && str[1] != "2" && str[1] != "3" )
                    {
                        LanguageTriGramWordsDictionary.Add(str[0], Int32.Parse(str[1]));
                    }
                }
                else
                {
                    LanguageTriGramWordsDictionary.Add(str[0], Int32.Parse(str[1]));
                }
            }

            readFile.Close();
        }
        public List<string> LanguageFourGramWordPrediction(string[] testScript)
        {
            string str;
            List<string> wordsStringList = new List<string>();
            if (testScript.Length > 3)
            {
                str = testScript[testScript.Length - 4] + ' ' + testScript[testScript.Length - 3] + ' ' +
                      testScript[testScript.Length - 2] + ' ' + testScript[testScript.Length - 1];
            }
            else
            {
                if (testScript.Length == 3)
                {
                    str = testScript[testScript.Length - 3] + ' ' + testScript[testScript.Length - 2] + ' ' +
                          testScript[testScript.Length - 1];
                }
                else
                {
                    if (testScript.Length == 2)
                    {
                        str = testScript[testScript.Length - 2] + ' ' + testScript[testScript.Length - 1];
                    }
                    else
                    {
                        str = testScript[0];
                    }
                }
            }
            foreach (KeyValuePair<string, int> keyValuePair in LanguageFourGramWordsDictionary)
            {
                string CorpusWord = keyValuePair.Key;
                if (CorpusWord.StartsWith(str))
                {
                    wordsStringList.Add(CorpusWord);
                }
            }
            return wordsStringList;
        }

        public void IntializeFourGramWords(string txtFileName,bool flag)
        {
            string line;
            StreamReader readFile =
                   new StreamReader(txtFileName);
            while ((line = readFile.ReadLine()) != null)
            {
                string[] str = line.Split('\t');
                if (!flag)
                {
                    if (str[1] != "1")
                    {
                        LanguageFourGramWordsDictionary.Add(str[0], Int32.Parse(str[1]));
                    } 
                }
                else
                {
                    LanguageFourGramWordsDictionary.Add(str[0], Int32.Parse(str[1]));
                }
                
            }

            readFile.Close();
        }
    }
}
