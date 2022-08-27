using Loading;
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

            LoadingWorker.Instance.Enqueue(new LoadJob()
            {
                JobDescription = "Loading GamePlay Assets...",
                Job = () =>
                {
                    return SceneManager.LoadSceneAsync("GamePlay", LoadSceneMode.Additive);
                }
            });

            LoadingWorker.Instance.DoLoading(LoadingStyle.Default, () =>
            {
                UIManager.Instance.WantToChangeState(UIMainState.GamePlay);
            });
        }
    }
}