using Lanostane.Models;
using LST.GamePlay.Graphics;
using LST.GamePlay.Scoring;
using UnityEngine;
using Utils.Maths;

namespace LST.GamePlay.Judge
{
    public abstract class SingleNoteJudgeHandle : JudgeHandleBase
    {
        public override float CurrentTiming => Timing;
        public override float CurrentDegree => Degree;
        public bool IsAutoJudgeNote { get; private set; }

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
            IsAutoJudgeNote = info.Flags.HasFlag(LST_NoteSpecialFlags.AutoJudgement);
        }

        public void TryReportJudge(float clickTime, InputHandle handle = null)
        {
            if (JudgeDone)
                return;

            var result = GetJudgeResult(Timing, clickTime);
            if (result == JudgeType.Miss) //???
            {
                TryReportMiss();
                return;
            }

            if (!IsJudgeReportAllowed(clickTime))
                return;

            JudgeDone = true;
            Graphic.JudgeDone = true;
            Graphic.Hide();

            ScoreManager.RegisterNote(new()
            {
                Type = result,
                Degree = Degree
            });
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
            ScoreManager.RegisterNote(new()
            {
                Type = JudgeType.Miss,
                Degree = Degree
            });
            Graphic.TriggerJudgeEffect(JudgeType.Miss);
            OnMissReported();
        }

        protected virtual void OnMissReported() { }

        protected float GetDelta(float chartTime)
        {
            return MathfE.AbsDelta(chartTime, Timing);
        }

        public bool IsDegreeInRange(float inputDegree)
        {
            var range = Size switch
            {
                LST_NoteSize.Size0 => JudgeConst.Size0TolDeg,
                LST_NoteSize.Size1 => JudgeConst.Size1TolDeg,
                LST_NoteSize.Size2 => JudgeConst.Size2TolDeg,
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
