using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Unity
{
    public static class Platform
    {
        public static bool IsWindows { get; private set; }
        public static bool IsMobile { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            var platform = Application.platform;
            IsWindows =
                platform == RuntimePlatform.WindowsPlayer ||
                platform == RuntimePlatform.WindowsEditor;

            IsMobile = Application.isMobilePlatform;
        }
    }
}
