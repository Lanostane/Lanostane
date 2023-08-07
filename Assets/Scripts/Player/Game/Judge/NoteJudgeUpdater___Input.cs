using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Maths;

namespace LST.Player.Judge
{
    public partial class NoteJudgeUpdater : MonoBehaviour, IChartUpdater
    {
        public const int AllowedInputCount = 9; //Who would use more than 9 fingers?
        private readonly InputHandle[] _InputHandles = new InputHandle[AllowedInputCount];
        private readonly List<JudgeHandleBase> _FirstPass = new();

        void InitializeInput()
        {
            for (int i = 0; i < AllowedInputCount; i++)
            {
                _InputHandles[i] = new(i);
            }
        }

        public bool TryGetInputHandle(int index, out InputHandle handle)
        {
#if UNITY_EDITOR_WIN
            index = Mathf.Abs(index);
#endif

            if (index < 0)
            {
                handle = null;
                return false;
            }

            if (index >= AllowedInputCount)
            {
                handle = null;
                return false;
            }

            handle = _InputHandles[index];
            return true;
        }

        public void InputHandleUpdated(InputHandle updatedInputHandle)
        {
            if (!updatedInputHandle.InValidPosition)
                return;

            var time = ChartPlayer.OffsetChartTime;
            UpdateFirstPass(time, updatedInputHandle);
            var handle = GetEligibleFromFirstPass(updatedInputHandle);
            if (handle == null)
                return;

            if (handle is LongNoteJudgeHandle longJudge)
            {
                longJudge.ReportLastInputTime(time, updatedInputHandle);
            }
            else if (handle is SingleNoteJudgeHandle singleJudge)
            {
                singleJudge.TryReportJudge(time, updatedInputHandle);
            }
        }

        private void UpdateHoldInput()
        {
            for (int i = 0; i < AllowedInputCount; i++)
            {
                var handle = _InputHandles[i];
                if (!handle.Holding)
                    continue;

                handle.EventType = InputEvent.PointerHold;
                InputHandleUpdated(handle);
            }
        }

        private void UpdateFirstPass(float time, InputHandle updatedInputHandle)
        {
            _FirstPass.Clear();
            var litems = _LongNoteHandles.Items;
            for (int i = 0; i < litems.Length; i++)
            {
                var handler = litems[i];
                if (handler.JudgeDone)
                    continue;

                if (!handler.IsInputAllowed(time))
                    continue;

                var routine = handler.ProcessInput(time, updatedInputHandle);
                if (routine == JudgeRoutine.AddToFirstPass)
                    _FirstPass.Add(handler);

                if (routine == JudgeRoutine.ForceStop)
                    break;
            }

            var sitems = _SingleNoteHandles.Items;
            for (int i = 0; i < sitems.Length; i++)
            {
                var handler = sitems[i];
                if (handler.JudgeDone)
                    continue;

                if (!handler.IsInputAllowed(time))
                    continue;

                var routine = handler.ProcessInput(time, updatedInputHandle);

                if (routine == JudgeRoutine.AddToFirstPass)
                    _FirstPass.Add(handler);

                if (routine == JudgeRoutine.ForceStop)
                    break;
            }
        }

        private JudgeHandleBase GetEligibleFromFirstPass(InputHandle inputHandle)
        {
            JudgeHandleBase handleToTrigger = null;
            //Actually find most relatable note
            if (_FirstPass.Count > 0)
            {
                if (_FirstPass.Count == 1) //Common Stuff EZ clap
                {
                    handleToTrigger = _FirstPass[0];
                }
                else //2+ This is thing we really need to handle
                {
                    var sorted = _FirstPass.OrderBy(x => x.CurrentTiming);
                    var timeSample = sorted.First();

                    var timeSimilaritySamples = sorted.Where(x => MathfE.AbsApprox(timeSample.CurrentTiming, x.CurrentTiming, 0.0018f));
                    var samplesCount = timeSimilaritySamples.Count();
                    if (samplesCount <= 1) //Only One TimeSimilar Sample? (Itself)
                    {
                        handleToTrigger = timeSample;
                    }
                    else //2+ Time Similar Sample?
                    {
                        handleToTrigger = timeSimilaritySamples
                            .OrderBy(x => MathfE.AbsDeltaAngle(x.CurrentDegree, inputHandle.GameAngle))
                            .First();
                    }
                }
            }

            return handleToTrigger;
        }
    }
}
