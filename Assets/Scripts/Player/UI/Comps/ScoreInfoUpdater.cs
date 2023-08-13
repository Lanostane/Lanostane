using LST.Player.Scoring;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Utils.Maths;

namespace LST.Player.UI
{
    public class ScoreInfoUpdater : MonoBehaviour
    {
        public TextMeshProUGUI ScoreText;
        public Color PerfectColor;
        public Color FullComboColor;
        public Color RegularColor;

        private int _OldScore = 0;
        private char[] _ScoreTextBuffer = new char[8];

        void Awake()
        {
            ScoreManager.ScoreUpdated += ScoreUpdated;
        }

        void OnDestroy()
        {
            ScoreManager.ScoreUpdated -= ScoreUpdated;
            _ScoreTextBuffer = null;
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
            StartCoroutine(UpdateScore(0.35f, _OldScore, ScoreManager.ScoreRounded));

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
                _OldScore = from + (int)(delta * Ease.Exponential.Out(p));
                SetScoreText(_OldScore);
                p += Time.fixedDeltaTime * deltaFactor;
                yield return new WaitForFixedUpdate();
            }

            SetScoreText(to);
        }

        private unsafe void SetScoreText(int score)
        {
            _ScoreTextBuffer[0] = GetDigitChar(score, 8);
            _ScoreTextBuffer[1] = GetDigitChar(score, 7);
            _ScoreTextBuffer[2] = GetDigitChar(score, 6);
            _ScoreTextBuffer[3] = GetDigitChar(score, 5);
            _ScoreTextBuffer[4] = GetDigitChar(score, 4);
            _ScoreTextBuffer[5] = GetDigitChar(score, 3);
            _ScoreTextBuffer[6] = GetDigitChar(score, 2);
            _ScoreTextBuffer[7] = GetDigitChar(score, 1);
            ScoreText.SetCharArray(_ScoreTextBuffer);
        }

        private static char GetDigitChar(int baseNumber, int digit)
        {
            var n = Math.Pow(10, digit);
            var n2 = Math.Pow(10, digit - 1);
            var i = (int)Math.Floor((baseNumber % n) / n2);
            return i switch
            {
                0 => '0',
                1 => '1',
                2 => '2',
                3 => '3',
                4 => '4',
                5 => '5',
                6 => '6',
                7 => '7',
                8 => '8',
                9 => '9',
                _ => throw new NotImplementedException()
            };
        }
    }
}
