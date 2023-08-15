using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils.FileSystems.PathProxy;

namespace Utils.FileSystems
{
    public static class Paths
    {
        public static string PersistentDataPath { get; private set; }
        public readonly static ChartsPathProxy Charts = new();
        public readonly static DataPathProxy Data = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Setup()
        {
            PersistentDataPath = Application.persistentDataPath;

            Charts.Setup();
            Data.Setup();
        }
    }
}
