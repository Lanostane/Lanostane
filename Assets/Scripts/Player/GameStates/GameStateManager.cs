using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LST.Player.States
{
    public static class GameStateManager
    {
        public static IGameStateUpdater StateUpdater { get; internal set; }
    }
}
