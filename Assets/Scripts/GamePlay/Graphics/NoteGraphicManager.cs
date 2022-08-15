using Charts;
using GamePlay.Graphics.Collections;
using GamePlay.Scrolls;
using UnityEngine;
using Utils;

namespace GamePlay.Graphics
{
    public interface INoteGraphicManager : IChartUpdater
    {
        ISingleNoteGraphic AddSingleNote(LST_SingleNoteInfo info);
        ILongNoteGraphic AddLongNote(LST_LongNoteInfo info);
    }

    public sealed class NoteGraphicManager : MonoBehaviour, INoteGraphicManager
    {
        public static INoteGraphicManager Instance { get; private set; }

        public Transform NoteOrigin;
        public GameObject Size0SinglePrefab;
        public GameObject Size1SinglePrefab;
        public GameObject Size2SinglePrefab;

        public GameObject Size0HoldPrefab;
        public GameObject Size1HoldPrefab;
        public GameObject Size2HoldPrefab;

        private readonly SingleNoteGraphicCollection _Singles = new();
        private readonly LongNoteGraphicCollection _Longs = new();

        void Awake()
        {
            Instance = this;
        }

        void OnDestroy()
        {
            Instance = null;
        }

        public ISingleNoteGraphic AddSingleNote(LST_SingleNoteInfo info)
        {
            GameObject note = Instantiate(GetSinglePrefab(info.Size), NoteOrigin);
            var graphic = note.GetComponent<ISingleNoteGraphic>();
            graphic.Setup(info);

            _Singles.Add(graphic);

            return graphic;
        }

        public ILongNoteGraphic AddLongNote(LST_LongNoteInfo info)
        {
            GameObject note = Instantiate(GetLongPrefab(info.Size), NoteOrigin);
            var graphic = note.GetComponent<ILongNoteGraphic>();
            graphic.Setup(info);

            _Longs.Add(graphic);

            return graphic;
        }

        public void UpdateChart(float chartTime)
        {
            UpdateSingleNotes(chartTime);
            UpdateLongNotes(chartTime);
        }



        public void CleanUp()
        {
            _Singles.Clear(destroy: true);
            _Longs.Clear(destroy: true);
        }

        private void UpdateSingleNotes(float chartTime)
        {
            ScrollAmountInfoBuildJob.Run_NoAlloc(
                ScrollManager.Instance.WatchingFrom,
                ScrollManager.Instance.WatchingTo,
                _Singles.ScrollAmounts,
                _Singles.ScrollAmountsBuffer);

            var graphics = _Singles.Graphics;
            var amounts = _Singles.ScrollAmountsBuffer;
            for (int i = 0; i < graphics.Length; i++)
            {
                var note = graphics[i];
                var info = amounts[i];

                if (note.JudgeDone)
                {
                    note.Hide();
                    continue;
                }

                if (!info.IsVisible && !MathfE.AbsApprox(chartTime, note.Timing, 0.2f))
                {
                    note.Hide();
                    continue;
                }
                note.UpdateProgress(info.EasedProgress);
            }
        }

        private void UpdateLongNotes(float chartTime)
        {
            ScrollAmountInfoBuildJob.Run_NoAlloc(
                ScrollManager.Instance.WatchingFrom,
                ScrollManager.Instance.WatchingTo,
                _Longs.HeadScrollAmounts,
                _Longs.HeadScrollAmountsBuffer);

            var graphics = _Longs.Graphics;
            var amounts = _Longs.HeadScrollAmountsBuffer;
            for (int i = 0; i < graphics.Length; i++)
            {
                var note = graphics[i];
                var info = amounts[i];

                if (note.JudgeDone)
                {
                    note.Hide();
                    continue;
                }

                if (note.UpdateVisibility(chartTime))
                {
                    note.UpdateProgress(info.EasedProgress, chartTime);
                }
            }
        }

        private GameObject GetSinglePrefab(LST_NoteSize size)
        {
            return size.ToValidSize() switch
            {
                LST_NoteSize.Size0 => Size0SinglePrefab,
                LST_NoteSize.Size1 => Size1SinglePrefab,
                LST_NoteSize.Size2 => Size2SinglePrefab,
                _ => Size0SinglePrefab
            };
        }

        private GameObject GetLongPrefab(LST_NoteSize size)
        {
            return size.ToValidSize() switch
            {
                LST_NoteSize.Size0 => Size0HoldPrefab,
                LST_NoteSize.Size1 => Size1HoldPrefab,
                LST_NoteSize.Size2 => Size2HoldPrefab,
                _ => Size0HoldPrefab
            };
        }
    }
}
