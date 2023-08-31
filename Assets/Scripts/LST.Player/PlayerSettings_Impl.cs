using LST.GamePlay;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace LST.Player
{
    [Serializable]
    public class PlayerSettingsData
    {
        [Range(-1000, 1000)]
        public int Offset = 0;

        [Range(1.0f, 9.0f)]
        public float ScrollSpeed = 7.0f;

        [Range(0.0f, 1.0f)]
        public float MasterVolume = 1.0f;

        [Range(0.0f, 1.0f)]
        public float SFXVolume = 1.0f;

        [Range(0.0f, 1.0f)]
        public float MusicVolume = 1.0f;
    }

    [Serializable]
    public class DebugSettingsData
    {
        [Range(0.0001f, 3.0f)]
        public float MusicPlaySpeed = 1.0f;

        public bool AudoPlayEnabled = false;
    }

    internal sealed class PlayerSettings_Impl : MonoBehaviour
    {
        [Label("User Settings")]
        public PlayerSettingsData UserData;

        [Label("Development Settings")]
        public DebugSettingsData DebugData;

        private AudioMixer _Mixer;

        private void Start()
        {
            _Mixer = Resources.Load<AudioMixer>("AudioMixer");

            GamePlayLoader.OnLoaded += OnGamePlayLoaded;
        }

        private void OnDestroy()
        {
            GamePlayLoader.OnLoaded -= OnGamePlayLoaded;
        }

        private void OnGamePlayLoaded()
        {
            GamePlays.ScrollUpdater.ScrollingSpeed = UserData.ScrollSpeed;
            GamePlays.NoteJudgeUpdater.AutoPlay = DebugData.AudoPlayEnabled;
            GamePlays.ChartPlayer.ChartOffset = UserData.Offset;
            Debug.Log("Settings Applied!");
        }

        private void FixedUpdate()
        {
            _Mixer.SetFloat("VolMaster", GetVolume(UserData.MasterVolume));
            _Mixer.SetFloat("VolMusic", GetVolume(UserData.MusicVolume));
            _Mixer.SetFloat("VolJudgeSFX", GetVolume(UserData.SFXVolume));
        }

        static float GetVolume(float vol01)
        {
            var clamped = Mathf.Clamp(vol01, min: float.Epsilon, max: 1.0f);
            return Mathf.Log10(clamped) * 20.0f;
        }
    }
}
