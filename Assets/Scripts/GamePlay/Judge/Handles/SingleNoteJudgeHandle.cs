using Charts;
using GamePlay.Graphics;
using GamePlay.Judge.Inputs;
using GamePlay.Scoring;
using UnityEngine;
using Utils;

namespace GamePlay.Judge.Handles
{
    public abstract class SingleNoteJudgeHandle : JudgeHandleBase
    {
        public override float CurrentTiming => Timing;
        public override float CurrentDegree => Degree;

        public override int TotalNoteCount => 1;
        public override bool JudgeDone { get; protected set; }
        public float Timing { get; private set; }
        public float Degree { get; private set; }
        public LST_SingleNoteType Type { get; private set; }
        public LST_NoteSize Size { get; private set; }
        protected ISingleNoteGraphic Graphic { get; private set; }

        public void Setup(LST_SingleNoteInfo info, ISingleNoteGraphic graphic)
        {
            Timing = info.Timing;
            Degree = info.Degree;
            Type = info.Type;
            Size = info.Size;
            Graphic = graphic;
            JudgeDone = false;
        }

        public void TryReportJudge(float clickTime, InputHandle handle = null)
        {
            if (JudgeDone)
                return;

            if (!IsJudgeReportAllowed(clickTime))
                return;

            var result = GetJudgeResult(Timing, clickTime);
            if (result == JudgeType.Miss) //???
            {
                TryReportMiss();
                return;
            }

            JudgeDone = true;
            Graphic.JudgeDone = true;
            Graphic.Hide();

            ScoreManager.RegisterNote(result, Degree);
            Graphic.TriggerJudgeEffect(result);
            OnJudgeReported(result, handle);
        }

        protected virtual void OnJudgeReported(JudgeType type, InputHandle handle = null) { }

        public void TryReportMiss()
        {
            if (JudgeDone)
                return;

            JudgeDone = true;
            Graphic.JudgeDone = true;
            Graphic.Hide();
            ScoreManager.RegisterNote(JudgeType.Miss, Degree);
            Graphic.TriggerJudgeEffect(JudgeType.Miss);
            OnMissReported();
        }

        protected virtual void OnMissReported() { }

        protected void OnJudge(JudgeType type)
        {
            if (JudgeDone)
                return;

            JudgeDone = true;
            Graphic.TriggerJudgeEffect(type);
            Graphic.Hide();
        }

        protected float GetDelta(float chartTime)
        {
            return MathfE.AbsDelta(chartTime, Timing);
        }

        public bool IsDegreeInRange(float inputDegree)
        {
            var range = Size switch
            {
                LST_NoteSize.Size0 => NoteJudgeManager.Size0Deg + NoteJudgeManager.JudgeAngleTolerance,
                LST_NoteSize.Size1 => NoteJudgeManager.Size1Deg + NoteJudgeManager.JudgeAngleTolerance,
                LST_NoteSize.Size2 => NoteJudgeManager.Size2Deg + NoteJudgeManager.JudgeAngleTolerance,
                _ => 8.5f,
            };

            if (range <= 0.0f)
            {
                Debug.LogError($"SizeType: {Size} was not implemented! defaulting to size0");
            }

            if (MathfE.ApproxAngle(inputDegree, Degree, range))
                return true;

            return false;
        }
    }
}
