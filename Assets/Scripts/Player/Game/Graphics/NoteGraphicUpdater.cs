﻿using Lanostane.Charts;
using LST.Player.Scrolls;
using System.Collections.Generic;
using UnityEngine;
using Utils.Maths;

namespace LST.Player.Graphics
{
    public sealed partial class NoteGraphicUpdater : MonoBehaviour, INoteGraphicUpdater
    {
        public Transform NoteOrigin;

        void Awake()
        {
            GamePlayManager.GraphicUpdater = this;
        }

        void OnDestroy()
        {
            GamePlayManager.GraphicUpdater = null;
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
