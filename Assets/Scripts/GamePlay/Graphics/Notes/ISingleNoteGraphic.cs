﻿using Lst.Charts;
using GamePlay.Judge;
using Utils.Maths;

namespace GamePlay.Graphics
{
    public interface ISingleNoteGraphic
    {
        LST_SingleNoteType Type { get; }
        float Timing { get; }
        Millisecond ScrollTiming { get; }
        bool JudgeDone { get; set; }
        void Setup(LST_SingleNoteInfo info);
        void Show();
        void Hide();
        void TriggerJudgeEffect(JudgeType type);
        void SetNoteType(LST_SingleNoteType type);
        void UpdateProgress(float progress01);
        void DestroyInstance();
    }
}