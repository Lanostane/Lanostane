using Cysharp.Threading.Tasks;
using Loadings;
using NaughtyAttributes;
//using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;
using Utils.Unity;

namespace LST.GamePlay
{
    public interface IGamePlayLoader
    {
        public TextAsset ChartToLoad { get; set; }
        public AudioClip MusicToPlay { get; set; }
    }

    public static class GamePlayLoader
    {
        public static event Action OnLoaded;
        public static event Action OnUnloaded;

        public static IGamePlayLoader Instance { get; internal set; }

        internal static void Invoke_OnLoaded()
        {
            OnLoaded?.Invoke();
        }

        internal static void Invoke_OnUnloaded()
        {
            OnUnloaded?.Invoke();
        }
    }
}
