using DG.Tweening;
using LST.GamePlay.Scoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace LST.Player.UI
{
    public class GameResultOverlay : BaseOverlay
    {
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
            ScoreText.text = $"Score: {ScoreManager.ScoreRounded:D8}";
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
