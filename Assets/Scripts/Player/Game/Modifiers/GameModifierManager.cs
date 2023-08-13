using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LST.Player.Modifiers
{
    public enum GameModes
    {
        HealthLimited,          //Health Mode
        GhostNotes,             //Hide Note that close enough to Judgeline
    }

    public enum NoteEaseMode : byte
    {
        Default,
        Linear,
        Deceleration
    }

    public sealed class GameModifierManager : MonoBehaviour
    {
        public static GameModifierManager Instance { get; private set; }



        public NoteEaseMode NoteEase { get; private set; }

        void Awake()
        {
            Shader.SetGlobalFloat("_ShadowMode_Enabled", 0.0f);
            Shader.SetGlobalFloat("_ShadowMode_StartDist", GameConst.LerpSpaceFactor(0.1f));
            Shader.SetGlobalFloat("_ShadowMode_EndDist", GameConst.LerpSpaceFactor(0.485f));
        }

        private void OnDestroy()
        {
            Shader.SetGlobalFloat("_ShadowMode_Enabled", 0.0f);
            Shader.SetGlobalFloat("_ShadowMode_StartDist", GameConst.LerpSpaceFactor(0.1f));
            Shader.SetGlobalFloat("_ShadowMode_EndDist", GameConst.LerpSpaceFactor(0.485f));
        }
    }
}
