using Cysharp.Threading.Tasks;
using Lanostane.Models;
using LST.GamePlay.Scrolls;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils.Maths;

namespace LST.GamePlay.Graphics
{
    internal sealed partial class NoteGraphicUpdater : MonoBehaviour, INoteGraphicUpdater
    {
        public AssetReferenceGameObject Size0HoldPrefab;
        public AssetReferenceGameObject Size1HoldPrefab;
        public AssetReferenceGameObject Size2HoldPrefab;

        private readonly LongNoteGraphicCollection _Longs = new();
        public async UniTask<ILongNoteGraphic> AddLongNote(LST_LongNoteInfo info)
        {
            var note = await InstantiateLongNote(info.Size);
            var graphic = note.GetComponent<ILongNoteGraphic>();
            graphic.Setup(info);

            _Longs.Add(graphic);

            return graphic;
        }

        private void UpdateLongNotes(float chartTime)
        {
            ScrollProgressUpdateJob.Update(_Longs.HeadScrollTimings, _Longs.HeadScrollProgressBuffer);

            var graphics = _Longs.Graphics;
            var scrollPs = _Longs.HeadScrollProgressBuffer;
            for (int i = 0; i < graphics.Length; i++)
            {
                var note = graphics[i];
                var scrollP = scrollPs[i];

                if (note.JudgeDone)
                {
                    note.Hide();
                    continue;
                }

                if (note.UpdateVisibility(chartTime))
                {
                    note.UpdateProgress(scrollP.EasedProgress, chartTime);
                    OnLongNoteProgressUpdate?.Invoke(scrollP.EasedProgress, note);
                }
            }
        }

        private async UniTask<GameObject> InstantiateLongNote(LST_NoteSize size)
        {
            var obj = await (size.ToValidSize() switch
            {
                LST_NoteSize.Size0 => Size0HoldPrefab.InstantiateAsync(NoteOrigin).ToUniTask(),
                LST_NoteSize.Size1 => Size1HoldPrefab.InstantiateAsync(NoteOrigin).ToUniTask(),
                LST_NoteSize.Size2 => Size2HoldPrefab.InstantiateAsync(NoteOrigin).ToUniTask(),
                _ => Size0HoldPrefab.InstantiateAsync(NoteOrigin).ToUniTask()
            });
            return obj;
        }
    }
}
