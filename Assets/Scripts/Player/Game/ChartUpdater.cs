using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Lanostane.Models;
using LST.Player.Scoring;
using System;
using System.Collections;
using UnityEngine;

namespace LST.Player
{
    public class ChartUpdater : IChartUpdater
    {
        public async UniTask BuildFromChart(LST_Chart chart, IProgress<LoadChartSteps> progress)
        {
            progress?.Report(LoadChartSteps.S3_BuildMotions);
            GamePlay.MotionUpdater.AddFromChart(chart);
            await UniTask.Yield();

            progress?.Report(LoadChartSteps.S4_PrepareMotions);
            GamePlay.MotionUpdater.Prepare();
            await UniTask.Yield();

            progress?.Report(LoadChartSteps.S5_AddScrolls);
            GamePlay.ScrollUpdater.AddFromChart(chart);
            await UniTask.Yield();

            progress?.Report(LoadChartSteps.S6_PrepareScrolls);
            GamePlay.ScrollUpdater.Prepare();
            await UniTask.Yield();

            int jobCount = 0;
            int jobPerFrame = 5;
            progress?.Report(LoadChartSteps.S7_AddSingleNotes);
            foreach (var note in chart.TapNotes)
            {
                var graphic = GamePlay.GraphicUpdater.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength && !note.Flags.HasFlag(LST_NoteSpecialFlags.NoJudgement))
                    GamePlay.NoteJudgeUpdater.AddSingleJudgeHandle(note.NoteInfo, graphic);

                if (++jobCount >= jobPerFrame)
                {
                    jobCount = 0;
                    await UniTask.Yield();
                }
            }
            

            foreach (var note in chart.CatchNotes)
            {
                var graphic = GamePlay.GraphicUpdater.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength && !note.Flags.HasFlag(LST_NoteSpecialFlags.NoJudgement))
                    GamePlay.NoteJudgeUpdater.AddSingleJudgeHandle(note.NoteInfo, graphic);

                if (++jobCount >= jobPerFrame)
                {
                    jobCount = 0;
                    await UniTask.Yield();
                }
            }

            foreach (var note in chart.FlickNotes)
            {
                var graphic = GamePlay.GraphicUpdater.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength && !note.Flags.HasFlag(LST_NoteSpecialFlags.NoJudgement))
                    GamePlay.NoteJudgeUpdater.AddSingleJudgeHandle(note.NoteInfo, graphic);

                if (++jobCount >= jobPerFrame)
                {
                    jobCount = 0;
                    await UniTask.Yield();
                }
            }

            progress?.Report(LoadChartSteps.S8_AddLongNotes);
            foreach (var note in chart.HoldNotes)
            {
                var graphic = GamePlay.GraphicUpdater.AddLongNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength && !note.Flags.HasFlag(LST_NoteSpecialFlags.NoJudgement))
                    GamePlay.NoteJudgeUpdater.AddLongJudgeHandle(note.NoteInfo, graphic);

                if (++jobCount >= jobPerFrame)
                {
                    jobCount = 0;
                    await UniTask.Yield();
                }
            }
            await UniTask.Yield();

            progress?.Report(LoadChartSteps.S9_PrepareGraphics);
            GamePlay.GraphicUpdater.Prepare();
            await UniTask.Yield();

            progress?.Report(LoadChartSteps.S10_InitializeScoring);
            GamePlay.NoteJudgeUpdater.InitializeScoring();
        }

        public void TimeUpdate(float chartTime)
        {
            GamePlay.BGUpdater.TimeUpdate(chartTime);
            GamePlay.ScrollUpdater.TimeUpdate(chartTime);
            GamePlay.NoteJudgeUpdater.TimeUpdate(chartTime);
            GamePlay.MotionUpdater.TimeUpdate(chartTime);
            GamePlay.GraphicUpdater.TimeUpdate(chartTime);
        }

        public void CleanUp()
        {
            GamePlay.BGUpdater.CleanUp();
            GamePlay.ScrollUpdater.CleanUp();
            GamePlay.NoteJudgeUpdater.CleanUp();
            GamePlay.MotionUpdater.CleanUp();
            GamePlay.GraphicUpdater.CleanUp();
            ScoreManager.Reset();
        }
    }
}