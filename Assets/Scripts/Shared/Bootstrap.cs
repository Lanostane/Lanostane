using Loadings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Shared
{
    internal enum BootstrapType
    {
        None,
        PlayerScenario,
        CreatorScenario,
        Automatic
    }

    internal sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField]
        private BootstrapType _Boot;

        void Start()
        {
            if (_Boot == BootstrapType.Automatic)
            {
                _Boot = Utils.Unity.Platform.IsWindows ?
                    BootstrapType.CreatorScenario:
                    BootstrapType.PlayerScenario;
            }

            if (_Boot == BootstrapType.PlayerScenario)
            {
                LoadingWorker.Instance.AddSceneLoadJob(SceneName.PlayerUI);
                LoadingWorker.Instance.StartLoading(LoadingStyle.Default);
            }
            else if (_Boot == BootstrapType.CreatorScenario)
            {
                LoadingWorker.Instance.AddSceneLoadJob(SceneName.CreatorUI);
                LoadingWorker.Instance.StartLoading(LoadingStyle.Default);
            }
        }
    }
}
