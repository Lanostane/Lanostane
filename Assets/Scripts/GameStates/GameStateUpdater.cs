using GameStates.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Loading;
using UnityEngine;

namespace GameStates
{
    public interface IGameStateUpdater
    {
        BaseState NowState { get; }
        GameStateType NowStateEnum { get; }
        void WantToChangeState(GameStateType type);
    }

    public enum GameStateType
    {
        Intro,
        MainUI,
        SongSelection,
        GamePlay,
        GameDone,
    }

    public sealed class GameStateUpdater : MonoBehaviour, IGameStateUpdater
    {
        public GS_Intro Intro { get; private set; } = new();
        public GS_MainUI MainUI { get; private set; } = new();
        public GS_SongSelection SongSelection { get; private set; } = new();
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
            DontDestroyOnLoad(this);

            Intro.Setup();
            MainUI.Setup();
            SongSelection.Setup();
            GamePlay.Setup();
            GameDone.Setup();

            NowState = Intro;
            NowStateEnum = GameStateType.Intro;

            WantToChangeState(GameStateType.MainUI);
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
            LoadingWorker.Instance.DoLoading(LoadingStyle.Default, () =>
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
                GameStateType.Intro => Intro,
                GameStateType.MainUI => MainUI,
                GameStateType.SongSelection => SongSelection,
                GameStateType.GamePlay => GamePlay,
                GameStateType.GameDone => GameDone,
                _ => throw new NotImplementedException($"GameStateType: {type} is not implemented!!!"),
            };
        }
    }
}
