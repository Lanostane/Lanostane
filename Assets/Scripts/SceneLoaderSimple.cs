using Settings;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class SceneLoaderSimple : MonoBehaviour
    {
        public void LoadGamePlay()
        {
            UserSetting.Load();

            UIManager.Instance.LoadingScreen.Show(()=>
            {
                var sceneLoading = SceneManager.LoadSceneAsync("GamePlay", LoadSceneMode.Additive);
                UIManager.Instance.LoadingScreen.HideAfter(sceneLoading, () =>
                {
                    UIManager.Instance.ChangeMainState(UIMainState.GamePlay);
                    UIManager.Instance.GameHeaderOverlay.OnOverlayEnabled();
                });
            });
        }
    }
}