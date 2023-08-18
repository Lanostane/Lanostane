using LST.GamePlay;
using NaughtyAttributes;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Utils.FileSystems;

namespace LST.Player
{
    public static class PlayerSettings
    {
        public static readonly PlayerSettingsData UserData = new();
        public static readonly DebugSettingsData DebugData = new();


        public static void LoadFromDisk()
        {
            //var config = Paths.Data.ReadTextFile("config.json");
            //UserData = JsonConvert.DeserializeObject<PlayerSettingsData>(config);
        }

        public static void SaveToDisk()
        {
            //
            //var config = Paths.Data.ReadTextFile("config.json");
            //UserData = JsonConvert.DeserializeObject<PlayerSettingsData>(config);
        }
    }
}
