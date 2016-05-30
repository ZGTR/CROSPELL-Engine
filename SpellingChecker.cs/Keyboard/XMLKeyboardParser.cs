using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SpellingChecker.Keyboard
{
    public static class XMLKeyboardParser
    {
        public static string[][][] ParseKeyboard(string xmlFilePath, KeyboardLanguage lang)
        {
            char delimeter = SetDelimeter(lang);
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFilePath);
            string[][][] arrKeyboard = new string[2][][];
            arrKeyboard[0] = new string[4][];
            arrKeyboard[1] = new string[4][];

            XmlNodeList elemList = doc.GetElementsByTagName("KeyboardRow");
            for (int i = 0; i < elemList.Count; i++)
            {
                arrKeyboard[0][i] = elemList[i].ChildNodes[0].InnerText.Trim().Split(delimeter);
                arrKeyboard[0][i] = arrKeyboard[0][i].Select(key => key.Trim()).ToArray();

                arrKeyboard[1][i] = elemList[i].ChildNodes[1].InnerText.Trim().Split(delimeter);
                arrKeyboard[1][i] = arrKeyboard[1][i].Select(key => key.Trim()).ToArray();
            }
            return arrKeyboard;
        }

        private static char SetDelimeter(KeyboardLanguage lang)
        {
            switch (lang)
            {
                case KeyboardLanguage.English:
                    return ' ';
                    break;
                case KeyboardLanguage.Arabic:
                    return '\n';
                    break;
                default:
                    throw new ArgumentOutOfRangeException("lang");
            }
        }
    }
}
