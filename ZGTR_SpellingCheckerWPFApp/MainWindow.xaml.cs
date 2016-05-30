using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.GreetingTab;
using ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.ISRI;
using ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.LanguageIdentification;
using ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.PorterHandler;
using ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.TextEngineTabs;
using ZGTR_SpellingCheckerWPFApp;
using ZGTR_SpellingCheckerWPFApp.GUIControllers;

namespace ZGTR_CROSPELLSpellingCheckerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SpellCheckMeHandler SpellCheckMeHandler;
        public CROSPELLImageTHandler ImageGUIHandler;
        public GenomeCipherAreaHandler GenomeCipherAreaHandler;
        public PorterHandler PorterHandler;
        public ISRIHandler IsriHandler;
        public MEDHandler MEDHandler;
        public LanguageIdentification LangHandler;
        public FindingMainTopic MainTopicHandler;
        public WordPrediction WordPredictionHandler;
        public ArabicEnglishDictionary ArEDicHandler;
        public SentHandler SentHandler;
        public GreetingTabHandler GreetingTabHandler;

        public MainWindow()
        {
            InitializeComponent();
            this.Left = 120;
            this.Top = 20;
            ImageGUIHandler = new CROSPELLImageTHandler(this);
            GenomeCipherAreaHandler = new GenomeCipherAreaHandler(this);
            PorterHandler = new PorterHandler(this);
            IsriHandler = new ISRIHandler(this);
            MEDHandler = new MEDHandler(this);
            LangHandler = new LanguageIdentification(this);
            MainTopicHandler = new FindingMainTopic(this);
            ArEDicHandler = new ArabicEnglishDictionary(this);
            WordPredictionHandler = new WordPrediction(this);
            SpellCheckMeHandler = new SpellCheckMeHandler(this);
            SentHandler = new SentHandler(this);
            GreetingTabHandler = new GreetingTabHandler(this);
            //new TextAreaHandler(this);
        }

        private void buttonChooseInputImage_Click(object sender, RoutedEventArgs e)
        {
            ImageGUIHandler.ChooseInputImage();
        }

        private void buttonChooseImageSet_Click(object sender, RoutedEventArgs e)
        {
            ImageGUIHandler.ChooseImageSet();
        }

        private void bFindAndOrderImages_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageGUIHandler.FindAndOrderImages();
            }
            catch (Exception)
            {
            }
        }

        private void bExtractAllOuputImages_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageGUIHandler.ExtractAllOuputImages();
            }
            catch (Exception)
            {
            }   
        }

        private void bMatchGenomes_Click(object sender, RoutedEventArgs e)
        {
            GenomeCipherAreaHandler.MatchGenomes();
        }

        private void bTxGNext_Click(object sender, RoutedEventArgs e)
        {
            GenomeCipherAreaHandler.AddPage();
        }

        private void bTxGPrev_Click(object sender, RoutedEventArgs e)
        {
            GenomeCipherAreaHandler.SubPage();
        }

        private void svTxGGenomesArea_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
         
        }

        private void bTxPClearArea_Click(object sender, RoutedEventArgs e)
        {
            PorterHandler.ClearAreaInput();
        }

        private void bTxPUploadTextFile_Click(object sender, RoutedEventArgs e)
        {
            PorterHandler.UploadTextFileInput();
        }

        private void bTxPClearArea2_Click(object sender, RoutedEventArgs e)
        {
            PorterHandler.ClearAreaOutput();
        }

        private void bTxPColorify_Click(object sender, RoutedEventArgs e)
        {
            PorterHandler.ColorifyStems();
        }

        private void bTxPStem_Click(object sender, RoutedEventArgs e)
        {
            PorterHandler.Stem();
        }

        private void bTxPUploadExcelFile_Click(object sender, RoutedEventArgs e)
        {
            PorterHandler.UploadExcelFile();
        }

        private void bTxPExportToExcelFile_Click(object sender, RoutedEventArgs e)
        {
            PorterHandler.ExportToExcelFile();
        }

        private void bTxIStem_Click(object sender, RoutedEventArgs e)
        {
            IsriHandler.Stem();
        }

        private void bTxIClearArea_Click(object sender, RoutedEventArgs e)
        {
            IsriHandler.ClearArea1();
        }

        private void bTxIUploadExcelFile_Click(object sender, RoutedEventArgs e)
        {
            IsriHandler.UploadExcelFile();
        }

        private void bTxIClearArea2_Click(object sender, RoutedEventArgs e)
        {
            IsriHandler.ClearArea2();
        }

        private void bTxIExportToExcelFile_Click(object sender, RoutedEventArgs e)
        {
            IsriHandler.ExportExcelFile();
        }

        private void bTxIColorify_Click(object sender, RoutedEventArgs e)
        {
            IsriHandler.Colorify();
        }

        private void bTxLFindDistance_Click(object sender, RoutedEventArgs e)
        {
            MEDHandler.FindDistance();
        }

        private void bTxLShowBT_Click(object sender, RoutedEventArgs e)
        {
            MEDHandler.ShowBackTrack();
        }

        private void bTxLNext_Click(object sender, RoutedEventArgs e)
        {
            MEDHandler.AddPage();
        }

        private void bTxLPrev_Click(object sender, RoutedEventArgs e)
        {
            MEDHandler.SubPage();
        }

        private void bTxLUploadTxtFile_Click(object sender, RoutedEventArgs e)
        {
            MEDHandler.UploadSourceFromTextFile();
        }

        private void bTxLSetSourceWord_Click(object sender, RoutedEventArgs e)
        {
            MEDHandler.SetSourceWord();
        }

        private void bTxLUploadExlFile_Click(object sender, RoutedEventArgs e)
        {
            MEDHandler.UploadSourceFromExcelFile();
        }

        private void bTxLExportToExcFile_Click(object sender, RoutedEventArgs e)
        {
            MEDHandler.ExportExcelFile();
        }

        private void bTxSMEnterCorpus_Click(object sender, RoutedEventArgs e)
        {
            SpellCheckMeHandler.EnterCorpus();
        }

        private void tbTxSMTextInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void ebTxSMArabic_Checked(object sender, RoutedEventArgs e)
        {
            SpellCheckMeHandler.ArabicChecked();
        }

        private void ebTxSMEnglish_Checked(object sender, RoutedEventArgs e)
        {
            SpellCheckMeHandler.EnglishChecked();
        }

        private void tbTxSMTextInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            SpellCheckMeHandler.KeyDown(e);
        }

        private void ebTxSMByCorups_Checked(object sender, RoutedEventArgs e)
        {
            SpellCheckMeHandler.ByCorups();
        }

        private void ebTxSMByUserPref_Checked(object sender, RoutedEventArgs e)
        {
            SpellCheckMeHandler.ByUserPref();
        }

        private void ebTxSMByKeyboard_Checked(object sender, RoutedEventArgs e)
        {
            SpellCheckMeHandler.ByKeyMap();
        }

        private void bTxSMInitArab_Click(object sender, RoutedEventArgs e)
        {
            SpellCheckMeHandler.InitArabicCyberSpell();
        }

        private void bTxSMInitEng_Click(object sender, RoutedEventArgs e)
        {
            SpellCheckMeHandler.InitEnglishCyberSpell();
        }

        private void ebTxSMByScallarNoKM_Checked(object sender, RoutedEventArgs e)
        {
            SpellCheckMeHandler.ScallarNoKM();
        }

        private void ebTxSMByScallarKM_Checked(object sender, RoutedEventArgs e)
        {
            SpellCheckMeHandler.ScallarKM();
        }

        private void bTDicInputCheck_Click(object sender, RoutedEventArgs e)
        {
            ArEDicHandler.button1_Click();
        }

        private void bTDicOutputClearArea_Click(object sender, RoutedEventArgs e)
        {
            tbDicOutput.Clear();
        }

        private void bTDicInputClearArea_Click(object sender, RoutedEventArgs e)
        {
            tbDicInput.Clear();
        }

        private void tbInputAll_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((bool)cbTxSMEnableSpellChecking.IsChecked)
            {
                //if ((bool)this.cbTxSMEnableAutoCyber.IsChecked)
                //{
                    if ((bool) rbTxSMByKeyboard.IsChecked || (bool) rbTxSMByScallarKM.IsChecked)
                        SpellCheckMeHandler.TextBoxTextChanged(true);
                    else
                        SpellCheckMeHandler.TextBoxTextChanged();
                //}
                //else
                //{
                //    SpellCheckMeHandler.NotifyWrongWords();
                //}
            }
            else
            {
                if ((bool)cbTxSMOM.IsChecked)
                {
                    SentHandler.SetEmotionString();
                }
                if ((bool)cbTxSMLangIden.IsChecked)
                {
                    if ((bool) rbLIdentUni.IsChecked || (bool) rbLIdentBi.IsChecked)
                    {
                        LangHandler.StartIdentifingLanguage(sender, e);
                    }
                }
                if ((bool)cbTxSMWordPrediction.IsChecked)
                {
                    if ((bool) rbWPbi.IsChecked || (bool) rbWPtri.IsChecked)
                    {
                        WordPredictionHandler.SPredictingWords(sender, e);
                    }
                }
            }
        }

        private void btInputClearAll_Click(object sender, RoutedEventArgs e)
        {
            tbInputAll.Clear();
            lbLIBasicLang.Items.Clear();
            lbLIOtherLang.Items.Clear();
            LangHandler.cleanLists();
            this.tblCTWTextArea.Text = String.Empty;
        }

        private void bCTWDoneEditing_Click(object sender, RoutedEventArgs e)
        {
            this.SpellCheckMeHandler.DoneEditing();
        }

        private void btTP_Click(object sender, RoutedEventArgs e)
        {
            MainTopicHandler.buttonCheck_Click();
        }

        private void lbWP_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string str = tbInputAll.Text;
                string s = lbWP.SelectedItems[0].ToString();
                string temp = "";
                bool finished = false;
                while (s != "" && !finished)
                {
                    if (!str.EndsWith(s))
                    {
                        temp += s[s.Length - 1];
                        s = s.Remove(s.Length - 1);

                    }
                    else
                    {
                        if (str.EndsWith(s))
                        {
                            char[] charArray = temp.ToCharArray();
                            Array.Reverse(charArray);
                            temp = new string(charArray);
                            str = str + temp;
                            tbInputAll.Text = str;
                            finished = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void bTxGLoadPreDefinedCihpers_Click(object sender, RoutedEventArgs e)
        {
            this.GenomeCipherAreaHandler.LoadPreDefineGenome();
        }

        private void bTxSMCorrectAllText_Click(object sender, RoutedEventArgs e)
        {
            this.SpellCheckMeHandler.CyberAllText();
        }

        private void bTxSMAddToDictionary_Click(object sender, RoutedEventArgs e)
        {
            this.SpellCheckMeHandler.AddNewEntryToDict();
        }


        private void bCTWCorrect_Click(object sender, RoutedEventArgs e)
        {
            this.SpellCheckMeHandler.CTWCorrectForAkeyMap();
        }

        private void btInputClearCyberArea_Click(object sender, RoutedEventArgs e)
        {
            this.tblCTWTextArea.Text = String.Empty;
        }
    }
}
