namespace GamePlay
{
    public struct JudgeConst
    {
        public const float Timeout = 0.31f;
        public const float TapPerfectPlus = 0.07f;
        public const float TapPerfect = 0.11f;
        public const float FlickPerfect = 0.1f;
        public const float TapGood = 0.175f;
        public const float FlickGood = 0.25f;
        public const float Size0Deg = 8.5f;
        public const float Size1Deg = 12.5f;
        public const float Size2Deg = 17.0f;
        public const float Size0TolDeg = Size0Deg + JudgeAngleTolerance;
        public const float Size1TolDeg = Size1Deg + JudgeAngleTolerance;
        public const float Size2TolDeg = Size2Deg + JudgeAngleTolerance;
        public const float JudgeAngleTolerance = 11.5f;
    }
}
