using LST.Player.UI;
using NaughtyAttributes;
//using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LST.Player
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
            GamePlay.GamePlayLoader = this;
        }

        [Button("Load GamePlay", EnableWhen.Playmode)]
        public void LoadGamePlay()
        {
            PlayerSettings.LoadFromDisk();

            LoadingWorker.Instance.AddSceneLoadJob(Lanostane.SceneName.GamePlay);
            LoadingWorker.Instance.DoLoading(new LoadingStyle()
            {
                HideScreenOnFinished = true,
                Style = LoadingStyles.BlackShutter
            }, () =>
            {
                GamePlay.ChartPlayer.LoadChart(MusicToPlay, ChartToLoad.text);
                GamePlay.ChartPlayer.StartChart();
                GamePlay.ScrollUpdater.ScrollingSpeed = PlayerSettings.Setting.ScrollSpeed;
                OnLoaded?.Invoke();
            });
        }

        [Button("Unload GamePlay", EnableWhen.Playmode)]
        public void UnloadGamePlay()
        {
            LoadingWorker.Instance.AddSceneUnloadJob(Lanostane.SceneName.GamePlay);
            LoadingWorker.Instance.AddJob(new()
            {
                Job = Resources.UnloadUnusedAssets,
                JobDescription = "Unloading Unused Assets..."
            });
            LoadingWorker.Instance.DoLoading(new LoadingStyle()
            {
                HideScreenOnFinished = true,
                Style = LoadingStyles.BlackShutter
            }, ()=>
            {
                OnUnloaded?.Invoke();
            });
        }

        void OnDestroy()
        {
            GamePlay.GamePlayLoader = null;
        }
    }
}
