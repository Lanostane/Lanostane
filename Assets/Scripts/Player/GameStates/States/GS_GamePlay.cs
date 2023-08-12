using Lanostane;
using LST.Player.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LST.Player.States
{
    public class GS_GamePlay : BaseState
    {
        public override bool IsChangeAllowed(GameStateType newType)
        {
            return newType switch
            {
                GameStateType.MainUI => true,
                _ => false
            };
        }

        public override void Enter()
        {
            ChartPlayer.ChartPlayFinished += OnChartFinished;
        }

        public override void Exit(GameStateType nextState)
        {
            ChartPlayer.ChartPlayFinished -= OnChartFinished;
        }

        private void OnChartFinished()
        {
            UIManager.Overlays.GameResult.SetActive(true);
            UIManager.Overlays.GameHeader.SetActive(false);
        }

        public override void GamePaused()
        {
            //TODO: Pause Screen
            UIManager.Overlays.GamePause.SetActive(true);
            GamePlayManager.ChartPlayer.Pause();
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
