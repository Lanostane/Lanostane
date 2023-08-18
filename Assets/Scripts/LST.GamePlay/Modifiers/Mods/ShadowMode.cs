using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LST.GamePlay.Modifiers
{
    internal sealed class ShadowMode : Modifier
    {
        public override GameModes Mode => GameModes.GhostNotes;

        public ShadowMode(bool enabled) : base()
        {
            ChangeShadowDistance(0.1f, 0.485f);
            SetEnabled(enabled);
        }

        public void ChangeShadowDistance(float startRatio, float endRatio)
        {
            Shader.SetGlobalFloat("_ShadowMode_StartDist", GameConst.LerpSpaceFactor(startRatio));
            Shader.SetGlobalFloat("_ShadowMode_EndDist", GameConst.LerpSpaceFactor(endRatio));
        }

        protected override void OnDisable()
        {
            Shader.SetGlobalFloat("_ShadowMode_Enabled", 0.0f);
        }

        protected override void OnEnable()
        {
            Shader.SetGlobalFloat("_ShadowMode_Enabled", 1.0f);
        }
    }
}
