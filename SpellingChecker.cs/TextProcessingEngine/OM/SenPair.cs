namespace ZGTR_CROSPELLSpellingCheckerLib.TextProcessingEngine.OM
{
    public class SenPair
    {
        public float Pos { set; get; }
        public float Neg { set; get; }

        public SenPair(float pos, float neg)
        {
            this.Pos = pos;
            this.Neg = neg;
        }
    }
}
