﻿using Loadings;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LST.GamePlay
{
    internal sealed class GamePlayLoader_Impl : MonoBehaviour, IGamePlayLoader
    {
        [field: SerializeField]
        public TextAsset ChartToLoad { get; set; }

        [field: SerializeField]
        public AudioClip MusicToPlay { get; set; }

        void Awake()
        {
            GamePlayLoader.Instance = this;
        }

        [Button("Load GamePlay", EnableWhen.Playmode)]
        public void LoadGamePlay()
        {
            LoadingWorker.Instance.AddSceneLoadJob(SceneName.GamePlay);
            LoadingWorker.Instance.AddJob(new LoadGamePlayJob(MusicToPlay, ChartToLoad));
            LoadingWorker.Instance.StartLoading(new LoadingStyle()
            {
                HideScreenOnFinished = true,
                Style = LoadingStyles.BlackShutter
            }, () =>
            {
                GamePlayLoader.Invoke_OnLoaded();
            });
        }

        [Button("Unload GamePlay", EnableWhen.Playmode)]
        public void UnloadGamePlay()
        {
            LoadingWorker.Instance.AddSceneUnloadJob(SceneName.GamePlay, unloadUnusedAssets: true);
            LoadingWorker.Instance.StartLoading(new LoadingStyle()
            {
                HideScreenOnFinished = true,
                Style = LoadingStyles.BlackShutter
            }, () =>
            {
                GamePlayLoader.Invoke_OnUnloaded();
            });
        }

        void OnDestroy()
        {
            GamePlayLoader.Instance = null;
        }
    }
}