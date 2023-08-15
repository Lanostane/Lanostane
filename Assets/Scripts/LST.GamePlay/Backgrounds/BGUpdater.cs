using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace LST.GamePlay.BG
{
    
    public class BGUpdater : MonoBehaviour, IBGUpdater
    {
        public VideoPlayer VP;

        void Awake()
        {
            GamePlays.BGUpdater = this;
        }

        void OnDestroy()
        {
            GamePlays.BGUpdater = null;
        }

        public void TimeUpdate(float chartTime)
        {
            
        }

        public void CleanUp()
        {
            
        }
    }
}
