using Cysharp.Threading.Tasks;
using Lanostane.Models;
using LST.GamePlay.Scrolls;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utils.Maths;

namespace LST.GamePlay.Graphics
{
    internal sealed partial class NoteGraphicUpdater : MonoBehaviour, INoteGraphicUpdater
    {
        public AssetReference Size0SinglePrefab;
        public AssetReference Size1SinglePrefab;
        public AssetReference Size2SinglePrefab;

        private readonly SingleNoteGraphicCollection _Singles = new();

        public async UniTask<ISingleNoteGraphic> AddSingleNote(LST_SingleNoteInfo info)
        {
            var note = await InstantiateSingleNote(info.Size);
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

        private async UniTask<GameObject> InstantiateSingleNote(LST_NoteSize size)
        {
            var obj = await (size.ToValidSize() switch
            {
                LST_NoteSize.Size0 => Size0SinglePrefab.InstantiateAsync(NoteOrigin).ToUniTask(),
                LST_NoteSize.Size1 => Size1SinglePrefab.InstantiateAsync(NoteOrigin).ToUniTask(),
                LST_NoteSize.Size2 => Size2SinglePrefab.InstantiateAsync(NoteOrigin).ToUniTask(),
                _ => Size0SinglePrefab.InstantiateAsync(NoteOrigin).ToUniTask()
            });
            return obj;
        }
    }
}
