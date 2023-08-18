using LST.GamePlay.Judge;
using LST.GamePlay.Modifiers;
using LST.GamePlay.Motions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LST.GamePlay
{
    public static partial class GamePlays
    {
        public static bool GamePlayLoaded { get; internal set; } = false;
        

        public static IGameModifierManager Modifier { get; internal set; }
        public static IChartPlayer ChartPlayer { get; internal set; }

        public static Camera MainCam { get; internal set; }
        public static Transform MainCamTransform { get; internal set; }

        public static IBGUpdater BGUpdater { get; internal set; }
        public static INoteGraphicUpdater GraphicUpdater { get; internal set; }
        public static IScrollUpdater ScrollUpdater { get; internal set; }
        public static IMotionUpdater MotionUpdater { get; internal set; }
        public static INoteJudgeUpdater NoteJudgeUpdater { get; internal set; }
    }
}
