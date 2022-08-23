using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UI.Overlays
{
    public class GameResultOverlay : BaseOverlay
    {
        private readonly static Vector3 _HiddenScale = new(1.2f, 1.2f, 1.2f);
        private readonly static Vector3 _VisibleScale = new(1.0f, 1.0f, 1.0f);
        protected override bool AutoActiveObject => true;
        protected override bool AutoDeactiveObject => true;

        public override void Setup()
        {
            base.Setup();
            Debug.Log("It does Setup");
        }

        protected override void OnOverlayDisabled()
        {
            //transform.DOScale(1.2f, 0.75f);
        }

        protected override void OnOverlayEnabled()
        {
            //transform.DOScale(1.0f, 0.75f);
        }
    }
}
