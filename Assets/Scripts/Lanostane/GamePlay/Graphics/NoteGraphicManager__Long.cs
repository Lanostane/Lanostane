using Lst.Charts;
using Lst.GamePlay.Graphics.Collections;
using Lst.GamePlay.Scrolls;
using Lst.GamePlay.Scrolls.Jobs;
using UnityEngine;
using Utils.Maths;

namespace Lst.GamePlay.Graphics
{
    public sealed partial class NoteGraphicManager : MonoBehaviour, INoteGraphicManager
    {
        public GameObject Size0HoldPrefab;
        public GameObject Size1HoldPrefab;
        public GameObject Size2HoldPrefab;

        private readonly LongNoteGraphicCollection _Longs = new();
        public ILongNoteGraphic AddLongNote(LST_LongNoteInfo info)
        {
            GameObject note = Instantiate(GetLongPrefab(info.Size), NoteOrigin);
            var graphic = note.GetComponent<ILongNoteGraphic>();
            graphic.Setup(info);

            _Longs.Add(graphic);

            return graphic;
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
