using DG.Tweening;
using GamePlay.Scoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Lanostane.UI.Screens.Overlays
{
    public class GameResultOverlay : BaseOverlay
    {
        private readonly static Vector3 _HiddenScale = new(1.2f, 1.2f, 1.2f);
        private readonly static Vector3 _VisibleScale = new(1.0f, 1.0f, 1.0f);
        public override OverlayType OverlayType => OverlayType.GameResult;
        protected override bool AutoActiveObject => true;
        protected override bool AutoDeactiveObject => true;

        public TextMeshProUGUI ScoreText;
        public TextMeshProUGUI PerfectText;
        public TextMeshProUGUI GoodText;
        public TextMeshProUGUI MissText;

        protected override void OnScreenEnabled()
        {
            //transform.DOScale(1.0f, 0.75f);
            ScoreText.text = $"Score: {ScoreManager.ScoreString}";
            PerfectText.text = $"Perfect: {ScoreManager.PerfectCount}/{ScoreManager.TotalNotes} (+{ScoreManager.PerfectPlusCount})";
            GoodText.text = $"Good: {ScoreManager.GoodCount}/{ScoreManager.TotalNotes}";
            MissText.text = $"Miss: {ScoreManager.MissCount}/{ScoreManager.TotalNotes}";
        }

        protected override void OnScreenDisabled()
        {
            //transform.DOScale(1.2f, 0.75f);
        }
    }
}
