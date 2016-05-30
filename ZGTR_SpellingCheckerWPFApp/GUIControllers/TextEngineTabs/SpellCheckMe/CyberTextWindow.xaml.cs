using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.SpellingCheckerEngine.Algorithms;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.TextEngineTabs.SpellCheckMe
{
    /// <summary>
    /// Interaction logic for CyberTextWindow.xaml
    /// </summary>
    public partial class CyberTextWindow : Window
    {
        private MainWindow MainWindow;
        private readonly string _inputText;
        private readonly CyberSpell _cyberCurrent;

        public CyberTextWindow(MainWindow mainWindow, string inputText, CyberSpell cyberCurrent)
        {
            this.MainWindow = mainWindow;
            _inputText = inputText;
            _cyberCurrent = cyberCurrent;
            InitializeComponent();
            SetTextBlockText(_inputText, _cyberCurrent, false);
        }

        private void SetTextBlockText(string inputText, CyberSpell cyberCurrent, bool isKeyMap)
        {
            //Run newRun = new Run("sssssssssssssssss".ToString());
            //newRun.Background = Brushes.LightBlue;
            //this.tblCTWTextArea.Inlines.Add(newRun);
            //this.tblCTWTextArea.Inlines.Add(new ComboBox());
            this.tblCTWTextArea.Text = String.Empty;
            string[] words = inputText.Split(new char[] {' ', '.', '!', '?', ','});
            try
            {
                for (int i = 0; i < words.Count(); i++)
                {
                    var currentInputWord = words[i];
                    List<SuggestItem> listOfPredictedWords = cyberCurrent.GetCorrectSpellSuggestionsForWord(currentInputWord,
                                                                                                            isKeyMap);
                    if (listOfPredictedWords.Count > 0)
                    {
                        if (listOfPredictedWords[0].Term == currentInputWord)
                        {
                            this.tblCTWTextArea.Inlines.Add(currentInputWord + " ");
                        }
                        else
                        {
                            ComboBox comboBoxWords = GetNewSuggComboBox(currentInputWord, listOfPredictedWords);
                            this.tblCTWTextArea.Inlines.Add(comboBoxWords);
                            this.tblCTWTextArea.Inlines.Add(" ");
                        }
                    }
                    else
                    {
                        this.tblCTWTextArea.Inlines.Add(currentInputWord + " ");
                    }
                }
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
            SetTextBlockText(_inputText, _cyberCurrent, false);
        }

        private void bCTWDoneEditing_Click(object sender, RoutedEventArgs e)
        {
            string textNew = GetAllNewCorrectedText();
            this.MainWindow.tbInputAll.Text = textNew;
            this.Close();
        }

        private string GetAllNewCorrectedText()
        {
            string newText = String.Empty;
            for (int i = 0; i < this.tblCTWTextArea.Inlines.Count; i++)
            {
                var currentInline = this.tblCTWTextArea.Inlines.ElementAt(i);
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

        private void bCTWCorrect_Click(object sender, RoutedEventArgs e)
        {
            SetTextBlockText(_inputText, _cyberCurrent, true);
        }
    }
}
