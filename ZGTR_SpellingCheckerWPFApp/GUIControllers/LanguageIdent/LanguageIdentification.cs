using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZGTR_SpellingCheckerWPFApp;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.LanguageIdentification
{
    public class LanguageIdentification
    {
        private MainWindow MainWindow;
        private LanguageIdentificationUni_GramHandler LIUniHandler ;
        private LanguageIdentificationBi_GramHandler LIBiHandler;
        public LanguageIdentification(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            LIUniHandler = new LanguageIdentificationUni_GramHandler(mainWindow);
            LIBiHandler=new LanguageIdentificationBi_GramHandler(mainWindow);
        }
        public void StartIdentifingLanguage(object sender, TextChangedEventArgs e)
        {
            if ((bool)MainWindow.rbLIdentUni.IsChecked)
            {
                LIUniHandler.textBoxInput_TextChanged(sender,e);

            }
            else
            {
                if((bool)MainWindow.rbLIdentBi.IsChecked)
                {
                    LIBiHandler.textBoxInput_TextChanged(sender,e);
                }
            }
        }
        public void cleanLists()
        {
            LIUniHandler.CleanLists();
            LIBiHandler.CleanLists();
        }
    }
}
