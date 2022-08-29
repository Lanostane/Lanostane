using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Lst.UI.Screens
{
    public sealed class GameScreen : BaseScreen
    {
        public override ScreenType Type => ScreenType.Game;
        protected override bool AutoActiveObject => true;
        protected override bool AutoDeactiveObject => true;

        protected override void OnScreenEnabled()
        {
            UIManager.Overlays.GameHeader.SetActive(true);
        }

        protected override void OnScreenDisabled()
        {
            UIManager.Overlays.GameHeader.SetActive(false);
            UIManager.Overlays.GameResult.SetActive(false);
        }
    }
}
