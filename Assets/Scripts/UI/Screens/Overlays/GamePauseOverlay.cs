using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UI.Screens.Overlays
{
    public class GamePauseOverlay : BaseOverlay
    {
        public override OverlayType OverlayType => OverlayType.GamePause;
        protected override bool AutoActiveObject => true;
        protected override bool AutoDeactiveObject => true;

        protected override void OnScreenDisabled()
        {
            
        }

        protected override void OnScreenEnabled()
        {
            
        }
    }
}
