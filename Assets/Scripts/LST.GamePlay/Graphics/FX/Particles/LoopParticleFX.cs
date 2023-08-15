using UnityEngine;

namespace LST.GamePlay.Graphics
{
    public class LoopParticleFX : MonoBehaviour, ILoopParticleFX
    {
        private LoopParticlePool _Pool;

        [SerializeField]
        private ParticleSystem _ParticleSystem;
        public ParticleSystem Particle => _ParticleSystem;

        private Transform _Tracking = null;

        private bool _AutoRecycle = true;

        void Update()
        {
            if (_Tracking != null)
            {
                transform.SetPositionAndRotation(_Tracking.position, _Tracking.rotation);
            }

            if (_AutoRecycle && !_ParticleSystem.isPlaying)
            {
                Recycle();
            }
        }

        public void SetOwner(LoopParticlePool pool)
        {
            _Pool = pool;
        }

        public void Recycle()
        {
            gameObject.SetActive(false);
            _Tracking = null;
            _Pool.Internal__Recycle(this);
        }

        public void SetAutoRecycle(bool autoRecycle)
        {
            _AutoRecycle = autoRecycle;
        }

        public void StartEmit()
        {
            gameObject.SetActive(true);
            _ParticleSystem.Play();
        }

        public void StopEmit(bool clear = false)
        {
            if (clear)
            {
                _ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            else
            {
                _ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetTracking(Transform tracking)
        {
            _Tracking = tracking;
        }
    }
}
