using Cysharp.Threading.Tasks;
using Loadings;
using NaughtyAttributes;
//using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;
using Utils.Unity;

namespace LST.GamePlay
{
    public interface IGamePlayLoader
    {
        event Action OnLoaded;
        event Action OnUnloaded;
        public TextAsset ChartToLoad { get; set; }
        public AudioClip MusicToPlay { get; set; }
    }

    public class GamePlayLoader : MonoBehaviour, IGamePlayLoader
    {
        public event Action OnLoaded;
        public event Action OnUnloaded;

        [field: SerializeField]
        public TextAsset ChartToLoad { get; set; }

        [field: SerializeField]
        public AudioClip MusicToPlay { get; set; }

        void Awake()
        {
            GamePlays.GamePlayLoader = this;
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
                OnLoaded?.Invoke();
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
                OnUnloaded?.Invoke();
            });
        }

        void OnDestroy()
        {
            GamePlays.GamePlayLoader = null;
        }
    }
}
