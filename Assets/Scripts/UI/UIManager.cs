using GamePlay;
using System.Collections;
using UI.Screens;
using UnityEngine;

namespace UI
{
    public interface IUIManager
    {
        void WantToChangeState(UIMainState state);
        void ForceChangeState(UIMainState state);
    }

    public enum UIMainState
    {
        Startup,
        MainMenu,
        GamePlay
    }

    [RequireComponent(typeof(ScreenHolder))]
    [RequireComponent(typeof(OverlayHolder))]
    public class UIManager : MonoBehaviour, IUIManager
    {
        public static IUIManager Instance { get; private set; }
        public static IScreenHolder Screens { get; private set; }
        public static IOverlayHolder Overlays { get; private set; }

        public Canvas HolderCanvas;

        private UIMainState _CurrentState = UIMainState.MainMenu;

        void Awake()
        {
            Instance = this;

            var screens = HolderCanvas.GetComponentsInChildren<BaseScreen>(includeInactive: true);
            var screenHolder = GetComponent<ScreenHolder>();
            screenHolder.Setup(screens);
            Screens = screenHolder;

            var overlayHolder = GetComponent<OverlayHolder>();
            overlayHolder.Setup(screens);
            Overlays = overlayHolder;

            ChartPlayer.ChartPlayFinished += ChartFinished;
        }

        void OnDestroy()
        {
            Instance = null;
            Overlays = null;
            ChartPlayer.ChartPlayFinished -= ChartFinished;
        }

        private void ChartFinished()
        {
            Debug.Log("Chart has Finished!");
            Overlays.GameResult.SetActive(true);
            Overlays.GameHeader.SetActive(false);
        }

        public void ForceChangeState(UIMainState state)
        {
            ChangeState(state);
        }

        public void WantToChangeState(UIMainState state)
        {
            ChangeState(state);
        }

        private void ChangeState(UIMainState state)
        {
            if (state == UIMainState.Startup)
            {
                Screens.Intro.SetActive(true);
                Screens.MainMenu.SetActive(false);
                Screens.Game.SetActive(false);
            }
            else if (state == UIMainState.MainMenu)
            {
                Screens.Intro.SetActive(false);
                Screens.MainMenu.SetActive(true);
                Screens.Game.SetActive(false);
            }
            else if (state == UIMainState.GamePlay)
            {
                Screens.Intro.SetActive(false);
                Screens.MainMenu.SetActive(false);
                Screens.Game.SetActive(true);
            }
        }
    }
}