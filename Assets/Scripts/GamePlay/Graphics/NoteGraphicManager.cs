using Lst.Charts;
using GamePlay.Graphics.Collections;
using GamePlay.Scrolls;
using GamePlay.Scrolls.Jobs;
using UnityEngine;
using Utils.Maths;

namespace GamePlay.Graphics
{
    public interface INoteGraphicManager : IChartUpdater
    {
        ISingleNoteGraphic AddSingleNote(LST_SingleNoteInfo info);
        ILongNoteGraphic AddLongNote(LST_LongNoteInfo info);
    }

    public sealed partial class NoteGraphicManager : MonoBehaviour, INoteGraphicManager
    {
        public static INoteGraphicManager Instance { get; private set; }

        public Transform NoteOrigin;

        void Awake()
        {
            Instance = this;
        }

        void OnDestroy()
        {
            Instance = null;
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
    }
}
