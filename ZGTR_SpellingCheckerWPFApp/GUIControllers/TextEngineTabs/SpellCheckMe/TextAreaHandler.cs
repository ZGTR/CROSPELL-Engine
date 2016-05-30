using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ZGTR_CROSPELLSpellingCheckerApp;

namespace ZGTR_SpellingCheckerWPFApp
{
    public class TextAreaHandler
    {
        private MainWindow _mainWindow;

        private ComboBox cmbBox = new ComboBox();
        public TextAreaHandler(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            
            cmbBox.Items.Add("word1");
            cmbBox.Items.Add("word2");
            cmbBox.Items.Add("word3");
            cmbBox.SelectedIndex = 0;
            cmbBox.DropDownClosed += new EventHandler(cmbBox_DropDownClosed);

            var tbx = new TextBox();
            tbx.Text = "HELLOZ! :P :D";

            var tb = new TextBlock();
            tb.Inlines.Add(cmbBox);
            tb.Inlines.Add(tbx);
            //tb.Inlines.Add(cmbBox);

            this._mainWindow.mainGrid.Children.Add(tb);
        }

        void cmbBox_DropDownClosed(object sender, EventArgs e)
        {
            MessageBox.Show(cmbBox.SelectedItem as String);
        }

    }
}
