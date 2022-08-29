using Lst.Settings.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lst.Settings
{
    public static class PlayerSetting
    {
        public static PlayerSetting_SO Settings { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Setup()
        {
            Settings = Resources.Load<PlayerSetting_SO>("PlayerSetting");
        }
    }
}
