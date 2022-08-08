using UnityEngine;

namespace GamePlay
{
    public sealed class JudgeSFX : MonoBehaviour
    {
        public static JudgeSFX Instance { get; private set; }

        public AudioSource PerfectTapAudio;
        public AudioSource PerfectFlickAudio;
        public AudioSource GoodTapAudio;
        public AudioSource GoodFlickAudio;

        void Awake()
        {
            Instance = this;
        }

        void OnDestroy()
        {
            Instance = null;
        }

        public static void PlayPerfectTap()
        {
            Instance.PerfectTapAudio.Play();
        }

        public static void PlayGoodTap()
        {
            Instance.GoodTapAudio.Play();
        }

        public static void PlayPerfectFlick()
        {
            Instance.PerfectFlickAudio.Play();
        }

        public static void PlayGoodFlick()
        {
            Instance.GoodFlickAudio.Play();
        }
    }
}
