using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lst.UI.Screens;
using Lst.UI.Screens.Overlays;
using UnityEngine;

namespace Lst.UI
{
    public interface IScreenHolder
    {
        IScreen Intro { get; }
        IScreen MainMenu { get; }
        IScreen Game { get; }
    }

    public sealed class ScreenHolder : MonoBehaviour, IScreenHolder
    {
        public IScreen Intro { get; private set; }
        public IScreen MainMenu { get; private set; }
        public IScreen Game { get; private set; }

        private readonly Dictionary<ScreenType, IScreen> _Screens = new();

        public void Setup(BaseScreen[] screens)
        {
            foreach (var screen in screens)
            {
                if (screen is BaseOverlay)
                {
                    continue;
                }

                var type = screen.Type;
                if (_Screens.TryGetValue(type, out var _))
                {
                    Debug.LogError($"Duplicated Overlay has added with type: {type}");
                    continue;
                }

                _Screens[type] = screen;
                screen.Setup();

                switch (type)
                {
                    case ScreenType.Intro:
                        Intro = screen;
                        break;

                    case ScreenType.MainMenu:
                        MainMenu = screen;
                        break;

                    case ScreenType.Game:
                        Game = screen;
                        break;
                }
            }
        }
    }
}
