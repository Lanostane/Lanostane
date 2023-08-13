﻿using UnityEngine;

namespace LST.Player.Graphics
{
    public interface ILoopParticleFX
    {
        void SetAutoRecycle(bool autoRecycle);
        void SetOwner(LoopParticlePool pool);
        void SetTracking(Transform tracking);
        void SetParent(Transform parent);
        void SetPosition(Vector3 position);
        void StartEmit();
        void StopEmit(bool clear = false);
        void Recycle();
    }
}