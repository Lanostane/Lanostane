using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;

namespace LST.Player
{
    internal static class InitializeWithLoad
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Start()
        {
            //Application.targetFrameRate = Screen.currentResolution.refreshRate;
        }
    }
}
