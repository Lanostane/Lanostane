using GamePlay.Judge;
using System;
using UnityEngine;

namespace GamePlay.Scoring
{

    public struct ScoreData
    {
        public JudgeType Type;
        public float Degree;
    }

    public static class ScoreManager
    {
        public delegate void NoteRegisteredDel(JudgeType type, float degree);

        public const double MaxScore = 10000000;
        public const double GoodScoreMult = 0.1f;

        public static bool IsPerfect { get; private set; }
        public static bool IsAllCombo { get; private set; }
        public static float Score { get; private set; }
        public static int ScoreRounded { get; private set; }
        public static string ScoreString { get; private set; }
        public static int TotalNotes => _NoteCount;
        public static int RegisteredNotes => _PerfectCount + _GoodCount + _MissCount;
        public static int ComboCount { get; private set; }

        public static event Action ScoreUpdated;
        public static event NoteRegisteredDel NoteRegistered;

        private static double _ScorePerNote;
        private static int _NoteCount;
        private static int _PerfectPlusCount;
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
            _PerfectPlusCount = 0;
            _GoodCount = 0;
            _MissCount = 0;

            IsPerfect = true;
            IsAllCombo = true;

            UpdateScore();
        }

        public static void RegisterNote(ScoreData data)
        {
            switch (data.Type)
            {
                case JudgeType.PerfectPlus:
                    ComboCount++;
                    _PerfectCount++;
                    _PerfectPlusCount++;
                    break;

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

            NoteRegistered?.Invoke(data.Type, data.Degree);
            UpdateScore();
        }

        private static void UpdateScore()
        {
            var perfectScore = _ScorePerNote * _PerfectCount;
            var goodScore = _ScorePerNote * _GoodCount * GoodScoreMult;

            Score = (float)(perfectScore + goodScore + _PerfectPlusCount);
            ScoreRounded = Mathf.RoundToInt(Score);
            SetScoreString(ScoreRounded);

            ScoreUpdated?.Invoke();
        }

        private static void SetScoreString(int intScore)
        {
            ScoreString = $"{intScore:00000000}";
        }
    }
}
