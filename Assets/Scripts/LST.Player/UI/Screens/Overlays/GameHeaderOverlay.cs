using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LST.Player.UI
{
    public class GameHeaderOverlay : BaseOverlay
    {
        private readonly static Vector3 s_HiddenScale = new(1.2f, 1.2f, 1.2f);
        private readonly static Vector3 s_VisibleScale = new(1.0f, 1.0f, 1.0f);

        public override OverlayType OverlayType => OverlayType.GameHeader;
        protected override bool AutoActiveObject => true;
        protected override bool AutoDeactiveObject => false;

        protected override void OnScreenDisabled()
        {
            transform.localScale = s_VisibleScale;
            transform.DOScale(1.2f, 0.75f);
        }

        protected override void OnScreenEnabled()
        {
            transform.localScale = s_HiddenScale;
            transform.DOScale(1.0f, 0.75f);
        }
    }
}
