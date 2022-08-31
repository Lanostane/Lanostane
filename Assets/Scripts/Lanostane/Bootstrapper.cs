using Lanostane.Packages.Readers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lanostane
{
    public sealed class Bootstrapper : MonoBehaviour
    {
        private static bool _Booted = false;

        void Start()
        {
            if (_Booted)
                return;

            _ = LytPackage.ConvertToLSTAsync(Path.Combine(Application.streamingAssetsPath, "test.layesta"));
            _ = LytPackage.ConvertToLSTAsync(Path.Combine(Application.streamingAssetsPath, "test1.layesta"));
            _ = LytPackage.ConvertToLSTAsync(Path.Combine(Application.streamingAssetsPath, "test2.layesta"));
            _ = LytPackage.ConvertToLSTAsync(Path.Combine(Application.streamingAssetsPath, "test3.layesta"));

            /*
            LoadingWorker.Instance.Enqueue(new LoadJob()
            {
                JobDescription = "Loading Main UI",
                Job = () =>
                {
                    return SceneManager.LoadSceneAsync("MainUI", LoadSceneMode.Single);
                }
            });
            LoadingWorker.Instance.DoLoading(LoadingStyle.Default);
            */

            _Booted = true;
        }
    }
}