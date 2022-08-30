using Lanostane.Charts;
using GamePlay.Graphics;
using GamePlay.Judge;
using GamePlay.Motions;
using GamePlay.Scrolls;
using System.Collections;
using UnityEngine;

namespace GamePlay
{
    public class ChartUpdater : IChartUpdater
    {
        public void Setup(LST_Chart chart)
        {
            MotionUpdater.Instance.SetDefaultMotion(chart.Default);
            MotionUpdater.Instance.AddMotions(chart);
            MotionUpdater.Instance.UpdateAbsValue();

            foreach (var scroll in chart.Scrolls)
            {
                ScrollUpdater.Instance.AddScroll(scroll);
            }
            ScrollUpdater.Instance.UpdateAbsValue();

            foreach (var note in chart.TapNotes)
            {
                var graphic = NoteGraphicUpdater.Instance.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength)
                    NoteJudgeUpdater.Instance.AddSingleJudgeHandle(note.NoteInfo, graphic);
            }

            foreach (var note in chart.CatchNotes)
            {
                var graphic = NoteGraphicUpdater.Instance.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength)
                    NoteJudgeUpdater.Instance.AddSingleJudgeHandle(note.NoteInfo, graphic);
            }

            foreach (var note in chart.FlickNotes)
            {
                var graphic = NoteGraphicUpdater.Instance.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength)
                    NoteJudgeUpdater.Instance.AddSingleJudgeHandle(note.NoteInfo, graphic);
            }

            foreach (var note in chart.HoldNotes)
            {
                var graphic = NoteGraphicUpdater.Instance.AddLongNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength)
                    NoteJudgeUpdater.Instance.AddLongJudgeHandle(note.NoteInfo, graphic);
            }

            NoteJudgeUpdater.Instance.InitializeScoring();
        }

        public void UpdateChart(float chartTime)
        {
            ScrollUpdater.Instance.UpdateChart(chartTime);
            NoteJudgeUpdater.Instance.UpdateChart(chartTime);
            MotionUpdater.Instance.UpdateChart(chartTime);
            NoteGraphicUpdater.Instance.UpdateChart(chartTime);
        }

        public void CleanUp()
        {
            ScrollUpdater.Instance.CleanUp();
            NoteJudgeUpdater.Instance.CleanUp();
            MotionUpdater.Instance.CleanUp();
            NoteGraphicUpdater.Instance.CleanUp();
        }
    }
}