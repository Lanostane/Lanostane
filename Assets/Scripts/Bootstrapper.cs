using Loading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Bootstrapper : MonoBehaviour
{
    void Start()
    {
        LoadingWorker.Instance.Enqueue(new LoadJob()
        {
            JobDescription = "Loading Main UI",
            Job = () =>
            {
                return SceneManager.LoadSceneAsync("MainUI", LoadSceneMode.Single);
            }
        });
        LoadingWorker.Instance.DoLoading(LoadingStyle.Default);
    }
}
