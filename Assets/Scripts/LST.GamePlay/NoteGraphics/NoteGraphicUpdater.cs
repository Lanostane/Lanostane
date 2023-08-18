using Lanostane.Models;
using LST.GamePlay.Scrolls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Utils.Maths;
using Utils.Unity;

namespace LST.GamePlay.Graphics
{
    internal sealed partial class NoteGraphicUpdater : MonoBehaviour, INoteGraphicUpdater
    {
        public Transform NoteOrigin;

        public event Action<float, ISingleNoteGraphic> OnSingleNoteProgressUpdate;
        public event Action<float, ILongNoteGraphic> OnLongNoteProgressUpdate;

        void Awake()
        {
            GamePlays.GraphicUpdater = this;
        }

        void OnDestroy()
        {
            GamePlays.GraphicUpdater = null;
        }

        public void Prepare()
        {
            
        }

        public void TimeUpdate(float chartTime)
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
