using Lanostane.Charts;
using System.Collections;
using UnityEngine;

namespace LST.Player
{
    public class ChartUpdater : IChartUpdater
    {
        public void Setup(LST_Chart chart)
        {
            GamePlayManager.MotionUpdater.SetDefaultMotion(chart.Default);
            GamePlayManager.MotionUpdater.AddMotions(chart);
            GamePlayManager.MotionUpdater.UpdateAbsValue();

            foreach (var scroll in chart.Scrolls)
            {
                GamePlayManager.ScrollUpdater.AddScroll(scroll);
            }
            GamePlayManager.ScrollUpdater.UpdateAbsValue();

            foreach (var note in chart.TapNotes)
            {
                var graphic = GamePlayManager.GraphicUpdater.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength)
                    GamePlayManager.NoteJudgeUpdater.AddSingleJudgeHandle(note.NoteInfo, graphic);
            }

            foreach (var note in chart.CatchNotes)
            {
                var graphic = GamePlayManager.GraphicUpdater.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength)
                    GamePlayManager.NoteJudgeUpdater.AddSingleJudgeHandle(note.NoteInfo, graphic);
            }

            foreach (var note in chart.FlickNotes)
            {
                var graphic = GamePlayManager.GraphicUpdater.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength)
                    GamePlayManager.NoteJudgeUpdater.AddSingleJudgeHandle(note.NoteInfo, graphic);
            }

            foreach (var note in chart.HoldNotes)
            {
                var graphic = GamePlayManager.GraphicUpdater.AddLongNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength)
                    GamePlayManager.NoteJudgeUpdater.AddLongJudgeHandle(note.NoteInfo, graphic);
            }

            GamePlayManager.NoteJudgeUpdater.InitializeScoring();
        }

        public void TimeUpdate(float chartTime)
        {
            GamePlayManager.BGUpdater.TimeUpdate(chartTime);
            GamePlayManager.ScrollUpdater.TimeUpdate(chartTime);
            GamePlayManager.NoteJudgeUpdater.TimeUpdate(chartTime);
            GamePlayManager.MotionUpdater.TimeUpdate(chartTime);
            GamePlayManager.GraphicUpdater.TimeUpdate(chartTime);
        }

        public void CleanUp()
        {
            GamePlayManager.BGUpdater.CleanUp();
            GamePlayManager.ScrollUpdater.CleanUp();
            GamePlayManager.NoteJudgeUpdater.CleanUp();
            GamePlayManager.MotionUpdater.CleanUp();
            GamePlayManager.GraphicUpdater.CleanUp();
        }
    }
}