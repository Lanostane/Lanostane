using LST.Player.Modifiers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LST.Player.Game.Modifiers
{
    public abstract class Modifier
    {
        private bool _Enabled = false;

        public abstract GameModes Mode { get; }

        public Modifier()
        {
            _Enabled = false;
            OnDisable();
        }

        public void SetEnabled(bool enabled)
        {
            if (enabled && !_Enabled)
            {
                _Enabled = true;
                OnEnable();
            }
            else if (!enabled && _Enabled)
            {
                _Enabled = false;
                OnDisable();
            }
        }

        protected abstract void OnDisable();
        protected abstract void OnEnable();
    }
}
