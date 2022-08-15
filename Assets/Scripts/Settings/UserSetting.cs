using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Settings
{
    public sealed class UserSetting_DTO
    {
        public int Offset = 0;
        public bool UsingHitSounds = true;
    }

    public static class UserSetting
    {
        private static UserSetting_DTO _Settings = new();
        private readonly static string _ConfigPath = Path.Combine(Application.persistentDataPath, "config.json");

        public static int Offset
        {
            get => _Settings.Offset;
            set => _Settings.Offset = value;
        }

        public static void Load()
        {
            if (File.Exists(_ConfigPath))
            {
                try
                {
                    var json = File.ReadAllText(_ConfigPath);
                    _Settings = JsonConvert.DeserializeObject<UserSetting_DTO>(json);
                }
                catch
                {
                    File.Delete(_ConfigPath);
                }
            }
        }

        public static void Save()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_Settings);
                File.WriteAllText(_ConfigPath, json);
            }
            catch
            {
                
            }
        }
    }
}
