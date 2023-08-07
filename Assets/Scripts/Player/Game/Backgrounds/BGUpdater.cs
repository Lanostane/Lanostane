using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace LST.Player.Backgrounds {
    
    public class BGUpdater : MonoBehaviour, IBGUpdater
    {
        public VideoPlayer VP;

        void Awake()
        {
            GamePlayManager.BGUpdater = this;
        }

        void OnDestroy()
        {
            GamePlayManager.BGUpdater = null;
        }

        public void TimeUpdate(float chartTime)
        {
            
        }

        public void CleanUp()
        {
            
        }
    }
}
