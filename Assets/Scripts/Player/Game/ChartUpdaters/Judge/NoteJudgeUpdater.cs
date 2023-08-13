using Lanostane.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using LST.Player.Graphics;
using LST.Player.Scoring;

namespace LST.Player.Judge
{
    public partial class NoteJudgeUpdater : MonoBehaviour, INoteJudgeUpdater
    {
        public bool AutoPlay { get; private set; }

        private readonly FastList<SingleNoteJudgeHandle> _SingleNoteHandles = new();
        private readonly FastList<LongNoteJudgeHandle> _LongNoteHandles = new();

        void Awake()
        {
            GamePlay.NoteJudgeUpdater = this;
            AutoPlay = PlayerSettings.DebugSetting.AudoPlayEnabled;
            InitializeInput();
        }

        void OnDestroy()
        {
            GamePlay.NoteJudgeUpdater = null;
        }

        public void TimeUpdate(float chartTime)
        {
            UpdateHoldInput();
            UpdateSingleNoteJudge(chartTime);
            UpdateLongNoteJudge(chartTime);
        }

        public void CleanUp()
        {
            _SingleNoteHandles.Clear();
            _LongNoteHandles.Clear();
        }

        public void InitializeScoring()
        {
            var longNoteTotal = _LongNoteHandles.Items.Sum(x => x.TotalNoteCount);
            ScoreManager.Initialize(_SingleNoteHandles.Length + longNoteTotal);
        }

        public void AddSingleJudgeHandle(LST_SingleNoteInfo info, ISingleNoteGraphic graphic)
        {
            SingleNoteJudgeHandle handle = null;
            switch (info.Type)
            {
                case LST_SingleNoteType.Click:
                    handle = new JudgeHandle_Single_Click();
                    break;

                case LST_SingleNoteType.Catch:
                    handle = new JudgeHandle_Single_Catch();
                    break;

                case LST_SingleNoteType.FlickIn:
                    handle = new JudgeHandle_Single_FlickIn();
                    break;

                case LST_SingleNoteType.FlickOut:
                    handle = new JudgeHandle_Single_FlickOut();
                    break;
            }

            if (handle != null)
            {
                handle.Setup(info, graphic);
                _SingleNoteHandles.Add(handle);
            }
            else
            {
                Debug.LogError($"{info.Type} was invalid for SingleNoteType...");
            }
        }

        public void AddLongJudgeHandle(LST_LongNoteInfo info, ILongNoteGraphic graphic)
        {
            LongNoteJudgeHandle handle = null;
            switch (info.Type)
            {
                case LST_LongNoteType.Hold:
                    handle = new JudgeHandle_Long_Hold();
                    break;
            }

            if (handle != null)
            {
                handle.Setup(info, graphic);
                _LongNoteHandles.Add(handle);
            }
            else
            {
                Debug.LogError($"{info.Type} was invalid for LongNoteType...");
            }
        }

        private void UpdateSingleNoteJudge(float chartTime)
        {
            var items = _SingleNoteHandles.Items;
            for(int i = 0; i<items.Length; i++)
            {
                var handler = items[i];
                if (handler.JudgeDone)
                    continue;

                if (AutoPlay)
                {
                    if (handler.Timing < chartTime)
                    {
                        handler.TryReportJudge(handler.Timing);
                    }
                    continue;
                }
                else
                {
                    if (chartTime >= handler.Timing + JudgeConst.Timeout)
                    {
                        handler.TryReportMiss();
                        continue;
                    }
                }
            }
        }

        private void UpdateLongNoteJudge(float chartTime)
        {
            var items = _LongNoteHandles.Items;
            for (int i = 0; i < items.Length; i++)
            {
                var handler = items[i];
                if (AutoPlay)
                {
                    handler.ReportAutoPlay(chartTime);
                    handler.UpdateGraphic(chartTime);
                    handler.UpdateJudge(chartTime);
                }
                else
                {
                    handler.UpdateGraphic(chartTime);
                    handler.UpdateJudge(chartTime);
                }
            }
        }
    }
}
