using System;
using SpellingChecker.SpellingCheckerEngine.Algorithm;
using ZGTR_PorterAlgorithmApp;

namespace SpellingChecker.SpellingCheckerEngine.Algorithms.MEDAlgos
{
    public class MEDStalkerWithoutBTEnhanced : MEDStalkerWithBTEnhanced
    {
        public MEDStalkerWithoutBTEnhanced(char[] tWord, int substitutionVal, int marginOfError, int areawidthOfConversion)
            : base(tWord, substitutionVal, marginOfError, areawidthOfConversion)
        {

        }

        protected override int GetMinAndUpdateBT(int j, int i, int iInsert, int iDel, int iSub)
        {
            int min = Math.Min(iSub, Math.Min(iInsert, iDel));
            return min;
        }
    }
}