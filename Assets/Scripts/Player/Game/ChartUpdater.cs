using Lanostane.Models;
using LST.Player.Scoring;
using System.Collections;
using UnityEngine;

namespace LST.Player
{
    public class ChartUpdater : IChartUpdater
    {
        public void Setup(LST_Chart chart)
        {
            GamePlay.MotionUpdater.SetDefaultMotion(chart.Default);
            GamePlay.MotionUpdater.AddMotions(chart);
            GamePlay.MotionUpdater.Prepare();

            foreach (var scroll in chart.Scrolls)
            {
                GamePlay.ScrollUpdater.AddScroll(scroll);
            }
            GamePlay.ScrollUpdater.Prepare();

            foreach (var note in chart.TapNotes)
            {
                var graphic = GamePlay.GraphicUpdater.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength && !note.Flags.HasFlag(LST_NoteSpecialFlags.NoJudgement))
                    GamePlay.NoteJudgeUpdater.AddSingleJudgeHandle(note.NoteInfo, graphic);
            }

            foreach (var note in chart.CatchNotes)
            {
                var graphic = GamePlay.GraphicUpdater.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength && !note.Flags.HasFlag(LST_NoteSpecialFlags.NoJudgement))
                    GamePlay.NoteJudgeUpdater.AddSingleJudgeHandle(note.NoteInfo, graphic);
            }

            foreach (var note in chart.FlickNotes)
            {
                var graphic = GamePlay.GraphicUpdater.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength && !note.Flags.HasFlag(LST_NoteSpecialFlags.NoJudgement))
                    GamePlay.NoteJudgeUpdater.AddSingleJudgeHandle(note.NoteInfo, graphic);
            }

            foreach (var note in chart.HoldNotes)
            {
                var graphic = GamePlay.GraphicUpdater.AddLongNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength && !note.Flags.HasFlag(LST_NoteSpecialFlags.NoJudgement))
                    GamePlay.NoteJudgeUpdater.AddLongJudgeHandle(note.NoteInfo, graphic);
            }

            GamePlay.GraphicUpdater.Prepare();
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