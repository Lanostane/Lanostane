using GameStates.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStates
{
    public static class GameStateManager
    {
        public static IGameStateUpdater StateUpdater { get; internal set; }
    }
}
