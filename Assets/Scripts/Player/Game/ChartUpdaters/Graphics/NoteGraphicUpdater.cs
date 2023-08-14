using Lanostane.Models;
using LST.Player.Scrolls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Utils.Maths;
using Utils.Unity;

namespace LST.Player.Graphics
{
    public sealed partial class NoteGraphicUpdater : MonoBehaviour, INoteGraphicUpdater
    {
        public Transform NoteOrigin;

        public event Action<float, ISingleNoteGraphic> OnSingleNoteProgressUpdate;
        public event Action<float, ILongNoteGraphic> OnLongNoteProgressUpdate;

        void Awake()
        {
            GamePlay.GraphicUpdater = this;
        }

        void OnDestroy()
        {
            GamePlay.GraphicUpdater = null;
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
