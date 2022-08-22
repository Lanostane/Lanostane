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
        public readonly static DataPathProxy Data = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Setup()
        {
            Data.Setup();
        }
    }
}
