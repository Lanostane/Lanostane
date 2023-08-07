using LST.Player.Judge;
using LST.Player.Motions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LST.Player
{
    public static class GamePlayManager
    {
        public static IChartPlayer Player { get; internal set; }

        public static IBGUpdater BGUpdater { get; internal set; }
        public static INoteGraphicUpdater GraphicUpdater { get; internal set; }
        public static IScrollUpdater ScrollUpdater { get; internal set; }
        public static IMotionUpdater MotionUpdater { get; internal set; }
        public static INoteJudgeUpdater NoteJudgeUpdater { get; internal set; }
    }
}
