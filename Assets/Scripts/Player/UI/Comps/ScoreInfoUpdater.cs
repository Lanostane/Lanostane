using LST.Player.Scoring;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Utils.Maths;
using Utils.Unity;

namespace LST.Player.UI
{
    public class ScoreInfoUpdater : MonoBehaviour
    {
        public TextMeshProUGUI ScoreText;
        public Color PerfectColor;
        public Color FullComboColor;
        public Color RegularColor;

        private bool _UpdatingScore = false;
        private int _OldScore = 0;
        private int _ScoreDelta = 0;
        private int _NewScore = 0;
        private int _DisplayScore = 0;
        private float _Timer = 0.0f;
        private float _EndTime = 0.35f;


        void Awake()
        {
            ScoreManager.ScoreUpdated += ScoreUpdated;
        }

        void OnDestroy()
        {
            ScoreManager.ScoreUpdated -= ScoreUpdated;
        }

        void Start()
        {
            ScoreUpdated();
        }

        void ScoreUpdated()
        {
            if (ScoreManager.IsAllPerfect)
            {
                ScoreText.color = PerfectColor;
            }
            else if (ScoreManager.IsAllCombo)
            {
                ScoreText.color = FullComboColor;
            }
            else
            {
                ScoreText.color = RegularColor;
            }

            UpdateScore(0.35f, _DisplayScore, ScoreManager.ScoreRounded);
        }

        private void UpdateScore(float duration, int oldScore, int newScore)
        {
            _UpdatingScore = true;
            _Timer = 0.0f;
            _EndTime = duration;
            _OldScore = oldScore;
            _NewScore = newScore;
            _ScoreDelta = _NewScore - _OldScore;
        }

        private void FixedUpdate()
        {
            if (!_UpdatingScore)
                return;

            if (_Timer >= _EndTime)
            {
                SetScoreText(_NewScore);
                _OldScore = _NewScore;
                _UpdatingScore = false;
                return;
            }

            var p = Mathf.Clamp01(_Timer / _EndTime);
            _DisplayScore = _OldScore + (int)(_ScoreDelta * Ease.Exponential.Out(p));
            SetScoreText(_DisplayScore);
            _Timer += Time.fixedDeltaTime;
        }

        private unsafe void SetScoreText(int score)
        {
            ScoreStrings.SetScoreBuffer(score);
            ScoreText.SetCharArray(ScoreStrings.ScoreBuffer);
        }
    }
}
