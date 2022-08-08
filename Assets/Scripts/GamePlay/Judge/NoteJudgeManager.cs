using Charts;
using GamePlay.Graphics;
using GamePlay.Judge.Handles;
using GamePlay.Judge.Inputs;
using GamePlay.Scoring;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay.Judge
{
    public interface INoteJudgeManager : IChartUpdater
    {
        bool AutoPlay { get; }
        void InitializeScoring();
        void AddSingleJudgeHandle(LST_SingleNoteInfo info, ISingleNoteGraphic graphic);
        void AddLongJudgeHandle(LST_LongNoteInfo info, ILongNoteGraphic graphic);
        bool TryGetInputHandle(int index, out InputHandle handle);
        void InputHandleUpdated(InputHandle updatedInputHandle);
    }

    public partial class NoteJudgeManager : MonoBehaviour, INoteJudgeManager
    {
        public const float Timeout = 0.35f;
        public const float TapPerfect = 0.09f;
        public const float FlickPerfect = 0.15f;
        public const float TapGood = 0.25f;
        public const float FlickGood = 0.27f;
        public const float Size0Deg = 8.5f;
        public const float Size1Deg = 12.5f;
        public const float Size2Deg = 17.0f;
        public const float JudgeAngleTolerance = 11.5f;

        public static INoteJudgeManager Instance { get; private set; }

        public bool AutoPlay => _AutoPlay;

        [SerializeField] private bool _AutoPlay = false;

        private readonly List<SingleNoteJudgeHandle> _SingleNoteHandles = new();
        private readonly List<LongNoteJudgeHandle> _LongNoteHandles = new();

        void Awake()
        {
            Instance = this;
            InitializeInput();
        }

        void OnDestroy()
        {
            Instance = null;
        }

        public void UpdateChart(float chartTime)
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
            var longNoteTotal = _LongNoteHandles.Sum(x => x.TotalNoteCount);
            ScoreManager.Initialize(_SingleNoteHandles.Count + longNoteTotal);
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
            foreach (var handler in _SingleNoteHandles)
            {
                if (handler.JudgeDone)
                    continue;

                if (_AutoPlay)
                {
                    if (handler.Timing < chartTime)
                    {
                        handler.TryReportJudge(handler.Timing);
                    }
                    continue;
                }
                else
                {
                    if (chartTime >= handler.Timing + Timeout)
                    {
                        handler.TryReportMiss();
                        continue;
                    }
                }
            }
        }

        private void UpdateLongNoteJudge(float chartTime)
        {
            foreach (var handler in _LongNoteHandles)
            {
                if (_AutoPlay)
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
