using LST.Player.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LST.Player.States
{
    public interface IGameStateUpdater
    {
        BaseState NowState { get; }
        GameStateType NowStateEnum { get; }
        void WantToChangeState(GameStateType type);
    }

    public enum GameStateType
    {
        InitialLoading,
        MainUI,
        GamePlay,
        GameDone,
    }

    public sealed class GameStateUpdater : MonoBehaviour, IGameStateUpdater
    {
        public GS_Intro Intro { get; private set; } = new();
        public GS_MainUI MainUI { get; private set; } = new();
        public GS_GamePlay GamePlay { get; private set; } = new();
        public GS_GameDone GameDone { get; private set; } = new();

        public BaseState NowState { get; private set; }
        public GameStateType NowStateEnum { get; private set; }

        private bool _ApplicationPaused = false;
        private bool _LoadingInProgress = false;


        void Awake()
        {
            if (GameStateManager.StateUpdater != null)
            {
                Destroy(this);
                return;
            }

            GameStateManager.StateUpdater = this;
            transform.SetParent(null);

            Intro.Setup();
            MainUI.Setup();
            GamePlay.Setup();
            GameDone.Setup();

            NowState = Intro;
            NowStateEnum = GameStateType.InitialLoading;

            WantToChangeState(GameStateType.InitialLoading);
        }

        void Update()
        {
            NowState.Update();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus != _ApplicationPaused)
            {
                _ApplicationPaused = pauseStatus;
                if (pauseStatus)
                {
                    NowState.GamePaused();
                }
                else
                {
                    NowState.GameResumed();
                }
            }
        }

        public void WantToChangeState(GameStateType nextStateType)
        {
            if (_LoadingInProgress)
                return;

            if (nextStateType == NowStateEnum)
                return;

            var nowState = NowState;
            var nextState = GetStateByType(nextStateType);

            if (!nowState.IsChangeAllowed(nextStateType))
                return;

            nowState.ExitLoadJob(nextStateType);
            nextState.EnterLoadJob();

            _LoadingInProgress = true;
            LoadingWorker.Instance.StartLoading(LoadingStyle.Default, () =>
            {
                GC.Collect();

                _LoadingInProgress = false;
                NowState = nextState;
                NowStateEnum = nextStateType;

                nowState.Exit(nextStateType);
                nextState.Enter();
            });
        }

        private void SetNowState(GameStateType state)
        {
            NowState = GetStateByType(state);
            NowStateEnum = state;
        }

        private BaseState GetStateByType(GameStateType type)
        {
            return type switch
            {
                GameStateType.InitialLoading => Intro,
                GameStateType.MainUI => MainUI,
                GameStateType.GamePlay => GamePlay,
                GameStateType.GameDone => GameDone,
                _ => throw new NotImplementedException($"GameStateType: {type} is not implemented!!!"),
            };
        }
    }
}
