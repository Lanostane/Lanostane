using GamePlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;
using UnityEngine;

namespace Providers
{
    [CreateAssetMenu(fileName = "GP_Provider", menuName = "Providers/GamePlay")]
    public class GamePlayProvider : ScriptableObject
    {
        public static void PauseChart()
        {
            GamePlayManager.Player?.Pause();
            UIManager.Overlays?.GamePause?.SetActive(true);
        }

        public static void ResumeChart()
        {
            GamePlayManager.Player?.Resume();
            UIManager.Overlays?.GamePause?.SetActive(false);
        }
    }
}
