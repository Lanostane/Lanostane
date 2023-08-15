using System;

namespace LST.GamePlay
{
    public static class BPM
    {
        public static event Action<float> BPMChanged;

        internal static void Invoke_BPMChange(float bpm) => BPMChanged?.Invoke(bpm);
    }
}
