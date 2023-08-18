using Lanostane.Models;
using LST.GamePlay.Scrolls;
using UnityEngine;
using Utils.Maths;

namespace LST.GamePlay.Graphics
{
    internal sealed partial class NoteGraphicUpdater : MonoBehaviour, INoteGraphicUpdater
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
            ScrollProgressUpdateJob.Update(_Singles.ScrollTimings, _Singles.ScrollProgressBuffer);

            var graphics = _Singles.Graphics;
            var scrollInfos = _Singles.ScrollProgressBuffer;
            for (int i = 0; i < graphics.Length; i++)
            {
                var note = graphics[i];
                var scrollInfo = scrollInfos[i];

                if (note.JudgeDone)
                {
                    note.Hide();
                    continue;
                }

                if (!scrollInfo.IsVisible)
                {
                    if (note.Flags.HasFlag(LST_NoteSpecialFlags.NoJudgement) || !MathfE.AbsApprox(chartTime, note.Timing, JudgeConst.Timeout))
                    {
                        note.Hide();
                        continue;
                    }
                }

                note.UpdateProgress(scrollInfo.EasedProgress);
                OnSingleNoteProgressUpdate?.Invoke(scrollInfo.EasedProgress, note);
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
