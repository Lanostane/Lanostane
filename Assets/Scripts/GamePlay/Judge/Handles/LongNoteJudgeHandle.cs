using Charts;
using GamePlay.Graphics;
using GamePlay.Judge.Inputs;
using GamePlay.Motions;
using GamePlay.Scoring;
using UnityEngine;
using Utils;

namespace GamePlay.Judge.Handles
{
    public abstract class LongNoteJudgeHandle : JudgeHandleBase
    {
        public override bool JudgeDone { get; protected set; }
        public float Timing { get; private set; }
        public float Duration { get; private set; }
        public float Degree { get; private set; }
        public LST_LongNoteType Type { get; private set; }
        public LST_NoteSize Size { get; private set; }
        protected ILongNoteGraphic Graphic { get; private set; }

        public int LastInputID { get; private set; }
        public float LastInputTime { get; private set; }
        public InputEvent LastInputType { get; private set; }

        public virtual void Setup(LST_LongNoteInfo info, ILongNoteGraphic graphic)
        {
            Timing = info.Timing;
            Duration = info.Duration;
            Degree = info.Degree;
            Type = info.Type;
            Size = info.Size;
            Graphic = graphic;
            JudgeDone = false;
            LastInputTime = float.MaxValue;
        }

        public void ReportLastInputTime(float clickTime, InputHandle updatedInputHandle)
        {
            LastInputTime = clickTime;
            if (updatedInputHandle != null)
            {
                LastInputType = updatedInputHandle.EventType;
                LastInputID = updatedInputHandle.ID;
            }
        }

        public void ResetInputID()
        {
            LastInputType = InputEvent.None;
            LastInputID = -1;
        }

        public void ResetInputTime()
        {
            LastInputTime = float.MaxValue;
            LastInputType = InputEvent.None;
            LastInputID = -1;
        }

        public void ReportAutoPlay(float updateTime)
        {
            LastInputTime = updateTime;
            LastInputType = InputEvent.AutoPlay;
            LastInputID = -1;
        }

        public abstract void UpdateGraphic(float chartTime);
        public abstract void UpdateJudge(float chartTime);

        public void ReportJudge(JudgeType type, bool fullyDone = false)
        {
            if (JudgeDone)
            {
                Debug.LogError($"Judge was reported after rail is done! {type}");
                return;
            }
                

            if (type == JudgeType.Miss)
            {
                ReportMiss(fullyDone);
                return;
            }

            if (fullyDone)
            {
                JudgeDone = true;
                Graphic.JudgeDone = true;
                Graphic.Hide();
                ResetInputTime();
            }

            ScoreManager.RegisterNote(type, CurrentDegree);
        }

        private void ReportMiss(bool fullyDone = false)
        {
            if (JudgeDone)
                return;

            if (fullyDone)
            {
                JudgeDone = true;
                Graphic.JudgeDone = true;
                Graphic.Hide();
            }

            ScoreManager.RegisterNote(JudgeType.Miss, CurrentDegree);
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

            DebugLines.DrawToBorder(CurrentDegree + range, Color.yellow, 0.1f);
            DebugLines.DrawToBorder(CurrentDegree - range, Color.yellow, 0.1f);
            DebugLines.DrawToBorder(inputDegree, Color.cyan, 0.1f);

            if (MathfE.ApproxAngle(inputDegree, CurrentDegree, range))
                return true;

            return false;
        }
    }
}
