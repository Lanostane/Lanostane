using Lst.Charts;
using GamePlay.Graphics.Collections;
using GamePlay.Scrolls;
using GamePlay.Scrolls.Jobs;
using UnityEngine;
using Utils.Maths;

namespace GamePlay.Graphics
{
    public sealed partial class NoteGraphicManager : MonoBehaviour, INoteGraphicManager
    {
        public GameObject Size0SinglePrefab;
        public GameObject Size1SinglePrefab;
        public GameObject Size2SinglePrefab;

        private readonly SingleNoteGraphicCollection _Singles = new();

        public ISingleNoteGraphic AddSingleNote(LST_SingleNoteInfo info)
        {
            GameObject note = Instantiate(GetSinglePrefab(info.Size), NoteOrigin);
            var graphic = note.GetComponent<ISingleNoteGraphic>();
            graphic.Setup(info);

            _Singles.Add(graphic);

            return graphic;
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
    }
}
