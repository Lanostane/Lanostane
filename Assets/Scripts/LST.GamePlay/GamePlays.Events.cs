using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LST.GamePlay
{
    public static partial class GamePlays
    {
        public static event Action<float> ChartProgressUpdated;
        public static event Action<float> ChartTimeUpdated;
        public static event Action ChartPlayFinished;

        internal static void Invoke_ChartProgressUpdated(float progress)
        {
            ChartProgressUpdated?.Invoke(progress);
        }

        internal static void Invoke_ChartTimeUpdated(float time)
        {
            ChartTimeUpdated?.Invoke(time);
        }

        internal static void Invoke_ChartPlayFinished()
        {
            ChartPlayFinished?.Invoke();
        }
    }
}
