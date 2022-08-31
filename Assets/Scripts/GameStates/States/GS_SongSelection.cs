using Lanostane;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStates.States
{
    public class GS_SongSelection : BaseState
    {
        public override bool IsChangeAllowed(GameStateType newType)
        {
            return newType switch
            {
                GameStateType.GamePlay => true,
                GameStateType.MainUI => true,
                _ => false
            };
        }

        public override void Enter()
        {
            
        }

        public override void EnterLoadJob()
        {
            Loading.AddSceneLoadJob(SceneName.GamePlay);
        }
    }
}
