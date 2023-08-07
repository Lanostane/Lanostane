using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LST.Player
{
    public enum GameStates
    {
        MainMenu,
        ChartLoaded,
        GamePlay,
        GamePlay_Paused,
        GamePlay_Result,
    }

    public static class GameState
    {
        internal static GameState_Impl Instance { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {

        }
    }
}
