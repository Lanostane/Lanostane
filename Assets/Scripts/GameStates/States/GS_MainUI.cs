using Lanostane;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;

namespace GameStates.States
{
    public class GS_MainUI : BaseState
    {
        public override bool IsChangeAllowed(GameStateType newType)
        {
            return newType switch
            {
                GameStateType.Intro => true,
                GameStateType.GamePlay => true,
                GameStateType.GameDone => true,
                _ => false
            };
        }

        public override void Enter()
        {
            UIManager.Instance.WantToChangeState(UIMainState.MainMenu);
        }

        public override void EnterLoadJob()
        {
            Loading.AddSceneLoadJob(SceneName.MainUI);
        }
    }
}
