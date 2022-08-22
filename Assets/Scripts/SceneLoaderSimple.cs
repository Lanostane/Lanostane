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

            UIManager.Overlays.Loading.DoLoading(
                () => {
                    return SceneManager.LoadSceneAsync("GamePlay", LoadSceneMode.Additive);
                },
                () =>
                {
                    UIManager.Instance.ChangeMainState(UIMainState.GamePlay);
                    UIManager.Overlays.GameHeader.SetActive(true);
                });
        }
    }
}