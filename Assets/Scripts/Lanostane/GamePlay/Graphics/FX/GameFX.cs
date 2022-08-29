using Lst.GamePlay.Graphics.FX.Particles;
using UnityEngine;

namespace Lst.GamePlay.Graphics.FX
{
    public class GameFX : MonoBehaviour
    {
        public static GameFX Instance { get; private set; }
        public static SingleParticlePool TapParticles => Instance._TapParticles;
        public static LoopParticlePool HoldParticles => Instance._HoldParticles;

        [SerializeField]
        private SingleParticlePool _TapParticles;
        [SerializeField]
        private LoopParticlePool _HoldParticles;

        void Start()
        {
            _TapParticles.Init(transform);
            _HoldParticles.Init(transform);
            Instance = this;
        }

        void OnDestroy()
        {
            Instance = null;
        }
    }
}
