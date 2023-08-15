using UnityEngine;

namespace LST.GamePlay.Graphics
{
    public interface ISingleParticleFX
    {
        void SetAutoRecycle(bool autoRecycle);
        void SetOwner(SingleParticlePool pool);
        void SetParent(Transform parent);
        void SetPosition(Vector3 position);
        void Emit();
        void Recycle();
    }
}
