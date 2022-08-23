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

        public const double MaxScore = ScoreConst.Max;
        public const double GoodScoreMult = ScoreConst.GoodMult;

        public static bool IsAllPurePerfect { get; private set; }
        public static bool IsAllPerfect { get; private set; }
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

            Reset();
            UpdateScore();
        }

        public static void Reset()
        {
            _PerfectCount = 0;
            _PerfectPlusCount = 0;
            _GoodCount = 0;
            _MissCount = 0;

            IsAllPurePerfect = true;
            IsAllPerfect = true;
            IsAllCombo = true;
        }

        public static void RegisterNote(ScoreData data)
        {
            switch (data.Type)
            {
                case JudgeType.PurePerfect:
                    ComboCount++;
                    _PerfectCount++;
                    _PerfectPlusCount++;
                    break;

                case JudgeType.Perfect:
                    ComboCount++;
                    _PerfectCount++;
                    IsAllPurePerfect = false;
                    break;

                case JudgeType.Good:
                    ComboCount++;
                    _GoodCount++;
                    IsAllPerfect = false;
                    IsAllPurePerfect = false;
                    break;

                case JudgeType.Miss:
                    _MissCount++;
                    ComboCount = 0;
                    IsAllCombo = false;
                    IsAllPerfect = false;
                    IsAllPurePerfect = false;
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
            ScoreString = intScore.ToString("D8");
        }
    }
}
