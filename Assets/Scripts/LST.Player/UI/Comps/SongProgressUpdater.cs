using LST.GamePlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LST.Player.UI
{
    public class SongProgressUpdater : MonoBehaviour
    {
        public RectTransform BaseRect;
        public Transform Knob;

        void FixedUpdate()
        {
            if (GamePlays.GamePlayLoaded)
            {
                if (GamePlays.ChartPlayer.MusicTime > 0.0f)
                {
                    var p = GamePlays.ChartPlayer.ChartTime / GamePlays.ChartPlayer.MusicTime;
                    var p1 = 0.0f;
                    var p2 = BaseRect.rect.size.x;
                    Knob.localPosition = Vector3.right * Mathf.Lerp(p1, p2, p);
                }
            }
        }
    }
}
