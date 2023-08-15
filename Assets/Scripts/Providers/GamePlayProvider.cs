using LST.GamePlay;
using LST.Player.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Providers
{
    [CreateAssetMenu(fileName = "GP_Provider", menuName = "Providers/GamePlay")]
    public class GamePlayProvider : ScriptableObject
    {
        public static void PauseChart()
        {
            GamePlays.ChartPlayer?.Pause();
            UIManager.Overlays?.GamePause?.SetActive(true);
        }

        public static void ResumeChart()
        {
            GamePlays.ChartPlayer?.Resume();
            UIManager.Overlays?.GamePause?.SetActive(false);
        }
    }
}
