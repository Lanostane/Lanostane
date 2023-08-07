using System.Collections;
using UnityEngine;

namespace LST.Player.UI
{
    public interface IUIManager
    {
        
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
        }

        void OnDestroy()
        {
            Instance = null;
            Screens = null;
            Overlays = null;
        }
    }
}