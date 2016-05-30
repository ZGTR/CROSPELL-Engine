using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.SpellingCheckerEngine.Algorithms;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.TextEngineTabs.SpellCheckMe
{
    public class CyberAllTextHandler
    {
        private MainWindow MainWindow;
        private readonly string _inputText;
        private readonly CyberSpell _cyberCurrent;
        private readonly bool _isArabic;

        public CyberAllTextHandler(MainWindow mainWindow, string inputText, CyberSpell cyberCurrent)
        {
            this.MainWindow = mainWindow;
            _inputText = inputText;
            _cyberCurrent = cyberCurrent;
            SetTextBlockText(_inputText, false);
        }

        public void SetTextBlockText(string inputText, bool isKeyMap)
        {
            this.MainWindow.tblCTWTextArea.Text = String.Empty;
            string[] words = inputText.Split(new char[] { ' ', '.', '!', '?', ',' });
            try
            {
                for (int i = 0; i < words.Count(); i++)
                {
                    var currentInputWord = words[i];
                    List<SuggestItem> listOfPredictedWords =
                        _cyberCurrent.GetCorrectSpellSuggestionsForWord(currentInputWord,
                                                                        isKeyMap);
                    if (listOfPredictedWords.Count > 0)
                    {
                        if (listOfPredictedWords[0].Term == currentInputWord)
                        {
                            this.MainWindow.tblCTWTextArea.Inlines.Add(currentInputWord + " ");
                        }
                        else
                        {
                            ComboBox comboBoxWords = GetNewSuggComboBox(currentInputWord, listOfPredictedWords);
                            this.MainWindow.tblCTWTextArea.Inlines.Add(comboBoxWords);
                            this.MainWindow.tblCTWTextArea.Inlines.Add(" ");
                        }
                    }
                    else
                    {
                        this.MainWindow.tblCTWTextArea.Inlines.Add(currentInputWord + " ");
                    }
                }
                //if (isArabic)
                //{
                //    var reversedInline = new List<Inline> (this.MainWindow.tblCTWTextArea.Inlines);
                //    reversedInline.Reverse();
                //    this.MainWindow.tblCTWTextArea.Inlines.Clear();
                //    this.MainWindow.tblCTWTextArea.Inlines.AddRange(reversedInline);
                //}
            }
            catch (Exception)
            {
            }
        }

        private ComboBox GetNewSuggComboBox(string currentInputWord, List<SuggestItem> listOfPredictedWords)
        {
            ComboBox comboBoxWords = new ComboBox();
            Button bAdd = new Button();
            bAdd.Height = 22;
            bAdd.Content = "Add " + currentInputWord;
            bAdd.Click += new RoutedEventHandler(bAdd_Click);
            comboBoxWords.Items.Add(bAdd);
            for (int i = 0; i < listOfPredictedWords.Count; i++)
            {
                comboBoxWords.Items.Add(listOfPredictedWords[i].Term);
                comboBoxWords.Height = 18;
                comboBoxWords.FontSize = 12;
                comboBoxWords.FontFamily = new FontFamily("Gill Sans MT");
                comboBoxWords.VerticalAlignment = VerticalAlignment.Center;
            }
            comboBoxWords.SelectedIndex = 1;
            return comboBoxWords;
        }

        void bAdd_Click(object sender, RoutedEventArgs e)
        {
            Button bSender = sender as Button;
            string wordWrongToAdd = ((bSender.Content) as String).Split(' ')[1];
            _cyberCurrent.CreateDictionaryEntry(wordWrongToAdd, "");
            SetTextBlockText(_inputText, false);
        }

        public string GetAllNewCorrectedText()
        {
            string newText = String.Empty;
            for (int i = 0; i < this.MainWindow.tblCTWTextArea.Inlines.Count; i++)
            {
                var currentInline = this.MainWindow.tblCTWTextArea.Inlines.ElementAt(i);
                if (currentInline is System.Windows.Documents.InlineUIContainer)
                {
                    newText += ((currentInline as System.Windows.Documents.InlineUIContainer).Child as ComboBox)
                        .SelectedItem as String;
                }
                else
                {
                    newText += (currentInline as System.Windows.Documents.Run).Text;
                }
            }
            return newText;
        }
    }
}
