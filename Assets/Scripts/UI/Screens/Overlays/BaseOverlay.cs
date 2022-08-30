using UnityEngine;

namespace UI.Screens.Overlays
{
    public enum OverlayType
    {
        GameHeader,
        GameResult
    }

    public interface IOverlay
    {
        OverlayType OverlayType { get; }
        void SetActive(bool wantToActive);
    }

    public abstract class BaseOverlay : BaseScreen, IOverlay
    {
        public sealed override ScreenType Type => ScreenType.Overlay;
        public abstract OverlayType OverlayType { get; }
    }
}
