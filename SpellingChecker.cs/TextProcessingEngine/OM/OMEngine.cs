using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.OM
{
    public class OMEngine
    {
        private Dictionary<string, SenPair> _dict;
        public OMEngine()
        {
            _dict = ReadFiles("Common.txt", "Group1.txt", "Group2.txt");
        }
        
        private Dictionary<string, SenPair> ReadFiles(string commonPath, string group1Path, string group2Path) 
        {
            Dictionary<string, SenPair> dict = new Dictionary<string, SenPair>();
            TextReader tr = null;
            try 
            {
                tr = new StreamReader(commonPath);
                string line;
                while ((line = tr.ReadLine()) != null) 
                {
                    var lineSplit = line.Split('\t');
                    var wordsSplit = lineSplit[2].Split(' ');
                    foreach (var word in wordsSplit)
	                {
		                var wordClean = word.Split('#')[0];
                        float pos = float.Parse(lineSplit[0]);
                        float neg = float.Parse(lineSplit[1]);
                        int index = dict.Keys.ToList<string>().IndexOf(wordClean);
                        //if (index != -1)
                        if(dict.Keys.Contains(wordClean))
                        {
                            if (dict[wordClean].Pos < pos)
                            {
                                dict[wordClean].Pos = pos;
                                dict[wordClean].Neg = neg;
                            }
                            else if (dict[wordClean].Neg < neg)
                            {
                                dict[wordClean].Pos = pos;
                                dict[wordClean].Neg = neg;
                            }                            
                        }
                        else
                        {
                            dict.Add(wordClean, new SenPair(pos, neg));
                        }
	                }                    
                }
                //Read Group1
                tr.Close();
                tr = new StreamReader(group1Path);
                while ((line = tr.ReadLine()) != null)
                {
                    var lineSplit = line.Split('\t');
                    var wordsSplit = lineSplit[6].Split(' ');
                    foreach (var word in wordsSplit)
                    {
                        var wordClean = word.Split('#')[0];
                        float pos = float.Parse(lineSplit[0]);
                        float neg = float.Parse(lineSplit[1]);
                        int index = dict.Keys.ToList<string>().IndexOf(wordClean);
                        //if (index != -1)
                        if (dict.Keys.Contains(wordClean))
                        {
                            if (dict[wordClean].Pos < pos)
                            {
                                dict[wordClean].Pos = pos;
                                dict[wordClean].Neg = neg;
                            }
                            else if (dict[wordClean].Neg < neg)
                            {
                                dict[wordClean].Pos = pos;
                                dict[wordClean].Neg = neg;
                            }
                        }
                        else
                        {
                            dict.Add(wordClean, new SenPair(pos, neg));
                        }
                    }
                }

                //Read Group2
                tr.Close();
                tr = new StreamReader(group2Path);
                while ((line = tr.ReadLine()) != null)
                {
                    var lineSplit = line.Split('\t');
                    var wordsSplit = lineSplit[4].Split(' ');
                    foreach (var word in wordsSplit)
                    {
                        var wordClean = word.Split('#')[0];
                        float pos = float.Parse(lineSplit[0]);
                        float neg = float.Parse(lineSplit[1]);
                        int index = dict.Keys.ToList<string>().IndexOf(wordClean);
                        //if (index != -1)
                        if (dict.Keys.Contains(wordClean))
                        {
                            //if (wordClean.Equals("worthy"))
                              //  index = index;
                            if (dict[wordClean].Pos < pos)
                            {
                                dict[wordClean].Pos = pos;
                                dict[wordClean].Neg = neg;
                            }
                            else if (dict[wordClean].Neg < neg)
                            {
                                dict[wordClean].Pos = pos;
                                dict[wordClean].Neg = neg;
                            }
                        }
                        else
                        {
                            dict.Add(wordClean, new SenPair(pos, neg));
                        }
                    }
                }

                return dict;
            }
            catch(Exception e)
            {}
            return null;
        }

        public float Evaluate(string sentence) 
        {
            var words = sentence.Split(new char[] {' ', ',', '.', '!', '?', ';'});

            var weight = 1f / words.Count();
            int countOfExisting = 0;
            float pos = 0, neg = 0;
            
            foreach (string word in words)
            {
                if (_dict.Keys.Contains(word))
                    countOfExisting++;
            }
            weight = 1f / countOfExisting;
            foreach (string word in words)
            {
                if (_dict.Keys.Contains(word))
                {
                    pos += (_dict[word].Pos)*weight;
                    neg += (_dict[word].Neg)*weight;
                }
            }
            //pos /= words.Count();
            //neg /= words.Count();
            if (pos >= neg)
                return pos;
            else if (neg > pos)
                return -neg;
            return 0;
        }

        public Evaluation Evaluate(float eval) 
        {
            int enumNum = (int)Math.Ceiling(eval * 5);
            if (eval > 0)
            {
                return(Evaluation)(enumNum-1);
            }
            else if (eval < 0)
                return(Evaluation)(-enumNum + 5);
            else
                return Evaluation.Neutral;
        }
    }
}

