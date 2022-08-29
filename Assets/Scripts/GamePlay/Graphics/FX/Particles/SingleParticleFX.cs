using UnityEngine;

namespace GamePlay.Graphics.FX.Particles
{
    public class SingleParticleFX : MonoBehaviour, ISingleParticleFX
    {
        private SingleParticlePool _Pool;

        [SerializeField]
        private ParticleSystem _ParticleSystem;
        public ParticleSystem Particle => _ParticleSystem;

        private bool _AutoRecycle = true;

        void FixedUpdate()
        {
            if (_AutoRecycle && !_ParticleSystem.isPlaying)
            {
                Recycle();
            }
        }

        public void SetOwner(SingleParticlePool pool)
        {
            _Pool = pool;
        }

        public void Recycle()
        {
            gameObject.SetActive(false);
            _Pool.Internal__Recycle(this);
        }

        public void Emit()
        {
            gameObject.SetActive(true);

            _ParticleSystem.Play();
        }

        public void SetAutoRecycle(bool autoRecycle)
        {
            _AutoRecycle = autoRecycle;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
