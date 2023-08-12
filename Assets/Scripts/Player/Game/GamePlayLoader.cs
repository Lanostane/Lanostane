using Lanostane.Settings;
using LST.Player.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LST.Player
{
    public interface IGamePlayLoader
    {
        public TextAsset ChartToLoad { get; set; }
        public AudioClip MusicToPlay { get; set; }
    }

    public class GamePlayLoader : MonoBehaviour, IGamePlayLoader
    {
        [field: SerializeField]
        public TextAsset ChartToLoad { get; set; }

        [field: SerializeField]
        public AudioClip MusicToPlay { get; set; }

        [field: SerializeField]
        public float ScrollSpeedOverride { get; set; } = -1.0f;

        void Awake()
        {
            GamePlayManager.GamePlayLoader = this;
        }

        void Start()
        {
            UserSetting.Load();

            LoadingWorker.Instance.AddSceneLoadJob(Lanostane.SceneName.GamePlay);
            LoadingWorker.Instance.DoLoading(new LoadingStyle()
            {
                HideScreenOnFinished = true,
                Style = LoadingStyles.BlackShutter
            }, ()=>
            {
                GamePlayManager.ChartPlayer.LoadChart(MusicToPlay, ChartToLoad.text);
                GamePlayManager.ChartPlayer.StartChart();

                if (ScrollSpeedOverride > 0.0f)
                {
                    GamePlayManager.ScrollUpdater.ScrollingSpeed = ScrollSpeedOverride;
                }
            });
        }

        void OnDestroy()
        {
            GamePlayManager.GamePlayLoader = null;
        }
    }
}
