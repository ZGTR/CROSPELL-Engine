using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZGTR_SpellingCheckerWPFApp;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.LanguageIdentification
{
   public class WordPrediction
    {
         private MainWindow MainWindow;
        private WordPredictionTri_Gram wpTriHandler ;
        private WordPredictionBi_Gram wpBiHandler;
        public WordPrediction(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            wpBiHandler = new WordPredictionBi_Gram(mainWindow);
            wpTriHandler = new WordPredictionTri_Gram(mainWindow);
        }
        public void SPredictingWords(object sender, TextChangedEventArgs e)
        {
            if ((bool)MainWindow.rbWPbi.IsChecked)
            {
                wpBiHandler.textBoxInput_TextChanged(sender, e);

            }
            else
            {
                if ((bool)MainWindow.rbWPtri.IsChecked)
                {
                    wpTriHandler.textBoxInput_TextChanged(sender, e);
                }
            }
        }
    }
}
