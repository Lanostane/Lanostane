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
        public static int RegisteredNotes => PerfectCount + GoodCount + MissCount;
        public static int ComboCount { get; private set; }

        public static event Action ScoreUpdated;
        public static event NoteRegisteredDel NoteRegistered;

        private static double _ScorePerNote;
        private static int _NoteCount;
        public static int PerfectPlusCount { get; private set; }
        public static int PerfectCount { get; private set; }
        public static int GoodCount { get; private set; }
        public static int MissCount { get; private set; }

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
            PerfectCount = 0;
            PerfectPlusCount = 0;
            GoodCount = 0;
            MissCount = 0;

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
                    PerfectCount++;
                    PerfectPlusCount++;
                    break;

                case JudgeType.Perfect:
                    ComboCount++;
                    PerfectCount++;
                    IsAllPurePerfect = false;
                    break;

                case JudgeType.Good:
                    ComboCount++;
                    GoodCount++;
                    IsAllPerfect = false;
                    IsAllPurePerfect = false;
                    break;

                case JudgeType.Miss:
                    MissCount++;
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
            var perfectScore = _ScorePerNote * PerfectCount;
            var goodScore = _ScorePerNote * GoodCount * GoodScoreMult;

            Score = (float)(perfectScore + goodScore + PerfectPlusCount);
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
