using System;
using System.IO;
using System.Text;

namespace ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.Porter
{
    public class PorterStemmer
    {
       // private int _wordLength;
        private  int _indexRightLeft;
        private  int _indexRightRight;
        private  char[] _stemSoFar;

        //public void AddSubString(String subStr)
        //{
        //    //if (_wordLength == 0)
        //    //    _wordStrBuilder = new StringBuilder();
        //    if (_stemSoFar != null)
        //        _wordStrBuilder = new StringBuilder(new String(_stemSoFar, 0, _stemSoFar.Length));
        //    _wordStrBuilder.Append(subStr);
        //    _stemSoFar = _wordStrBuilder.ToString().ToCharArray();
        //    _wordLength = _stemSoFar.Length;
        //}

        public PorterStemmer()
        {
            _indexRightLeft = 0;
            _indexRightRight = 0;
            _stemSoFar = null;
        }

        private  int M()
        {
            // [C](VC){m}[V]
            int n = 0;
            int i = 0;
            while (true)
            {
                if (i > _indexRightLeft) return n;
                // Capture first vowel
                if (!IsConstantAtIndex(i)) break;
                i++;
            }
            i++;
            while (true)
            {
                // Capture a constant
                while (true)
                {
                    if (i > _indexRightLeft) return n;
                    if (IsConstantAtIndex(i)) break;
                    i++;
                }
                i++;
                n++;
                // Capture a vowel
                while (true)
                {
                    if (i > _indexRightLeft) return n;
                    if (!IsConstantAtIndex(i)) break;
                    i++;
                }
                i++;
            }
        }

        private  bool IsConstantAtIndex(int i)
        {
            switch (_stemSoFar[i])
            {
                case 'a':
                case 'e':
                case 'i':
                case 'o':
                case 'u': return false;
                case 'y': return (i == 0) ? true : !IsConstantAtIndex(i - 1);
                default: return true;
            }
        }

        private  bool IsVowelInStem()
        {
            for (int i1 = 0; i1 <= _indexRightLeft; i1++)
            {
                if (!IsConstantAtIndex(i1))
                    return true;
            }
            return false;
        }

        // Hopping -> Hop / Filing -> File
        private  bool ContainsDoubleConstant(int indexRightLeft)
        {
            if (indexRightLeft < 1)
                return false;
            if (_stemSoFar[indexRightLeft] != _stemSoFar[indexRightLeft - 1])
                return false;
            return IsConstantAtIndex(indexRightLeft);
        }

        /* CVC(i) is true <=> i-2,i-1,i has the form IsConstantAtIndex - vowel - IsConstantAtIndex
           and also if the second c is not w,x or y. this is used when trying to
           restore an e at the end of a short word. e.g.

              cav(e), lov(e), hop(e), crim(e), but
              snow, box, tray.
        */
        private  bool CVC(int iIn)
        {
            if (((iIn >= 2 && IsConstantAtIndex(iIn)) && !IsConstantAtIndex(iIn - 1)) && IsConstantAtIndex(iIn - 2))
            {
                int ch = _stemSoFar[iIn];
                return (ch != 'w' && ch != 'x') && ch != 'y';
            }
            return false;
        }

        private  bool EndsWith(String s)
        {
            int length = s.Length;
            int nextIndexToAdd = _indexRightRight - length + 1;
            if (nextIndexToAdd < 0)
                return false;
            char[] sc = s.ToCharArray();
            for (int index0 = 0; index0 < length; index0++)
                if (_stemSoFar[nextIndexToAdd + index0] != sc[index0])
                    return false;
            _indexRightLeft = _indexRightRight - length;
            return true;
        }

        private  void SetEndOfWordTo(String s)
        {
            int length = s.Length;
            int nextIndexToAdd = _indexRightLeft+ 1;
            char[] sc = s.ToCharArray();
            for (int index0 = 0; index0 < length; index0++)
                _stemSoFar[nextIndexToAdd + index0] = sc[index0];
            _indexRightRight = _indexRightLeft+ length;
        }

        private  void CheckM_andReplaceEndWith(String s)
        {
            if (M() > 0)
                SetEndOfWordTo(s);
        }

        #region Steps

        private  void Step1()
        {
            Step1a();
            Step1b();
            Step1c();
        }

        private  void Step1a()
        {
            if (_stemSoFar[_indexRightRight] == 's') // plurals
            {
                if (EndsWith("sses"))   // SSES -> SS
                    _indexRightRight -= 2;
                else if (EndsWith("ies"))   // IES -> I 
                    SetEndOfWordTo("i");
                else if (_stemSoFar[_indexRightRight - 1] != 's')   // S -> NULL
                    _indexRightRight--;
            }
        }

        private  void Step1b()
        {
            if (EndsWith("eed"))    // (m>0) EED -> EE 
            {
                if (M() > 0)
                    _indexRightRight--;
            }
            // If the second or third of the rules in Step 1b is successful
            // (*v*) ED -> 
            // (*v*) ING ->
            else if ((EndsWith("ed") && IsVowelInStem()) || (EndsWith("ing") && IsVowelInStem()))
            {
                // Stem the ed and the ing from the word
                _indexRightRight = _indexRightLeft;
                // Now look for further stemming
                if (EndsWith("at"))
                    SetEndOfWordTo("ate");
                else if (EndsWith("bl"))
                    SetEndOfWordTo("ble");
                else if (EndsWith("iz"))
                    SetEndOfWordTo("ize");
                else if (ContainsDoubleConstant(_indexRightRight))
                {
                    _indexRightRight--;
                    // cash ch for further use
                    char ch = _stemSoFar[_indexRightRight];
                    if (ch == 'l' || ch == 's' || ch == 'z')
                        _indexRightRight++;
                }
                else if (M() == 1 && CVC(_indexRightRight)) 
                    SetEndOfWordTo("e");
            }
        }

        private  void Step1c()
        {
            if (EndsWith("y") && IsVowelInStem())
                _stemSoFar[_indexRightRight] = 'i';
        }

        private  void Step2()
        {
            // OutOfBound fix
            if (_indexRightRight == 0)
                return;

            // OutOfBound fix
            switch (_stemSoFar[_indexRightRight - 1])
            {
                // Switch-case structure to improve performance a bit
                case 'a':
                    if (EndsWith("ational")) { CheckM_andReplaceEndWith("ate"); break; }
                    if (EndsWith("tional")) { CheckM_andReplaceEndWith("tion"); break; }
                    break;
                case 'c':
                    if (EndsWith("enci")) { CheckM_andReplaceEndWith("ence"); break; }
                    if (EndsWith("anci")) { CheckM_andReplaceEndWith("ance"); break; }
                    break;
                case 'e':
                    if (EndsWith("izer")) { CheckM_andReplaceEndWith("ize"); break; }
                    break;
                case 'l':
                    if (EndsWith("bli")) { CheckM_andReplaceEndWith("ble"); break; }
                    if (EndsWith("alli")) { CheckM_andReplaceEndWith("al"); break; }
                    if (EndsWith("entli")) { CheckM_andReplaceEndWith("ent"); break; }
                    if (EndsWith("eli")) { CheckM_andReplaceEndWith("e"); break; }
                    if (EndsWith("ousli")) { CheckM_andReplaceEndWith("ous"); break; }
                    break;
                case 'o':
                    if (EndsWith("ization")) { CheckM_andReplaceEndWith("ize"); break; }
                    if (EndsWith("ation")) { CheckM_andReplaceEndWith("ate"); break; }
                    if (EndsWith("ator")) { CheckM_andReplaceEndWith("ate"); break; }
                    break;
                case 's':
                    if (EndsWith("alism")) { CheckM_andReplaceEndWith("al"); break; }
                    if (EndsWith("iveness")) { CheckM_andReplaceEndWith("ive"); break; }
                    if (EndsWith("fulness")) { CheckM_andReplaceEndWith("ful"); break; }
                    if (EndsWith("ousness")) { CheckM_andReplaceEndWith("ous"); break; }
                    break;
                case 't':
                    if (EndsWith("aliti")) { CheckM_andReplaceEndWith("al"); break; }
                    if (EndsWith("iviti")) { CheckM_andReplaceEndWith("ive"); break; }
                    if (EndsWith("biliti")) { CheckM_andReplaceEndWith("ble"); break; }
                    break;
                // Added from others
                case 'g':
                    if (EndsWith("logi")) { CheckM_andReplaceEndWith("log"); break; }
                    break;
                default:
                    break;
            }
        }

        // -ic-, -full, -ness ...
        private  void Step3()
        {
            switch (_stemSoFar[_indexRightRight])
            {
                case 'e':
                    if (EndsWith("icate")) { CheckM_andReplaceEndWith("ic"); break; }
                    if (EndsWith("ative")) { CheckM_andReplaceEndWith(""); break; }
                    if (EndsWith("alize")) { CheckM_andReplaceEndWith("al"); break; }
                    break;
                case 'i':
                    if (EndsWith("iciti")) { CheckM_andReplaceEndWith("ic"); break; }
                    break;
                case 'l':
                    if (EndsWith("ical")) { CheckM_andReplaceEndWith("ic"); break; }
                    if (EndsWith("ful")) { CheckM_andReplaceEndWith(""); break; }
                    break;
                case 's':
                    if (EndsWith("ness")) { CheckM_andReplaceEndWith(""); break; }
                    break;
            }
        }

        // -ant, -ence ...
        // <c>vcvc<v>
        private  void Step4()
        {
            if (_indexRightRight == 0)
                return;

            switch (_stemSoFar[_indexRightRight - 1])
            {
                case 'a':
                    if (EndsWith("al")) break; return;
                case 'c':
                    if (EndsWith("ance")) break;
                    if (EndsWith("ence")) break; return;
                case 'e':
                    if (EndsWith("er")) break; return;
                case 'i':
                    if (EndsWith("ic")) break; return;
                case 'l':
                    if (EndsWith("able")) break;
                    if (EndsWith("ible")) break; return;
                case 'n':
                    if (EndsWith("ant")) break;
                    if (EndsWith("ement")) break;
                    if (EndsWith("ment")) break;
                    if (EndsWith("ent")) break; return; // "element" prob
                case 'o':
                    if (EndsWith("ion") && _indexRightLeft>= 0 &&
                        (_stemSoFar[_indexRightLeft] == 's' || _stemSoFar[_indexRightLeft] == 't')) break;
                    if (EndsWith("ou")) break; return;
                case 's':
                    if (EndsWith("ism")) break; return;
                case 't':
                    if (EndsWith("ate")) break;
                    if (EndsWith("iti")) break; return;
                case 'u':
                    if (EndsWith("ous")) break; return;
                case 'v':
                    if (EndsWith("ive")) break; return;
                case 'z':
                    if (EndsWith("ize")) break; return;
                default:
                    return;
            }
            if (M() > 1)
                _indexRightRight = _indexRightLeft;
        }

        // tidy up
        private  void Step5()
        {
            _indexRightLeft= _indexRightRight;

            if (_stemSoFar[_indexRightRight] == 'e')
            {
                // Cash M() into memory
                int m = M();
                if (m > 1 || m == 1 && !CVC(_indexRightRight - 1))
                    _indexRightRight--;
            }
            if (_stemSoFar[_indexRightRight] == 'l' && ContainsDoubleConstant(_indexRightRight) && M() > 1)
                _indexRightRight--;
        }

        #endregion

        public  String GetStem(String wordToStem)
        {
            _stemSoFar = wordToStem.ToCharArray();
            _indexRightRight = _stemSoFar.Length - 1;
            if (_indexRightRight > 1)
            {
                Step1();
                Step2();
                Step3();
                Step4();
                Step5();
            }
            return this.ToString();
        }

        public override string ToString()
        {
            return new String(_stemSoFar, 0, _indexRightRight + 1);
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
                                    PorterStemmer porterStemmer = new PorterStemmer();
                                    textStemmed += porterStemmer.GetStem(currentWord.ToString());
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