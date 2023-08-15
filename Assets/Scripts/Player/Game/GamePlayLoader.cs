using Cysharp.Threading.Tasks;
using LST.Player.UI;
using NaughtyAttributes;
//using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;
using Utils.Unity;

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
            LoadingWorker.Instance.AddJob(new ()
            {
                JobDescription = "Loading Chart...",
                Job = () =>
                {
                    return GamePlay.ChartPlayer.LoadChart(MusicToPlay, ChartToLoad.text, Progress.Create<LoadChartSteps>((step)=>
                    {
                        EditorLog.Info($"Step Change: {step}");
                    }));
                }
            });
            LoadingWorker.Instance.StartLoading(new LoadingStyle()
            {
                HideScreenOnFinished = true,
                Style = LoadingStyles.BlackShutter
            }, () =>
            {
                
                GamePlay.ChartPlayer.StartChart();
                GamePlay.ScrollUpdater.ScrollingSpeed = PlayerSettings.Setting.ScrollSpeed;
                OnLoaded?.Invoke();
            });
        }

        [Button("Unload GamePlay", EnableWhen.Playmode)]
        public void UnloadGamePlay()
        {
            LoadingWorker.Instance.AddSceneUnloadJob(Lanostane.SceneName.GamePlay, unloadUnusedAssets: true);
            LoadingWorker.Instance.StartLoading(new LoadingStyle()
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
