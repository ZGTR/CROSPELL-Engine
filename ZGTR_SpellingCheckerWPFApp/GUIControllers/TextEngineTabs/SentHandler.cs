using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.OM;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.TextEngineTabs
{
    public class SentHandler
    {
        private MainWindow MainWindow;
        private OMEngine engine;

        public SentHandler(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            engine = new OMEngine();
        }

        public void SetEmotionString(){
             
            float eval;
            //eval = engine.Evaluate("It's a bad stinky stinky stinky thing.");
            eval = engine.Evaluate(this.MainWindow.tbInputAll.Text);
            this.MainWindow.tbTxSMOM.Text = "[" + eval + "]\t" + engine.Evaluate(eval).ToString();
        }
    }
}
