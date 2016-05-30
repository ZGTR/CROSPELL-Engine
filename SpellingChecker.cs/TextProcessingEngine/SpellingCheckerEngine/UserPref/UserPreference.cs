using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SpellingChecker.SpellingCheckerEngine
{
    public class UserPreference
    {
        public string WordWrong { get; set; }
        public string WordPrefered { get; set; }
        public int Counter { get; set; }

        public XElement UserPreferenceXML()
        {
            XElement element = new XElement("UserPreference");
            element.SetAttributeValue("id", WordWrong);
            element.Add(
                new XElement("WordPrefered", this.WordPrefered),
                new XElement("Counter", Counter.ToString()));
            return element;
        }
    }
}
