using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Loading;
using UnityEngine.SceneManagement;

namespace GameStates
{
    public abstract partial class BaseState
    {
        protected static ILoadingWorker Loading { get; private set; }

        public virtual void Setup()
        {
            if (Loading == null)
            {
                Loading = LoadingWorker.Instance;
            }
        }

        public virtual bool IsChangeAllowed(GameStateType nextState)
        {
            return true;
        }
        
        public virtual void Enter()
        {

        }

        public virtual void Exit(GameStateType nextState)
        {

        }

        public virtual void GamePaused()
        {

        }

        public virtual void GameResumed()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void EnterLoadJob()
        {

        }

        public virtual void ExitLoadJob(GameStateType nextState)
        {

        }
    }
}
