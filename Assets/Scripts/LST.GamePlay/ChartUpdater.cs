using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Lanostane.Models;
using LST.GamePlay.Scoring;
using System;
using System.Collections;
using UnityEngine;

namespace LST.GamePlay
{
    internal sealed class ChartUpdater : IChartUpdater
    {
        public async UniTask BuildFromChart(LST_Chart chart, IProgress<LoadChartSteps> progress)
        {
            progress?.Report(LoadChartSteps.S3_BuildMotions);
            GamePlays.MotionUpdater.AddFromChart(chart);
            await UniTask.Yield();

            progress?.Report(LoadChartSteps.S4_PrepareMotions);
            GamePlays.MotionUpdater.Prepare();
            await UniTask.Yield();

            progress?.Report(LoadChartSteps.S5_AddScrolls);
            GamePlays.ScrollUpdater.AddFromChart(chart);
            await UniTask.Yield();

            progress?.Report(LoadChartSteps.S6_PrepareScrolls);
            GamePlays.ScrollUpdater.Prepare();
            await UniTask.Yield();

            int jobCount = 0;
            int jobPerFrame = 20;
            progress?.Report(LoadChartSteps.S7_AddSingleNotes);
            foreach (var note in chart.TapNotes)
            {
                var graphic = GamePlays.GraphicUpdater.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength && !note.Flags.HasFlag(LST_NoteSpecialFlags.NoJudgement))
                    GamePlays.NoteJudgeUpdater.AddSingleJudgeHandle(note.NoteInfo, graphic);

                if (++jobCount >= jobPerFrame)
                {
                    jobCount = 0;
                    await UniTask.Yield();
                }
            }
            

            foreach (var note in chart.CatchNotes)
            {
                var graphic = GamePlays.GraphicUpdater.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength && !note.Flags.HasFlag(LST_NoteSpecialFlags.NoJudgement))
                    GamePlays.NoteJudgeUpdater.AddSingleJudgeHandle(note.NoteInfo, graphic);

                if (++jobCount >= jobPerFrame)
                {
                    jobCount = 0;
                    await UniTask.Yield();
                }
            }

            foreach (var note in chart.FlickNotes)
            {
                var graphic = GamePlays.GraphicUpdater.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength && !note.Flags.HasFlag(LST_NoteSpecialFlags.NoJudgement))
                    GamePlays.NoteJudgeUpdater.AddSingleJudgeHandle(note.NoteInfo, graphic);

                if (++jobCount >= jobPerFrame)
                {
                    jobCount = 0;
                    await UniTask.Yield();
                }
            }

            progress?.Report(LoadChartSteps.S8_AddLongNotes);
            foreach (var note in chart.HoldNotes)
            {
                var graphic = GamePlays.GraphicUpdater.AddLongNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength && !note.Flags.HasFlag(LST_NoteSpecialFlags.NoJudgement))
                    GamePlays.NoteJudgeUpdater.AddLongJudgeHandle(note.NoteInfo, graphic);

                if (++jobCount >= jobPerFrame)
                {
                    jobCount = 0;
                    await UniTask.Yield();
                }
            }
            await UniTask.Yield();

            progress?.Report(LoadChartSteps.S9_PrepareGraphics);
            GamePlays.GraphicUpdater.Prepare();
            await UniTask.Yield();

            progress?.Report(LoadChartSteps.S10_InitializeScoring);
            GamePlays.NoteJudgeUpdater.InitializeScoring();
        }

        public void TimeUpdate(float chartTime)
        {
            GamePlays.BGUpdater.TimeUpdate(chartTime);
            GamePlays.ScrollUpdater.TimeUpdate(chartTime);
            GamePlays.NoteJudgeUpdater.TimeUpdate(chartTime);
            GamePlays.MotionUpdater.TimeUpdate(chartTime);
            GamePlays.GraphicUpdater.TimeUpdate(chartTime);
        }

        public void CleanUp()
        {
            GamePlays.BGUpdater.CleanUp();
            GamePlays.ScrollUpdater.CleanUp();
            GamePlays.NoteJudgeUpdater.CleanUp();
            GamePlays.MotionUpdater.CleanUp();
            GamePlays.GraphicUpdater.CleanUp();
            ScoreManager.Reset();
        }
    }
}