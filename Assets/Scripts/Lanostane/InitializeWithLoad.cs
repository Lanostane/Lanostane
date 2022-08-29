using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lanostane
{
    internal static class InitializeWithLoad
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Start()
        {
#if !UNITY_EDITOR
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
#endif
        }
    }
}
