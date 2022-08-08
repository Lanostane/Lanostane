using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePlay.GUI
{
    public class SongProgressUpdater : MonoBehaviour
    {
        public RectTransform BaseRect;
        public Transform Knob;

        void FixedUpdate()
        {
            if (ChartPlayer.MusicTime > 0.0f)
            {
                var p = ChartPlayer.ChartTime / ChartPlayer.MusicTime;
                var p1 = 0.0f;
                var p2 = BaseRect.rect.size.x;
                Knob.localPosition = Vector3.right * Mathf.Lerp(p1, p2, p);
            }
        }
    }
}
