using GamePlay;
using Lanostane;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;

namespace GameStates.States
{
    public class GS_GamePlay : BaseState
    {
        public override bool IsChangeAllowed(GameStateType newType)
        {
            return newType switch
            {
                GameStateType.MainUI => true,
                GameStateType.SongSelection => true,
                _ => false
            };
        }

        public override void Enter()
        {
            UIManager.Instance.WantToChangeState(UIMainState.GamePlay);
        }

        public override void GamePaused()
        {
            //TODO: Pause Screen
            UIManager.Overlays.GamePause.SetActive(true);
            GamePlayManager.Player.Pause();
        }

        public override void EnterLoadJob()
        {
            Loading.AddSceneLoadJob(SceneName.GamePlay);
        }

        public override void ExitLoadJob(GameStateType nextState)
        {
            if (nextState == GameStateType.MainUI)
            {
                Loading.AddSceneUnloadJob(SceneName.GamePlay);
            }
        }
    }
}
