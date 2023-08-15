using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace LST.GamePlay.Graphics
{
    [Serializable]
    public class LoopParticlePool
    {
        [ReadOnly(true)] public GameObject Prefab;
        [ReadOnly(true)] public int Count;

        private readonly Queue<ILoopParticleFX> _Pool = new();

        public void Init(Transform mainTransform)
        {
            for (int i = 0; i < Count; i++)
            {
                var newParticle = UnityEngine.Object.Instantiate(Prefab, mainTransform);
                newParticle.SetActive(false);
                if (!newParticle.TryGetComponent<ILoopParticleFX>(out var fx))
                {
                    UnityEngine.Object.Destroy(newParticle); //???
                }

                fx.SetOwner(this);
                _Pool.Enqueue(fx);
            }
        }

        public bool TryAllocate(out ILoopParticleFX fx)
        {
            if (_Pool.TryDequeue(out fx))
            {
                fx.SetOwner(this);
                return true;
            }

            return false;
        }

        internal void Internal__Recycle(ILoopParticleFX fx)
        {
            _Pool.Enqueue(fx);
        }
    }
}
