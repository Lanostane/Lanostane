using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Screens;
using UI.Screens.Overlays;
using UnityEngine;

namespace UI
{
    public interface IOverlayHolder
    {
        IOverlay GameHeader { get; }
        IOverlay GameResult { get; }
        IOverlay GamePause { get; }
    }

    public class OverlayHolder : MonoBehaviour, IOverlayHolder
    {
        public IOverlay GameHeader { get; private set; }
        public IOverlay GameResult { get; private set; }
        public IOverlay GamePause { get; private set; }

        private readonly Dictionary<OverlayType, IOverlay> _Overlays = new();

        public void Setup(BaseScreen[] screens)
        {
            foreach (var screen in screens)
            {
                if (screen is not BaseOverlay overlay)
                {
                    continue;
                }

                var type = overlay.OverlayType;
                if (_Overlays.TryGetValue(type, out var _))
                {
                    Debug.LogError($"Duplicated Overlay has added with type: {type}");
                    continue;
                }

                _Overlays[type] = overlay;
                overlay.Setup();

                switch (type)
                {
                    case OverlayType.GameHeader:
                        GameHeader = overlay;
                        break;

                    case OverlayType.GameResult:
                        GameResult = overlay;
                        break;

                    case OverlayType.GamePause:
                        GamePause = overlay;
                        break;
                }
            }
        }

        public IOverlay Get(OverlayType type)
        {
            return _Overlays[type];
        }
    }
}
