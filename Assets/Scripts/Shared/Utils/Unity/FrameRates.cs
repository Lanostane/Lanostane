using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Unity
{
    public static class FrameRates
    {
        public enum Mode
        {
            Unlimited,
            MaxTo60,
            MaxToScreenRefreshRate
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            if (Platform.IsMobile)
            {
                SetMode(Mode.MaxTo60);
            }
            else if (Platform.IsWindows)
            {
                SetMode(Mode.Unlimited);
            }
        }

        public static void SetMode(Mode mode)
        {
            switch (mode)
            {
                case Mode.Unlimited:
                    Application.targetFrameRate = -1;
                    break;

                case Mode.MaxTo60:
                    Application.targetFrameRate = 60;
                    break;

                case Mode.MaxToScreenRefreshRate:
                    Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
                    break;
            }
        }
    }
}
