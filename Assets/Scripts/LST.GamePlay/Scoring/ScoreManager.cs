﻿using LST.GamePlay.Judge;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Utils.Unity;

namespace LST.GamePlay.Scoring
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
        public static int TotalNotes { get; private set; }
        public static int RegisteredNotes => PerfectCount + GoodCount + MissCount;
        public static int ComboCount { get; private set; }

        public static event Action ScoreUpdated;
        public static event NoteRegisteredDel NoteRegistered;
        public static event Action LastNoteRegistered;

        private static double s_ScorePerNote;

        public static int PerfectPlusCount { get; private set; }
        public static int PerfectCount { get; private set; }
        public static int GoodCount { get; private set; }
        public static int MissCount { get; private set; }
        public static int RegisteredCount => PerfectCount + GoodCount + MissCount;
        public static RankType CurrentRank { get; private set; }

        public static void Initialize(int maxNoteCount)
        {
            EditorLog.Info($"Setting up score! Max Notes: {maxNoteCount}");

            ComboCount = 0;

            TotalNotes = maxNoteCount;
            s_ScorePerNote = MaxScore / TotalNotes;

            Reset();
        }

        public static void Reset()
        {
            PerfectCount = 0;
            PerfectPlusCount = 0;
            GoodCount = 0;
            MissCount = 0;
            CurrentRank = RankType.Failed;

            IsAllPurePerfect = true;
            IsAllPerfect = true;
            IsAllCombo = true;
            UpdateScore();
        }

        public static void RegisterNote(ScoreData data)
        {
            var registered = RegisteredCount;
            if (registered == TotalNotes)
            {
                return;
            }

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

            
            UpdateScore();
            NoteRegistered?.Invoke(data.Type, data.Degree);
            if (registered == TotalNotes)
            {
                LastNoteRegistered?.Invoke();
            }
        }

        private static void UpdateScore()
        {
            double perfectScore = s_ScorePerNote * PerfectCount;
            double goodScore = s_ScorePerNote * GoodCount * GoodScoreMult;

            Score = (float)(perfectScore + goodScore + PerfectPlusCount);
            ScoreRounded = Mathf.RoundToInt(Score);

            SetRank(ScoreRounded);

            ScoreUpdated?.Invoke();
        }

        private static void SetRank(int intScore)
        {
            if (intScore >= ScoreConst.Rank1Cap)
            {
                CurrentRank = RankType.R1_X;
            }
            else if (intScore >= ScoreConst.Rank2Cap)
            {
                CurrentRank = RankType.R2_S;
            }
            else if (intScore >= ScoreConst.Rank3Cap)
            {
                CurrentRank = RankType.R3_A;
            }
            else if (intScore >= ScoreConst.Rank4Cap)
            {
                CurrentRank = RankType.R4_B;
            }
            else if (intScore >= ScoreConst.Rank5Cap)
            {
                CurrentRank = RankType.R5_C;
            }
            else if (intScore >= ScoreConst.Rank6Cap)
            {
                CurrentRank = RankType.R6_D;
            }
            else
            {
                CurrentRank = RankType.Failed;
            }
        }
    }
}