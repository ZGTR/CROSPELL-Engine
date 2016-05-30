using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using SpellingChecker.ImagesProcessingEngine;
using SpellingChecker.Keyboard;
using SpellingChecker.SpellingCheckerEngine;
using SpellingChecker.SpellingCheckerEngine.Algorithm;
using SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos;
using SpellingChecker.SpellingCheckerEngine.UserPref;
using ZGTR_CROSPELLSpellingCheckerLib.ImagesProcessingEngine;
using ZGTR_CROSPELLSpellingCheckerLib.SpellingCheckerEngine.Algorithms;
using ZGTR_CROSPELLSpellingCheckerLib.SpellingCheckerEngine.Algorithms.MEDAlgos.BiologicalComp;
using ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.SpellingCheckerEngine.Algorithms;

namespace SpellingChecker.cs
{
    class Program
    {
        static void Main(string[] args)
        {
            //DateTime d1 = DateTime.Now;
            //List<Bitmap> listIn = new List<Bitmap>();
            //for (int i = 1; i < 251; i++)
            //{
            //    listIn.Add(new Bitmap("imageSetIn\\image (" + i.ToString() + ").jpg"));
            //}
            //ImageChecker checker = new ImageChecker(listIn);
            //List<Bitmap> listOut = checker.GetClosestImages(new Bitmap("imageChosen.jpg"), 1, 5, 10,
            //                                                OrderingByColorOption.ByRedAndBlueAndGreenComp,
            //                                                MEDAlgorithmChosen.MEDStalkerWithoutBTEnhanced);
            //for (int i = 0; i < listOut.Count; i++)
            //{
            //    listOut[i].Save("imageSetOut\\imageOut" + (i + 1).ToString() + ".jpg");
            //}
            //DateTime d2 = DateTime.Now;
            //Console.WriteLine("Finished in " + (d2 - d1).TotalMinutes + " min, " + (d2 - d1).TotalSeconds + " sec.");
            //Console.ReadLine();


            // ZGTR: Corpus of English from  http://norvig.com/big.txt

        //    KeyboardHandler handler = new KeyboardHandler(KeyboardLanguage.Arabic);
        ////    int distance = handler.GetDistanceBetweenKeys('a', 'S');
        ////    int distasssnce = handler.GetDistanceBetweenKeys('a', 's');
        ////    int distaance = handler.GetDistanceBetweenKeys('a', 'f');
        ////    int distansssce = handler.GetDistanceBetweenKeys('a', 'F');
        ////    int distssance = handler.GetDistanceBetweenKeys('a', 'c');
        ////    int distansce = handler.GetDistanceBetweenKeys('a', 'C');
        ////    int distanscqe = handler.GetDistanceBetweenKeys('a', 'v');
        ////    int distanswce = handler.GetDistanceBetweenKeys('a', 'V');
        ////    int distansawce = handler.GetDistanceBetweenKeys('a', 'B');
        ////    int distanswsace = handler.GetDistanceBetweenKeys('a', 'N');
        ////    int distaaanswce = handler.GetDistanceBetweenKeys('a', 'M');
        ////    int daistaaanswce = handler.GetDistanceBetweenKeys('a', 'm');
        ////    int distasnce = handler.GetDistanceBetweenKeys('a', '`');
        //    int distsssance = handler.GetDistanceBetweenKeys('ش', 'ئ');
        //    int distdance = handler.GetDistanceBetweenKeys('\\', 'l');
        //    int dissdtdance = handler.GetDistanceBetweenKeys('|', 'l');
        //    //int distsance = handler.GetDistanceBetweenKeys('<', ';');

            //CyberSpell algo = new CyberSpell("", "", KeyboardLanguage.Arabic);
            //string s1 = algo.GetCorrectSpellSuggestionsForText("I ply hrd whn am angy");
            //string s = algo.GetCorrectSpellSuggestionsForText("لعب الولد في الحدديقة بجاب المزعة");
            //string s2 = algo.GetCorrectSpellSuggestionsForText("لعب الود في المكن بجان الأكل");

            //UserPreferenceHandler handler = new UserPreferenceHandler(@"Databases\UserPreferenceDb\UserPreferenceDb.xml");
            //handler.AddPreferenceToXMlDb("tade", "take");
            //handler.AddPreferenceToXMlDb("tade", "take");
            //handler.AddPreferenceToXMlDb("tade", "make");

            //int i1 = handler.GetUserPreference("tade", "take");
            //int i2 = handler.GetUserPreference("tade", "make");
            //int i3 = handler.GetUserPreference("tade", "ss");
            //int i4 = handler.GetUserPreference("s", "sds");

            //MEDRegularWithBT algo = new MEDRegularWithBT("INTENTION".ToArray(), "EXECUTION".ToArray(), 2);
            //int i = algo.GetMED();
            //var arr = algo.GetBackTraceArray();

            //MEDRegularWithBT algor = new MEDRegularWithBT("EXECUTION".ToArray(), "IINTENTION".ToArray(), 2);
            //int ir = algor.GetMED();
            //var arrr = algor.GetBackTraceArray();

            //// All matches from max i,j
            //MEDNeedlemanWunschAlgo algo1 = new MEDNeedlemanWunschAlgo("ATTATC".ToArray(), "ATCAT".ToArray(), 1, 1, 1);
            //int i1 = algo1.GetMED();
            //var list1 = algo1.GetBackTraceArray();

            //// All matches from borders
            //MEDNeedlemanWunschAlgoMod algo2 = new MEDNeedlemanWunschAlgoMod("ATTATC".ToArray(), "ATCAT".ToArray(), 1, 1, 1);
            //int i2 = algo2.GetMED();
            //var list2 = algo2.GetBackTraceArray();

            //// All matches from borders
            //MEDNeedlemanWunschAlgoMod algo2R = new MEDNeedlemanWunschAlgoMod("ATCAT".ToArray(), "ATTATC".ToArray(), 1, 1, 1);
            //int i2r = algo2R.GetMED();
            //var list2r = algo2R.GetBackTraceArray();


            //// All matches from everywhere
            //MEDSmithWatermanAlgo algo3 = new MEDSmithWatermanAlgo("ATTATC".ToArray(), "ATATC".ToArray(), 1, 1, 1);
            //int i3 = algo3.GetMED();
            //var list3 = algo3.GetBackTraceArray();

            //// All matches from everywhere
            //MEDSmithWatermanAlgo algo3R = new MEDSmithWatermanAlgo("ATCAT".ToArray(), "ATTATC".ToArray(), 1, 1, 1);
            //int i3R = algo3R.GetMED();
            //var list3R = algo3R.GetBackTraceArray();

            //Bitmap sBM = new Bitmap(@"ImagePatchFinder\s.jpg");
            //Bitmap tBM = new Bitmap(@"ImagePatchFinder\t.jpg");
            //ImagePatchFinder imagePatchFinder = new ImagePatchFinder(sBM, tBM);
            //var list = imagePatchFinder.GetPatchedAreaBitmap(ColorComponent.B, 0, 0, 150, 1, 1, 2, UnitIntention.Insertion);
            //list[0].Save(@"ImagePatchFinder\output.jpg");

            CyberSpell algo = new CyberSpell("EnglishCorpus.txt", "", KeyboardLanguage.English, false);
            var list = algo.GetCorrectSpellSuggestionsForWord("tade", false);
        }
    }
}
