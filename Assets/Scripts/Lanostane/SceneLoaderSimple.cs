using Lst.Loading;
using Lst.Settings;
using System.Collections;
using Lst.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lst
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