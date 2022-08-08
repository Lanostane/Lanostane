using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace GamePlay.Graphics.FX.Particles
{
    [Serializable]
    public class SingleParticlePool
    {
        [ReadOnly(true)] public GameObject Prefab;
        [ReadOnly(true)] public int Count;

        private readonly Queue<ISingleParticleFX> _Pool = new();

        public void Init(Transform mainTransform)
        {
            for (int i = 0; i < Count; i++)
            {
                var newParticle = UnityEngine.Object.Instantiate(Prefab, mainTransform);
                newParticle.SetActive(false);
                if (!newParticle.TryGetComponent<ISingleParticleFX>(out var fx))
                {
                    UnityEngine.Object.Destroy(newParticle); //???
                }

                fx.SetOwner(this);
                _Pool.Enqueue(fx);
            }
        }

        public void Trigger(Transform parent, Vector3 position)
        {
            if (_Pool.TryDequeue(out var fx))
            {
                fx.SetOwner(this);
                fx.SetParent(parent);
                fx.SetPosition(position);
                fx.Emit();
            }
        }

        internal void Internal__Recycle(ISingleParticleFX fx)
        {
            _Pool.Enqueue(fx);
        }
    }
}
