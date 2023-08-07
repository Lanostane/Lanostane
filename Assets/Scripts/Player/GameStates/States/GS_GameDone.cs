using Lanostane;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LST.Player.States
{
    public class GS_GameDone : BaseState
    {
        public override bool IsChangeAllowed(GameStateType newType)
        {
            return newType switch
            {
                GameStateType.GamePlay => true,
                _ => false
            };
        }

        public override void Enter()
        {
            
        }

        public override void ExitLoadJob(GameStateType nextState)
        {
            Loading.AddSceneUnloadJob(SceneName.GamePlay);
        }
    }
}
