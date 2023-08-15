using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
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

        [Range(-1, 120)]
        public int FrameRate = -1;
    }

    [Serializable]
    public class DebugSettingsData
    {
        [Range(0.0001f, 3.0f)]
        public float MusicPlaySpeed = 1.0f;

        public bool AudoPlayEnabled = false;
    }

    public sealed class PlayerSettings : MonoBehaviour
    {
        public static PlayerSettingsData Setting;
        public static DebugSettingsData DebugSetting;

        [Label("User Settings")]
        public PlayerSettingsData UserData;

        [Label("Development Settings")]
        public DebugSettingsData DebugData;

        private AudioMixer _Mixer;

        [Button(null, EnableWhen.Playmode)]
        public static void LoadFromDisk()
        {

        }

        [Button(null, EnableWhen.Playmode)]
        public static void SaveToDisk()
        {

        }

        void Start()
        {
            Setting = UserData;
            DebugSetting = DebugData;
            _Mixer = Resources.Load<AudioMixer>("AudioMixer");
        }

        void FixedUpdate()
        {
            _Mixer.SetFloat("VolMaster", GetVolume(Setting.MasterVolume));
            _Mixer.SetFloat("VolMusic", GetVolume(Setting.MusicVolume));
            _Mixer.SetFloat("VolJudgeSFX", GetVolume(Setting.SFXVolume));

            Application.targetFrameRate = Setting.FrameRate;
        }

        static float GetVolume(float vol01)
        {
            var clamped = Mathf.Clamp(vol01, float.Epsilon, 1.0f);
            return Mathf.Log10(clamped) * 20.0f;
        }
    }
}
