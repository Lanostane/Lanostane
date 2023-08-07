﻿using LST.Player.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Providers
{
    [CreateAssetMenu(fileName = "GS_Provider", menuName = "Providers/GameState")]
    public class GameStateProvider : ScriptableObject
    {
        public static BaseState NowState => GameStateManager.StateUpdater.NowState;

        public static GameStateType NowStateEnum => GameStateManager.StateUpdater.NowStateEnum;

        public static void WantToChangeState(int type)
        {
            GameStateManager.StateUpdater.WantToChangeState((GameStateType)type);
        }

        public static void WantToSetIntroState()
        {
            GameStateManager.StateUpdater.WantToChangeState(GameStateType.InitialLoading);
        }

        public static void WantToSetMainUIState()
        {
            GameStateManager.StateUpdater.WantToChangeState(GameStateType.MainUI);
        }

        public static void WantToSetGamePlayState()
        {
            GameStateManager.StateUpdater.WantToChangeState(GameStateType.GamePlay);
        }

        public static void WantToSetGameDoneState()
        {
            GameStateManager.StateUpdater.WantToChangeState(GameStateType.GameDone);
        }
    }
}
