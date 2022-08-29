using GamePlay.Scoring;
using System.Collections;
using TMPro;
using UnityEngine;
using Utils.Maths;

namespace Lst.UI.Comps
{
    public class ScoreInfoUpdater : MonoBehaviour
    {
        public TextMeshProUGUI ScoreText;
        public Color PerfectColor;
        public Color FullComboColor;
        public Color RegularColor;

        private int _OldScore = 0;

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

            StopAllCoroutines();
            StartCoroutine(UpdateScore(0.45f, _OldScore, ScoreManager.ScoreRounded));

            _OldScore = ScoreManager.ScoreRounded;
        }

        IEnumerator UpdateScore(float duration, int from, int to)
        {
            if (from == to)
            {
                SetScoreText(to);
                yield break;
            }

            var p = 0.0f;
            var deltaFactor = 1.0f / duration;
            var delta = to - from;

            while (p <= 1.0f)
            {
                _OldScore = to + (int)(delta * Ease.Exponential.Out(p));
                SetScoreText(_OldScore);
                p += Time.fixedDeltaTime * deltaFactor;
                yield return new WaitForFixedUpdate();
            }

            SetScoreText(to);
        }

        private void SetScoreText(int score)
        {
            ScoreText.text = score.ToString("D8");
        }
    }
}
