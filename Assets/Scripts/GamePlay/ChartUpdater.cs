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
            MotionManager.Instance.SetDefaultMotion(chart.Default);
            MotionManager.Instance.AddMotions(chart);
            MotionManager.Instance.UpdateAbsValue();

            foreach (var scroll in chart.Scrolls)
            {
                ScrollManager.Instance.AddScroll(scroll);
            }
            ScrollManager.Instance.UpdateAbsValue();

            foreach (var note in chart.TapNotes)
            {
                var graphic = NoteGraphicManager.Instance.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength)
                    NoteJudgeManager.Instance.AddSingleJudgeHandle(note.NoteInfo, graphic);
            }

            foreach (var note in chart.CatchNotes)
            {
                var graphic = NoteGraphicManager.Instance.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength)
                    NoteJudgeManager.Instance.AddSingleJudgeHandle(note.NoteInfo, graphic);
            }

            foreach (var note in chart.FlickNotes)
            {
                var graphic = NoteGraphicManager.Instance.AddSingleNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength)
                    NoteJudgeManager.Instance.AddSingleJudgeHandle(note.NoteInfo, graphic);
            }

            foreach (var note in chart.HoldNotes)
            {
                var graphic = NoteGraphicManager.Instance.AddLongNote(note.NoteInfo);
                if (note.Timing <= chart.SongLength)
                    NoteJudgeManager.Instance.AddLongJudgeHandle(note.NoteInfo, graphic);
            }

            NoteJudgeManager.Instance.InitializeScoring();
        }

        public void UpdateChart(float chartTime)
        {
            ScrollManager.Instance.UpdateChart(chartTime);
            NoteJudgeManager.Instance.UpdateChart(chartTime);
            MotionManager.Instance.UpdateChart(chartTime);
            NoteGraphicManager.Instance.UpdateChart(chartTime);
        }

        public void CleanUp()
        {
            ScrollManager.Instance.CleanUp();
            NoteJudgeManager.Instance.CleanUp();
            MotionManager.Instance.CleanUp();
            NoteGraphicManager.Instance.CleanUp();
        }
    }
}