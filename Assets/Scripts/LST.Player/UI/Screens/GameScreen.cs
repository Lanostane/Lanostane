using LST.GamePlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LST.Player.UI
{
    public sealed class GameScreen : BaseScreen
    {
        public Button StartButton;

        public override ScreenType Type => ScreenType.Game;
        protected override bool AutoActiveObject => true;
        protected override bool AutoDeactiveObject => true;

        private void Awake()
        {
            StartButton.onClick.AddListener(() =>
            {
                GamePlayLoader.Instance.LoadGamePlay();
                StartButton.gameObject.SetActive(false);
            });
        }

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
