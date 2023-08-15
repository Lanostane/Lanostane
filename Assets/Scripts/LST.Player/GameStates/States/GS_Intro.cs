using Loadings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LST.Player.States
{
    public class GS_Intro : BaseState
    {
        public override bool IsChangeAllowed(GameStateType newType)
        {
            return newType switch
            {
                GameStateType.MainUI => true,
                GameStateType.GamePlay => true,
                _ => false
            };
        }

        public override void Enter()
        {
            
        }

        public override void EnterLoadJob()
        {
            Loading.AddSceneLoadJob(SceneName.Main);
        }
    }
}
