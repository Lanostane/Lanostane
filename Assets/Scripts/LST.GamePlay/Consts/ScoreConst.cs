namespace LST.GamePlay
{
    public struct ScoreConst
    {
        public const int Max = 10000000;
        public const double GoodMult = 0.1;

        private const int s_TierScoreMult = Max / 100;
        public const int Rank1Cap = 98 * s_TierScoreMult; //X
        public const int Rank2Cap = 95 * s_TierScoreMult; //S
        public const int Rank3Cap = 92 * s_TierScoreMult; //A
        public const int Rank4Cap = 88 * s_TierScoreMult; //B
        public const int Rank5Cap = 82 * s_TierScoreMult; //C
        public const int Rank6Cap = 76 * s_TierScoreMult; //D
    }
}
