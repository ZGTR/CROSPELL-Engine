using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace SpellingChecker.SpellingCheckerEngine.UserPref
{
    public class UserPreferenceHandler
    {
        private string _xmlDbPath;
        private XElement _elementRoot;

        public UserPreferenceHandler(string xmlDbPath)
        {
            _xmlDbPath = xmlDbPath;
            _elementRoot = XElement.Load(this._xmlDbPath);
        }

        public void AddPreferenceToXMlDb(string wordWrong, string wordPref)
        {
            XElement nodeElementStored = (from node in _elementRoot.Elements("UserPreference")
                                          where
                                              node.Attribute("id").Value.ToString() == wordWrong.ToLower() &&
                                              node.Element("WordPrefered").Value == wordPref
                                          select node).SingleOrDefault();
            if (nodeElementStored != null)
            {
                nodeElementStored.Remove();
                _elementRoot.Save(this._xmlDbPath);
                nodeElementStored.SetElementValue("Counter", Int32.Parse(nodeElementStored.Element("Counter").Value) + 1);
                _elementRoot.AddFirst(nodeElementStored);
                _elementRoot.Save(this._xmlDbPath);
            }
            else
            {
                // 1: coz it's the first time
                UserPreference userPreference = new UserPreference() { Counter = 1, WordPrefered = wordPref, WordWrong = wordWrong };
                _elementRoot.Add(userPreference.UserPreferenceXML());
                _elementRoot.Save(this._xmlDbPath);
            }
        }

        //private XmlNode GetMatchedXmlNode(XmlNodeList xmlList, string wordWrong)
        //{
        //    //try
        //    //{
        //    //    for (int i = 0; i < xmlList.Count; i++)
        //    //    {
        //    //        if (xmlList[i].InnerText.Trim().ToString() == wordWrong)
        //    //        {
        //    //            return xmlList[i];
        //    //        }
        //    //    }
        //    //}
        //    //catch (Exception)
        //    //{
        //    //}
        //    return null;
        //}
        public int GetUserPreference(string wordWrong, string wordPref)
        {
            int userPrefVal = 0;
            try
            {
                XElement nodeElementStored = (from node in _elementRoot.Elements("UserPreference")
                                              where
                                                  node.Attribute("id").Value.ToString() == wordWrong.ToLower() &&
                                                  node.Element("WordPrefered").Value == wordPref
                                              select node).SingleOrDefault();
                userPrefVal = Int32.Parse(nodeElementStored.Element("Counter").Value) + 1;
                return userPrefVal;
            }
            catch (Exception)
            {
            }
            return 0;
        }
    }
}
