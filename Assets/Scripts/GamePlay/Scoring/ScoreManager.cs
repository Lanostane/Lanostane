using GamePlay.Judge;
using System;
using UnityEngine;

namespace GamePlay.Scoring
{
    public static class ScoreManager
    {
        public delegate void NoteRegisteredDel(JudgeType type, float degree);

        public const double MaxScore = 1000000000;
        public const double GoodScoreMult = 0.6f;

        public static bool IsPerfect { get; private set; }
        public static bool IsAllCombo { get; private set; }
        public static float Score { get; private set; }
        public static int ScoreRounded { get; private set; }
        public static int TotalNotes => _NoteCount;
        public static int RegisteredNotes => _PerfectCount + _GoodCount + _MissCount;
        public static int ComboCount { get; private set; }

        public static event Action ScoreUpdated;
        public static event NoteRegisteredDel NoteRegistered;

        private static double _ScorePerNote;
        private static int _NoteCount;
        private static int _PerfectCount;
        private static int _GoodCount;
        private static int _MissCount;

        public static void Initialize(int maxNoteCount)
        {
            Debug.Log($"Setting up score! Max Notes: {maxNoteCount}");

            ComboCount = 0;

            _NoteCount = maxNoteCount;
            _ScorePerNote = MaxScore / _NoteCount;

            _PerfectCount = 0;
            _GoodCount = 0;
            _MissCount = 0;

            IsPerfect = true;
            IsAllCombo = true;

            UpdateScore();
        }

        public static void RegisterNote(JudgeType type, float degree)
        {
            switch (type)
            {
                case JudgeType.Perfect:
                    ComboCount++;
                    _PerfectCount++;
                    break;

                case JudgeType.Good:
                    ComboCount++;
                    _GoodCount++;
                    IsPerfect = false;
                    break;

                case JudgeType.Miss:
                    _MissCount++;
                    ComboCount = 0;
                    IsAllCombo = false;
                    IsPerfect = false;
                    break;

                default: //Something went wrong
                    return;
            }

            NoteRegistered?.Invoke(type, degree);
            UpdateScore();
        }

        private static void UpdateScore()
        {
            var perfectScore = _ScorePerNote * _PerfectCount;
            var goodScore = _ScorePerNote * _GoodCount * GoodScoreMult;

            Score = (float)(perfectScore + goodScore);
            ScoreRounded = Mathf.RoundToInt(Score);

            ScoreUpdated?.Invoke();
        }
    }
}
