using GamePlay;
using System.Collections;
using UI.Overlays;
using UnityEngine;

namespace UI
{
    public interface IUIManager
    {
        void ChangeMainState(UIMainState state);
    }

    public enum UIMainState
    {
        MainMenu,
        GamePlay
    }

    [RequireComponent(typeof(OverlayHolder))]
    public class UIManager : MonoBehaviour, IUIManager
    {
        public static IUIManager Instance { get; private set; }
        public static IOverlayHolder Overlays { get; private set; }

        public GameObject MainScreen;
        public GameObject GamePlayScreen;

        private UIMainState _CurrentState = UIMainState.MainMenu;

        void Awake()
        {
            Instance = this;
            Overlays = GetComponent<OverlayHolder>();
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
            Debug.Log("TEMP CODE, Active Result Screen");

        }

        public void ChangeMainState(UIMainState state)
        {
            if (state == UIMainState.MainMenu)
            {
                MainScreen.SetActive(true);
                GamePlayScreen.SetActive(false);
            }
            else if (state == UIMainState.GamePlay)
            {
                MainScreen.SetActive(false);
                GamePlayScreen.SetActive(true);
            }
        }
    }
}