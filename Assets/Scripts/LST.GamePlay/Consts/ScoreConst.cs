namespace LST.GamePlay
{
    public struct ScoreConst
    {
        public const int Max = 10000000;
        public const double GoodMult = 0.1;

        private const int TierScoreMult = Max / 100;
        public const int Rank1Cap = 98 * TierScoreMult; //X
        public const int Rank2Cap = 95 * TierScoreMult; //S
        public const int Rank3Cap = 92 * TierScoreMult; //A
        public const int Rank4Cap = 88 * TierScoreMult; //B
        public const int Rank5Cap = 82 * TierScoreMult; //C
        public const int Rank6Cap = 76 * TierScoreMult; //D
    }
}
