using Lanostane.Charts.DTO;
using GamePlay.Graphics;
using GamePlay.Judge;
using GamePlay.Motions;
using GamePlay.Scrolls;
using Newtonsoft.Json;
using Lanostane.Settings;
using System;
using UnityEngine;
using Utils.Maths;

namespace GamePlay
{
    public interface IChartPlayer
    {
        void LoadChart(AudioClip music, string json);
    }

    public sealed class ChartPlayer : MonoBehaviour, IChartPlayer
    {
        public static bool ChartLoaded { get; private set; }
        public static float MusicTime { get; private set; }
        public static float ChartTime { get; private set; }
        public static float OffsetChartTime { get; private set; }

        public static event Action<float> ChartProgressUpdated;
        public static event Action<float> ChartTimeUpdated;
        public static event Action ChartPlayFinished;

        public TextAsset Chart;
        public AudioClip Music;

        public AudioSource Audio;
        public float PlaySpeed { get; private set; }
        public bool StartChartOnLoaded;

        private readonly ChartUpdater _Updater = new();
        private bool _ChartPlaying = false;
        private float _ChartOffset = 0.0f;
        private float _ChartPlaytime = 0.0f;

        void Start()
        {
            _ChartOffset = -UserSetting.Offset / 1000.0f;
            PlaySpeed = PlayerSetting.Settings.PlaySpeed;
            LoadChart(Music, Chart.text);
        }

        void OnDestroy()
        {
            ResetValues();
        }

        void OnValidate()
        {
            Audio.pitch = PlaySpeed;

        }

        public void LoadChart(AudioClip music, string json)
        {
            ResetValues();

            Audio.clip = music;
            MusicTime = Audio.clip.length;

            var laChart = JsonConvert.DeserializeObject<LaChart>(json);
            var chart = laChart.CreateLanostaneChart();
            _Updater.Setup(chart);
            _ChartPlaytime = chart.SongLength;

            ChartLoaded = true;
            if (StartChartOnLoaded)
            {
                MotionManager.Instance.StartDefaultMotion(1.75f);
                Invoke(nameof(StartChart), 1.75f);
            }
        }

        public void Invoke_TimeUpdate(float time)
        {
            ChartTimeUpdated?.Invoke(time);
            ChartProgressUpdated?.Invoke(time / MusicTime);
        }

        private void ResetValues()
        {
            ChartLoaded = false;
            MusicTime = 0.0f;
            ChartTime = 0.0f;
            OffsetChartTime = 0.0f;
        }

        private void CleanupCharts()
        {
            _Updater.CleanUp();
        }

        public void Pause()
        {
            if (ChartLoaded && _ChartPlaying)
            {
                Audio.Pause();
            }
        }

        public void Resume()
        {
            if (ChartLoaded && !_ChartPlaying)
            {
                Audio.UnPause();
            }
        }

        void StartChart()
        {
            Audio.Play();
            Audio.pitch = PlaySpeed;
            ChartTime = 0.0f;
            OffsetChartTime = 0.0f;
            _ChartPlaying = true;
        }

        void Update()
        {
            if (!_ChartPlaying)
                return;

            var playing = ChartTime <= _ChartPlaytime || Audio.isPlaying;
            if (!playing)
            {
                _ChartPlaying = false;
                ChartPlayFinished?.Invoke();
            }
            else
            {
                ChartTime += (Time.deltaTime * PlaySpeed);

                var audioTime = Audio.time;
                if (!MathfE.AbsApprox(ChartTime, audioTime, 0.05f))
                {
                    ChartTime = audioTime;
                }

                OffsetChartTime = ChartTime + _ChartOffset;
                _Updater.UpdateChart(OffsetChartTime);
            }
        }
    }
}
