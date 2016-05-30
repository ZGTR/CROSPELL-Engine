using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.ISRI
{
    public class ISRIAlgo
    {
        private string[] _p1, _p2, _p3;
        private string[] _s1, _s2, _s3;
        private string[][] _pr4, _pr53;
        private string []_short_vowels;
        private string []_hamza;
        private string []_intial_hamza;
        private string[] _stop_words;
        private string stm;

        public ISRIAlgo()
        {
            _p1=new string[9]{"\u0644", "\u0628", "\u0641", "\u0633", "\u0648",
                   "\u064a", "\u062a", "\u0646", "\u0627"};
            _p2=new string[2]{"\u0627\u0644", "\u0644\u0644"};
            _p3=new string[4]{"\u0643\u0627\u0644", "\u0628\u0627\u0644",
                   "\u0648\u0644\u0644", "\u0648\u0627\u0644"};


            _s3=new string[5]{"\u062a\u0645\u0644", "\u0647\u0645\u0644",
                    "\u062a\u0627\u0646", "\u062a\u064a\u0646",
                    "\u0643\u0645\u0644"};
            _s2=new string[16]{"\u0648\u0646", "\u0627\u062a", "\u0627\u0646",
                   "\u064a\u0646", "\u062a\u0646", "\u0643\u0645",
                   "\u0647\u0646", "\u0646\u0627", "\u064a\u0627",
                   "\u0647\u0627", "\u062a\u0645", "\u0643\u0646",
                   "\u0646\u064a", "\u0648\u0627", "\u0645\u0627",
                   "\u0647\u0645"};
            _s1=new string[7]{"\u0629", "\u0647", "\u064a", "\u0643", "\u062a",
                   "\u0627", "\u0646"};

            _pr4 = new string[4][];
            _pr4[0] = new string[1] { "\u0645" };
            _pr4[1] = new string[1] { "\u0627" };
            _pr4[2] = new string[3] {"\u0627", "\u0648", "\u064A" };
            _pr4[3] = new string[1] { "\u0629"};

            _pr53=new string[7][];
            _pr53[0]=new string[2]{"\u0627", "\u062a"};
            _pr53[1]=new string[3]{"\u0627", "\u064a", "\u0648"};
            _pr53[2]=new string[3]{"\u0627", "\u062a", "\u0645"};
            _pr53[3]=new string[3]{"\u0645", "\u064a", "\u062a"};
            _pr53[4]=new string[2]{"\u0645", "\u062a"};
            _pr53[5]=new string[2]{"\u0627", "\u0648"};
            _pr53[6]=new string[2]{"\u0627", "\u0645"};

            _short_vowels =new string[2]{"\u064B","\u0652"};
            _hamza = new string[3]{"\u0621","\u0624","\u0626"};
            _intial_hamza = new string[3]{"\u0622","\u0623","\u0625"};

            _stop_words = new string[49]
                              {
                                  "\u064a\u0643\u0648\u0646",
                                  "\u0648\u0644\u064a\u0633",
                                  "\u0648\u0643\u0627\u0646",
                                  "\u0643\u0630\u0644\u0643",
                                  "\u0627\u0644\u062a\u064a",
                                  "\u0648\u0628\u064a\u0646",
                                  "\u0639\u0644\u064a\u0647\u0627",
                                  "\u0645\u0633\u0627\u0621",
                                  "\u0627\u0644\u0630\u064a",
                                  "\u0648\u0643\u0627\u0646\u062a",
                                  "\u0648\u0644\u0643\u0646",
                                  "\u0648\u0627\u0644\u062a\u064a",
                                  "\u062a\u0643\u0648\u0646",
                                  "\u0627\u0644\u064a\u0648\u0645",
                                  "\u0627\u0644\u0644\u0630\u064a\u0646",
                                  "\u0639\u0644\u064a\u0647",
                                  "\u0643\u0627\u0646\u062a",
                                  "\u0644\u0630\u0644\u0643",
                                  "\u0623\u0645\u0627\u0645",
                                  "\u0647\u0646\u0627\u0643",
                                  "\u0645\u0646\u0647\u0627",
                                  "\u0645\u0627\u0632\u0627\u0644",
                                  "\u0644\u0627\u0632\u0627\u0644",
                                  "\u0644\u0627\u064a\u0632\u0627\u0644",
                                  "\u0645\u0627\u064a\u0632\u0627\u0644",
                                  "\u0627\u0635\u0628\u062d",
                                  "\u0623\u0635\u0628\u062d",
                                  "\u0623\u0645\u0633\u0649",
                                  "\u0627\u0645\u0633\u0649",
                                  "\u0623\u0636\u062d\u0649",
                                  "\u0627\u0636\u062d\u0649",
                                  "\u0645\u0627\u0628\u0631\u062d",
                                  "\u0645\u0627\u0641\u062a\u0626",
                                  "\u0645\u0627\u0627\u0646\u0641\u0643",
                                  "\u0644\u0627\u0633\u064a\u0645\u0627",
                                  "\u0648\u0644\u0627\u064a\u0632\u0627\u0644",
                                  "\u0627\u0644\u062d\u0627\u0644\u064a",
                                  "\u0627\u0644\u064a\u0647\u0627",
                                  "\u0627\u0644\u0630\u064a\u0646",
                                  "\u0641\u0627\u0646\u0647",
                                  "\u0648\u0627\u0644\u0630\u064a",
                                  "\u0648\u0647\u0630\u0627",
                                  "\u0644\u0647\u0630\u0627",
                                  "\u0641\u0643\u0627\u0646",
                                  "\u0633\u062a\u0643\u0648\u0646",
                                  "\u0627\u0644\u064a\u0647",
                                  "\u064a\u0645\u0643\u0646",
                                  "\u0628\u0647\u0630\u0627",
                                  "\u0627\u0644\u0630\u0649"
                              };
           
        }
        public string Stem(string token)
        {
            stm = token;
            Norm(1);
            if (_stop_words.ToList().Contains(stm))
            {
                return stm;
            }
            Pre32();
            Suf32();
            Waw();
            Norm(2);
            if (stm.Length <= 3)
            {
                return stm;
            }
            if (stm.Length == 4)
            {
                ProW4();
                return stm;
            }
            if (stm.Length == 5)
            {
                ProW53();
                EndW5();
                return stm;
            }
            if (stm.Length == 6)
            {
                ProW6();
                EndW6();
                return stm;
            }
            else
            {
                if (stm.Length == 7)
                {
                    Suf1();
                }
                if (stm.Length == 7)
                {
                    Pre1();
                }
                if (stm.Length == 6)
                {
                    ProW6();
                    EndW6();
                    return stm;
                }
            }
            return stm;
        }

        private void EndW6()
        {
            
            if (stm.Length==3)
            {
                return;
            }
            if(stm.Length==5)
            {
                ProW53();
                EndW5();
            }
            if (stm.Length==6)
            {
                ProW64();
                return;
            }
        }

        private void ProW64()
        {
            //      #  افعلال
            
            if (stm[0] == '\u0627' && stm[4] == '\u0627')
            {
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                return;
            }
          //     #   متفعلل
            
            if (stm.StartsWith("\u0645\u062a"))
            {
                stm = stm.Remove(0, 2);
                return;
            }
        }

        private void ProW6()
        {
           //  #   مستفعل   -    استفعل
           
            if (stm.StartsWith("\u0627\u0633\u062a") || stm.StartsWith("\u0645\u0633\u062a"))
            {
                stm=stm.Remove(0, 3);
                return;
            }
           //    #     مفعالة
            
            if (stm[0] == '\u0645' && stm[3] == '\u0627' && stm[5] == '\u0629')
            {
                stm = stm.Remove(stm.IndexOf('\u0645'), 1);
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                stm = stm.Remove(stm.IndexOf('\u0629'), 1);
                return;
            }
            //      #     افتعال
            
            if (stm[0] == '\u0627' && stm[2] == '\u062a' && stm[4] == '\u0627')
            {
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                stm = stm.Remove(stm.IndexOf('\u062a'), 1);
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                return;
            }
            //     #    افعوعل
            
            if (stm[0] == '\u0627' && stm[3] == '\u0648' && stm[2] == stm[4])
            {
                char ein = stm[2];
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                stm = stm.Remove(stm.IndexOf('\u0648'), 1);
                stm = stm.Remove(stm.IndexOf(ein), 1);
                return;
            }
            
              //#     تفاعيل  
           
            if (stm[0] == '\u062a' && stm[2] == '\u0627' && stm[4] == '\u064a')
            {
                stm = stm.Remove(stm.IndexOf('\u062a'), 1);
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                stm = stm.Remove(stm.IndexOf('\u064a'), 1);
                return;
            }
            
            Suf1();
            if (stm.Length==6)
            {
                Pre1();
            }
        }

        private void EndW5()
        {
          
            if (stm.Length==3)
            {
                return;
            }
            if (stm.Length==4)
            {
                ProW4();
                return;
            }
            if(stm.Length==5)
            {
                ProW54();
                return;
            }
        }

        private void ProW54()
        {
            //#تفعلل - افعلل - مفعلل
            foreach (string s in _pr53[2])
            {
                if ( s.Contains(stm[0]))
                {
                    stm = stm.Remove(stm.IndexOf(s), 1);
                    return;
                }
            }
            //# فعللة
            if (stm[4] == '\u0629')
                {
                    stm = stm.Remove(stm.IndexOf('\u0629'), 1);
                    return;
                }
            //# فعالل
            if (stm[2] == '\u0627')
            {
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                return;
            }
        }

        private void ProW53()
        {
            //#  افتعل   -  افاعل
            foreach (string s in _pr53[0])
            {
                if (stm[0] == '\u0627' && (s.Contains(stm[2])))
                {
                    stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                    stm = stm.Remove(stm.IndexOf(s), 1);
                    return;
                }
            }
            //# مفعول  -   مفعال  -   مفعيل
            foreach (string s in _pr53[1])
            {
                if (stm[0] == '\u0645' && (s.Contains(stm[3])))
                {
                    stm = stm.Remove(stm.IndexOf('\u0645'), 1);
                    stm = stm.Remove(stm.IndexOf(s), 1);
                    return;
                }
            }
            //#  مفعلة  -    تفعلة   -  افعلة
            foreach (string s in _pr53[2])
            {
                if (stm[4] == '\u0629' && (s.Contains(stm[0])))
                {
                    stm = stm.Remove(stm.IndexOf('\u0629'), 1);
                    stm = stm.Remove(stm.IndexOf(s), 1);
                    return;
                }
            }
            //#  مفتعل  -    يفتعل   -  تفتعل
            foreach (string s in _pr53[3])
            {
                if (stm[2] == '\u062a' && (s.Contains(stm[0])))
                {
                    stm = stm.Remove(stm.IndexOf('\u062a'), 1);
                    stm = stm.Remove(stm.IndexOf(s), 1);
                    return;
                }
            }
            //#مفاعل  -  تفاعل
            foreach (string s in _pr53[4])
            {
                if (stm[2] == '\u0627' && (s.Contains(stm[0])))
                {
                    stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                    stm = stm.Remove(stm.IndexOf(s), 1);
                    return;
                }
            }
            //#     فعولة  -   فعالة
            foreach (string s in _pr53[5])
            {
                if (stm[4] == '\u0629' && (s.Contains(stm[2])))
                {
                    stm = stm.Remove(stm.IndexOf('\u0629'), 1);
                    stm = stm.Remove(stm.IndexOf(s), 1);
                    return;
                }
            }
            //#     انفعل   -   منفعل
            foreach (string s in _pr53[6])
            {
                if (stm[1] == '\u0646' && (s.Contains(stm[0])))
                {
                    stm = stm.Remove(stm.IndexOf('\u0646'), 1);
                    stm = stm.Remove(stm.IndexOf(s), 1);
                    return;
                }
            }
            //#   افعال
            if (stm[0] == '\u0627' && (stm[3] == '\u0627'))
            {
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                return;
            }
            //#   فعلان
            if (stm[3] == '\u0627' && (stm[4] == '\u0646'))
            {
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                stm = stm.Remove(stm.IndexOf('\u0646'), 1);
                return;
            }
            //#    تفعيل
            if (stm[0] == '\u062a' && (stm[3] == '\u064a'))
            {
                stm = stm.Remove(stm.IndexOf('\u064a'), 1);
                stm = stm.Remove(stm.IndexOf('\u062a'), 1);
                
                return;
            }
            //#     فاعول
            if (stm[1] == '\u0627' && (stm[3] == '\u0648'))
            {
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                stm = stm.Remove(stm.IndexOf('\u0648'), 1);
                return;
            }
            //#     فواعل
            if (stm[1] == '\u0648' && (stm[2] == '\u0627'))
            {
                stm = stm.Remove(stm.IndexOf('\u0648'), 1);
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                return;
            }
            //#  فعائل
            if (stm[2] == '\u0627' && (stm[3] == '\u0626'))
            {
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                stm = stm.Remove(stm.IndexOf('\u0626'), 1);
                return;
            }
            //#   فاعلة
            if (stm[1] == '\u0627' && (stm[4] == '\u0629'))
            {
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                stm = stm.Remove(stm.IndexOf('\u0629'), 1);
                return;
            }
            //# فعالي
            if (stm[2] == '\u0627' && (stm[4] == '\u064a'))
            {
                stm = stm.Remove(stm.IndexOf('\u0627'), 1);
                stm = stm.Remove(stm.IndexOf('\u064a'), 1);
                return;
            }

            Suf1();
            if (stm.Length == 5)
            {
                Pre1();
            }
        }

        private void ProW4()
        {
            //#  مفعل
            if (_pr4[0][0].Contains(stm[0]))
            {
                stm = stm.Remove(stm.IndexOf(_pr4[0][0]), 1);
                return;
            }
            //#   فاعل
            if (_pr4[1][0].Contains(stm[1]))
            {
                stm = stm.Remove(stm.IndexOf(_pr4[1][0]), 1);
                return;
            }
            //#    فعال   -   فعول    - فعيل
            foreach (string s in _pr4[2])
            {
                if (s.Contains(stm[2]))
                {
                    stm = stm.Remove(stm.IndexOf(s), 1);
                    return;
                }
            }
            //#     فعلة
            if (_pr4[3][0].Contains(stm[3]))
            {
                stm = stm.Remove(stm.IndexOf(_pr4[3][0]), 1);
                return;
            }
            Suf1();
            if (stm.Length==4)
            {
                Pre1();
            }
            
        }

        private void Pre1()
        {
            foreach (string s in _p1)
            {
                if (stm.StartsWith(s))
                {
                    stm = stm.Remove(stm.IndexOf(s), 1);
                    return;
                }
            }
        }

        private void Suf1()
        {
            foreach (string s in _s1)
            {
                if (stm.EndsWith(s))
                {
                    stm = stm.Remove(stm.IndexOf(s), 1);
                    return;
                }   
            }
        }

        private void Waw()
        {
            if ((stm.Length >= 4) && (stm.StartsWith("\u0648\u0648")))
            {
                stm = stm.Remove(stm.IndexOf("\u0648\u0648"), 1);
            }
        }

        private void Suf32()
        {
            if (stm.Length >= 6)
            {
                foreach (string s in _s3)
                {
                    if (stm.EndsWith(s))
                    {
                        stm = stm.Remove(stm.IndexOf(s), 3);
                        return;
                    }
                }
            }

            if (stm.Length >= 5)
            {
                foreach (string s in _s2)
                {
                    if (stm.EndsWith(s))
                    {
                        stm = stm.Remove(stm.IndexOf(s), 2);
                        return;
                    }
                }
            }
        }

        private void Pre32()
        {
            if (stm.Length >= 6)
            {
                foreach (string s in _p3)
                {
                    if (stm.StartsWith(s))
                    {
                        stm = stm.Remove(stm.IndexOf(s), 3);
                        return;
                    }
                }
            }

            if (stm.Length >= 5)
            {
                foreach (string s in _p2)
                {
                    if (stm.StartsWith(s))
                    {
                        stm = stm.Remove(stm.IndexOf(s), 2);
                        return;
                    }
                }
            }
        }

        private void Norm(int i)
        {
            if (i==1)
            {
                 RemoveShortVowels();
            }
            if (i==2)
            {
                RemoveIntialHamza();
            }

        }

        private void RemoveIntialHamza()
        {
            foreach (string t in _intial_hamza)
            {
                if(stm.Contains(t))
                {
                    //int k = stm.IndexOf(_intial_hamza[j]);
                    stm=stm.Replace(t, "\u0627");
                }
            }
        }

        private void RemoveShortVowels()
        {
            foreach (string t in _short_vowels)
            {
                if (stm.Contains(t))
                {
                    int j = stm.IndexOf(t);
                    stm=stm.Remove(j, 1);
                }
            }
        }

        public static String GetStemmedTextForFile(String filePath)
        {
            String textStemmed = String.Empty;
            try
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                StringBuilder currentWord = new StringBuilder();
                try
                {
                    while (true)
                    {
                        int newByte = fileStream.ReadByte();
                        char ch = Char.ToLower((char)newByte);
                        if (Char.IsLetter(ch))
                        {
                            while (true)
                            {
                                currentWord.Append(ch);
                                newByte = fileStream.ReadByte();
                                ch = Char.ToLower((char)newByte);
                                if (!Char.IsLetter(ch))
                                {
                                    ISRIAlgo isriStemmer = new ISRIAlgo();
                                    textStemmed += isriStemmer.Stem(currentWord.ToString());
                                    currentWord = new StringBuilder();
                                    break;
                                }
                            }
                        }
                        if (newByte < 0)
                            break;
                        // Write down the delimeter
                        textStemmed += ch.ToString();
                    }
                }
                catch (IOException)
                {
                    new IOException("Error while reading the file.");
                }
            }
            catch (FileNotFoundException)
            {
                new FileNotFoundException("The file: " + filePath + " was not found.");
            }
            return textStemmed;
        }
    }
}
