using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Audio;

namespace LST.Player
{
    [Serializable]
    public struct PlayerSettingsData
    {
        [Range(-1000, 1000)]
        public int Offset;
        [Range(1.0f, 9.0f)]
        public float ScrollSpeed;
        [Range(0.0f, 1.0f)]
        public float MasterVolume;
        [Range(0.0f, 1.0f)]
        public float SFXVolume;
        [Range(0.0f, 1.0f)]
        public float MusicVolume;

        public int FrameRate;
    }

    public sealed class PlayerSettings : MonoBehaviour
    {
        public static PlayerSettingsData Setting;

        public PlayerSettingsData Data;

        [SerializeField]
        private AudioMixer _Mixer;

        void Start()
        {
            _Mixer = Resources.Load<AudioMixer>("AudioMixer");
        }

        void Update()
        {
            Setting = Data;
            _Mixer.SetFloat("VolMaster", GetVolume(Data.MasterVolume));
            _Mixer.SetFloat("VolMusic", GetVolume(Data.MusicVolume));
            _Mixer.SetFloat("VolJudgeSFX", GetVolume(Data.SFXVolume));

            Application.targetFrameRate = Data.FrameRate;
        }

        static float GetVolume(float vol01)
        {
            var clamped = Mathf.Clamp(vol01, float.Epsilon, 1.0f);
            return Mathf.Log10(clamped) * 20.0f;
        }
    }
}
