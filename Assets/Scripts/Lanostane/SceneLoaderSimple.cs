using Lanostane.Loading;
using Lanostane.Settings;
using System.Collections;
using Lanostane.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lanostane
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