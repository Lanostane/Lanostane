using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lanostane.UI.Screens.Overlays
{
    public class GameHeaderOverlay : BaseOverlay
    {
        private readonly static Vector3 _HiddenScale = new(1.2f, 1.2f, 1.2f);
        private readonly static Vector3 _VisibleScale = new(1.0f, 1.0f, 1.0f);

        public override OverlayType OverlayType => OverlayType.GameHeader;
        protected override bool AutoActiveObject => true;
        protected override bool AutoDeactiveObject => false;

        protected override void OnScreenDisabled()
        {
            transform.localScale = _VisibleScale;
            transform.DOScale(1.2f, 0.75f);
        }

        protected override void OnScreenEnabled()
        {
            transform.localScale = _HiddenScale;
            transform.DOScale(1.0f, 0.75f);
        }
    }
}
