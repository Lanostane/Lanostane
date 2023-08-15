using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LST.GamePlay.Modifiers
{
    public abstract class Modifier
    {
        public bool IsEnabled { get; private set; }
        public abstract GameModes Mode { get; }

        public Modifier()
        {
            IsEnabled = false;
            OnDisable();
        }

        public void SetEnabled(bool enabled)
        {
            if (enabled && !IsEnabled)
            {
                IsEnabled = true;
                OnEnable();
            }
            else if (!enabled && IsEnabled)
            {
                IsEnabled = false;
                OnDisable();
            }
        }

        protected abstract void OnDisable();
        protected abstract void OnEnable();
    }
}
