using System.Collections;
using UI.Overlays;
using UI.Screens;
using UnityEngine;

namespace UI
{
    public interface IUIManager
    {
        void ChangeMainState(UIMainState state);
        ILoadingScreen LoadingScreen { get; }
        IOverlay GameHeaderOverlay { get; }
    }

    public enum UIMainState
    {
        MainMenu,
        GamePlay
    }

    public class UIManager : MonoBehaviour, IUIManager
    {
        public static IUIManager Instance { get; private set; }

        [SerializeField] private LoadingScreen _LoadingScreen;
        [SerializeField] private GameHeaderOverlay _HeaderOverlay;
        public ILoadingScreen LoadingScreen => _LoadingScreen;
        public IOverlay GameHeaderOverlay => _HeaderOverlay;

        public GameObject MainScreen;
        public GameObject GamePlayScreen;

        private UIMainState _CurrentState = UIMainState.MainMenu;

        void Awake()
        {
            Instance = this;
        }

        void OnDestroy()
        {
            Instance = null;
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