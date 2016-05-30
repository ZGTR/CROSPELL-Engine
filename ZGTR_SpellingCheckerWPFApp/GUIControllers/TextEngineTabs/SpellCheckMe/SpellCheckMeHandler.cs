using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using SpellingChecker.ImagesProcessingEngine;
using SpellingChecker.Keyboard;
using SpellingChecker.SpellingCheckerEngine;
using SpellingChecker.SpellingCheckerEngine.UserPref;
using ZGTR_CROSPELLSpellingCheckerApp;
using ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.TextEngineTabs.SpellCheckMe;
using ZGTR_CROSPELLSpellingCheckerLib.HelperModules;
using ZGTR_CROSPELLSpellingCheckerLib.SpellingCheckerEngine.Algorithms;
using ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.SpellingCheckerEngine.Algorithms;

namespace ZGTR_SpellingCheckerWPFApp.GUIControllers
{
    public class SpellCheckMeHandler
    {
        private MainWindow MainWindow;
        private CyberSpell _CyberArabic;
        private CyberSpell _CyberEnglish;
        private CyberSpell _CyberCurrent;
        private UserPreferenceHandler _userPreferenceHandler;
        private SuggestionsOrdering _currentSuggestionsOrdering;
        private CyberAllTextHandler _cyberAllTextHandler;
        private static int UserPrefWeight = 50;
        
        public SpellCheckMeHandler(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            _currentSuggestionsOrdering = SuggestionsOrdering.ByCorups;
            _userPreferenceHandler = new UserPreferenceHandler(@"Databases\UserPreferenceDb\UserPreferenceDb.xml");
        }

        public void InitEnglishCyberSpell()
        {
            bool isFast = GetSpeedConfig();
            _CyberEnglish = new CyberSpell("EnglishCorpus.txt", "", KeyboardLanguage.English, isFast);
            _CyberCurrent = _CyberEnglish;
        }

        public void InitArabicCyberSpell()
        {
            bool isFast = GetSpeedConfig();
            _CyberArabic = new CyberSpell("", "", KeyboardLanguage.Arabic, isFast);
            _CyberCurrent = _CyberArabic;
        }

        public void EnterCorpus()
        {
            bool isFast = GetSpeedConfig();
            OpenFileDialog dialog = new OpenFileDialog();
            string corpusPath = String.Empty;
            if ((bool) dialog.ShowDialog())
            {
                corpusPath = dialog.FileName;
            }
            if (corpusPath != String.Empty)
            {
                _CyberCurrent = new CyberSpell(corpusPath, "", KeyboardLanguage.English, isFast);
            }
        }

        private bool GetSpeedConfig()
        {
            bool isFast = true;
            if ((bool)this.MainWindow.rbTxSMNormalSpeed.IsChecked)
            {
                isFast = false;
            }
            return isFast;
        }

        public void ArabicChecked()
        {
            _CyberCurrent = _CyberArabic;
            this.MainWindow.tblCTWTextArea.TextAlignment = TextAlignment.Right;
        }

        public void EnglishChecked()
        {
            _CyberCurrent = _CyberEnglish;
            this.MainWindow.tblCTWTextArea.TextAlignment = TextAlignment.Left;
        }

        public void TextBoxTextChanged(bool isKeyMap = false)
        {
            try
            {
                var listBox = this.MainWindow.lbTxSMSuggestions;
                var tbInput = this.MainWindow.tbInputAll;
                if ((bool)this.MainWindow.cbTxSMEnableAutoCyber.IsChecked)
                {
                    _cyberAllTextHandler = new CyberAllTextHandler(this.MainWindow, tbInput.Text, _CyberCurrent);
                }
                if (tbInput.Text.ElementAt(tbInput.Text.Length - 1) == ' ')
                {
                    listBox.Items.Clear();
                    string lastWord = GetLastWordInInputText();
                    var suggestions = _CyberCurrent.GetCorrectSpellSuggestionsForWord(lastWord, isKeyMap);
                    if (suggestions.Count > 0)
                    {
                        if (suggestions[suggestions.Count - 1].Term == lastWord)
                        {
                            // the word is correct
                            return;
                        }
                        // the word is wrong
                        List<string> listFinal = GetOrderedSuggestions(lastWord, suggestions);
                        listFinal.ForEach(t => listBox.Items.Add(t));
                    }
                }
            }
            catch (Exception)
            {}
        }

        private List<string> GetOrderedSuggestions(string lastWord, List<SuggestItem> suggestions)
        {
            switch (_currentSuggestionsOrdering)
            {
                case SuggestionsOrdering.ByCorups:
                    return GetSuggestionsByCorups(lastWord, suggestions);
                    break;
                case SuggestionsOrdering.ByUserPref:
                    return GetSuggestionsByUserPref(lastWord, suggestions);
                    break;
                case SuggestionsOrdering.ByKeyMap:
                    return GetSuggestionsByCorups(lastWord, suggestions);
                    break;
                case SuggestionsOrdering.Scallar:
                    return GetSuggestionsByScallar(lastWord, suggestions);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private List<string> GetSuggestionsByCorups(string lastWord, List<SuggestItem> suggestions)
        {
            return suggestions.Select(t => GetTermToString(t.Count, t.Term)).ToList();
        }

        private List<string> GetSuggestionsByUserPref(string lastWord, List<SuggestItem> suggestions)
        {
            List<SuggestItem> listOfPreferedWords = GetListUserPreference(lastWord, suggestions);
            List<string> listFinal = GetListOfSuggestionsFinalByUserPref(listOfPreferedWords, suggestions);
            return listFinal;
        }

        private List<string> GetSuggestionsByScallar(string lastWord, List<SuggestItem> suggestions)
        {
            //int scaleVal = suggestions[suggestions.Count - 1].Count - suggestions[0].Count / suggestions.Count;
            List<SuggestItem> listOfPreferedWords = GetListUserPreference(lastWord, suggestions);
            List<string> listFinal = GetListOfSuggestionsFinalByScallar(listOfPreferedWords, suggestions);
            return listFinal;
        }

        public void ByCorups()
        {
            this._currentSuggestionsOrdering = SuggestionsOrdering.ByCorups;
            TextBoxTextChanged();
        }

        public void ByUserPref()
        {
            this._currentSuggestionsOrdering = SuggestionsOrdering.ByUserPref;
            TextBoxTextChanged();
        }

        public void ByKeyMap()
        {
            this._currentSuggestionsOrdering = SuggestionsOrdering.ByKeyMap;
            TextBoxTextChanged(true);
        }

        public void ScallarNoKM()
        {
            this._currentSuggestionsOrdering = SuggestionsOrdering.Scallar;
            TextBoxTextChanged();
        }

        public void ScallarKM()
        {
            this._currentSuggestionsOrdering = SuggestionsOrdering.Scallar;
            TextBoxTextChanged(true);
        }

        private string GetLastWordInInputText()
        {
            var tbInput = this.MainWindow.tbInputAll;
            string allText = tbInput.Text;
            string[] splits = allText.Trim().Split(new char[] { ' ', ',', '!', '?', '.' });
            string lastWord = splits[splits.Count() - 1];
            return lastWord;
        }

        private List<string> GetListOfSuggestionsFinalByScallar(List<SuggestItem> listUserPrefFinal,
                List<SuggestItem> suggestions)
        {
            var ListFinal = new List<string>();
            var ListFinalSuggestions = new List<SuggestItem>();
                //= listUserPrefFinal.Select(t => GetTermToString(t.Count, t.Term)).ToList();
            bool isNew = false;
            foreach (SuggestItem suggestItem in suggestions)
            {
                isNew = false;
                if (!listUserPrefFinal.Contains(suggestItem, new SuggestItemsComparer()))
                {
                    isNew = true;
                }
                else
                {
                    int count = (listUserPrefFinal.Where(t => t.Term == suggestItem.Term).Single() as SuggestItem).Count;
                    int totalUserPrefWeight = count*UserPrefWeight;
                    suggestItem.Count += totalUserPrefWeight;
                    ListFinalSuggestions.Add(suggestItem);
                }

                if (isNew || listUserPrefFinal.Count == 0)
                {
                    ListFinalSuggestions.Add(suggestItem);
                }
            }
            ListFinalSuggestions.Sort(new Comparison<SuggestItem>(CompareSuggestion));
            ListFinalSuggestions.Reverse();
            ListFinal = ListFinalSuggestions.Select(t => GetTermToString(t.Count, t.Term)).ToList();
            return ListFinal;
        }

        private List<string> GetListOfSuggestionsFinalByUserPref(List<SuggestItem> listUserPrefFinal, 
            List<SuggestItem> suggestions)
        {
            var ListFinal = listUserPrefFinal.Select(t => GetTermToString(t.Count, t.Term)).ToList();
            bool isNew = false;
            foreach (SuggestItem suggestItem in suggestions)
            {
                isNew = false;
                if (!listUserPrefFinal.Contains(suggestItem, new SuggestItemsComparer()))
                {
                    isNew = true;
                }
                if (isNew || listUserPrefFinal.Count == 0)
                {
                    ListFinal.Add(GetTermToString(suggestItem.Count, suggestItem.Term));
                }
            }
            return ListFinal;
        }

        private string GetTermToString(int count, string term)
        {
            return "[" + count + "]\t" + term;
        }

        private List<SuggestItem> GetListUserPreference(string lastWord, 
            List<SuggestItem> suggestions)
        {
            var listOfPreferedWords = new List<SuggestItem>();
            foreach (SuggestItem suggestItem in suggestions)
            {
                int prefCount = _userPreferenceHandler.GetUserPreference(lastWord, suggestItem.Term);
                if (prefCount > 0)
                {
                    // there's a user preference for this word
                    listOfPreferedWords.Add(new SuggestItem() { Count = prefCount, Term = suggestItem.Term });
                }
            }
            listOfPreferedWords.Sort(new Comparison<SuggestItem>(CompareSuggestion));
            listOfPreferedWords.Reverse();

            return listOfPreferedWords;
        }

        private int CompareSuggestion(SuggestItem x, SuggestItem y)
        {
            if (x.Count > y.Count)
            {
                return 1;
            }
            else
            {
                if (x.Count < y.Count)
                {
                    return -1;
                }
            }
            return 0;
        }

        public void KeyDown(KeyEventArgs e)
        {
            var listBox = this.MainWindow.lbTxSMSuggestions;
            var tbInput = this.MainWindow.tbInputAll;
            string allText = tbInput.Text;
            string newWord = String.Empty;
            if (e.Key == Key.Tab)
            {
                if (this.MainWindow.lbTxSMSuggestions.SelectedItem != null)
                    newWord = (this.MainWindow.lbTxSMSuggestions.SelectedItem as String);
                else
                {
                    try
                    {
                        newWord = (this.MainWindow.lbTxSMSuggestions.Items[0] as String);
                    }
                    catch (Exception)
                    {
                    }
                }
                if (newWord != String.Empty)
                {
                    if (newWord != null)
                        newWord = newWord.Trim().Split(']')[1].Split('\t')[1];
                    string lastWord = GetLastWordInInputText();
                    int indexLastWord = allText.LastIndexOf(lastWord);
                    allText = allText.Remove(indexLastWord);
                    if (newWord != null)
                    {
                        allText = allText.Insert(indexLastWord, newWord);
                        tbInput.Text = allText + " ";
                        _userPreferenceHandler.AddPreferenceToXMlDb(lastWord, newWord);
                    }
                    e.Handled = true;
                }
            }
            else
            {
                try
                {
                    if (e.Key == Key.Up)
                    {
                        if (listBox.SelectedIndex > 0)
                            listBox.SelectedIndex = listBox.SelectedIndex - 1;
                        e.Handled = true;
                    }
                    else
                    {
                        if (e.Key == Key.Down)
                        {
                            if (listBox.SelectedIndex < listBox.Items.Count - 1)
                                listBox.SelectedIndex = listBox.SelectedIndex + 1;
                            e.Handled = true;
                        }
                    }
                }
                catch (Exception)
                { }
            }
            tbInput.CaretIndex = tbInput.Text.Length;
        }

        //public void NotifyWrongWords()
        //{
        //    string allText = this.MainWindow.tbInputAll.Text;
        //}

        public void AddNewEntryToDict()
        {
            string newEntry = this.MainWindow.tbxTxSMNewDicEntry.Text;
            if (newEntry != String.Empty)
            {
                _CyberCurrent.CreateDictionaryEntry(newEntry, "");
            }
        }

        public void DoneEditing()
        {
            string textNew = _cyberAllTextHandler.GetAllNewCorrectedText();
            this.MainWindow.tbInputAll.Text = textNew;
        }

        public void CTWCorrectForAkeyMap()
        {
            var tbInput = this.MainWindow.tbInputAll;
            //_cyberAllTextHandler = new CyberAllTextHandler(this.MainWindow, tbInput.Text, _CyberCurrent);
            _cyberAllTextHandler.SetTextBlockText(tbInput.Text, true);
        }

        public void CyberAllText()
        {
            var tbInput = this.MainWindow.tbInputAll;
            _cyberAllTextHandler = new CyberAllTextHandler(this.MainWindow, tbInput.Text, _CyberCurrent);
        }
    }
}
