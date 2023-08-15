using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Maths;

namespace LST.GamePlay.Modifiers
{
    public enum GameModes
    {
        HealthLimited,          //Health Mode
        GhostNotes,             //Hide Note that close enough to Judgeline
    }

    public interface IGameModifierManager
    {
        public ShadowMode ShadowMode { get; }
        GameSpaceEaseMode NoteEase { get; set; }
        void SetModEnabled(GameModes mod, bool enabled);
    }

    public sealed class GameModifierManager : MonoBehaviour, IGameModifierManager
    {
        public ShadowMode ShadowMode { get; private set; }

        [field: SerializeField]
        public GameSpaceEaseMode NoteEase { get; set; } = GameSpaceEaseMode.Default;

        private readonly Dictionary<GameModes, Modifier> _Modifiers = new();

        void Awake()
        {
            GamePlays.Modifier = this;

            ShadowMode = new(enabled: false);
            _Modifiers[GameModes.GhostNotes] = ShadowMode;
        }

        void OnDestroy()
        {
            ShadowMode.SetEnabled(false);
            GamePlays.Modifier = null;
        }

        public void SetModEnabled(GameModes mod, bool enabled)
        {
            if (_Modifiers.TryGetValue(mod, out var modifier))
            {
                modifier.SetEnabled(enabled);
            }
            else
            {
                Debug.LogError($"{mod} is missing!");
            }
        }

        [Button(EnableWhen.Playmode)]
        public void ToggleShadowMode()
        {
            ShadowMode.SetEnabled(!ShadowMode.IsEnabled);
        }
    }
}
