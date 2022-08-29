using Lst.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Utils.Maths;

namespace Lst.UI.Comps.Calibration
{
    public struct Sample
    {
        public int Ms;
        public int Delta;
    }

    public class CalibrationControl : MonoBehaviour
    {
        public const string START_TEXT = "TAP HERE\n\nTO\n\nSTART";
        public const string TAP_TEXT = "TAP HERE\n\nTO THE\n\nBEAT";
        public const int SAMPLE_INTERVAL = 500;
        public const float SONG_DURATION = 16.5f;

        public TapArea TapArea;
        public AudioSource CueMusic;

        private readonly List<Sample> _Samples = new();
        private bool _Calibrating = false;
        private bool _CalibratingFinished = false;
        private float _Timing = 0.0f;

        void Awake()
        {
            TapArea.PointerDowned += TapArea_Pressed;    
        }

        private void ResetState()
        {
            _Calibrating = false;
            _CalibratingFinished = false;
            _Timing = 0.0f;
            _Samples.Clear();
            TapArea.SetText(START_TEXT);
        }

        private void TapArea_Pressed()
        {
            if (!_Calibrating)
            {
                _Samples.Clear();
                TapArea.SetText(TAP_TEXT);

                CueMusic.Play();
                StartCoroutine(CalibrationTimeUpdater(SONG_DURATION));
                _Calibrating = true;
                _CalibratingFinished = false;
            }
            else if (!_CalibratingFinished)
            {
                var tappedMs = (int)(_Timing * 1000.0f);
                var tappedFactor = tappedMs / (float)SAMPLE_INTERVAL;
                var soundMs = Mathf.RoundToInt(tappedFactor) * SAMPLE_INTERVAL;
                var delta = tappedMs - soundMs;

                _Samples.Add(new()
                {
                    Ms = soundMs,
                    Delta = delta
                });

                TapArea.SetText($"{delta}ms\n\nKeep Going!");
                TapArea.DoBounce();
            }
        }

        IEnumerator CalibrationTimeUpdater(float duration)
        {
            _Timing = 0.0f;
            while (_Timing <= duration)
            {
                _Timing += Time.deltaTime;
                yield return null;
            }

            CalibrationFinished();
        }

        private void CalibrationFinished()
        {
            _CalibratingFinished = true;

            if (_Samples.Count > 8)
            {
                var deltaSorted = _Samples.OrderBy(x => x.Delta);
                var lowest = deltaSorted.First().Delta;
                var accurate = (int)_Samples.Select(x => MathfE.AbsDelta(x.Delta, lowest)).Average();
                var avgDelta = Mathf.RoundToInt((float)_Samples.Average(x => x.Delta));

                var text = string.Empty;
                if (accurate < 40)
                {
                    text = "Amazing Accuracy!";
                }
                else if (accurate < 80)
                {
                    text = "Nicely done!";
                }
                else if (accurate < 120)
                {
                    text = "Good Job!";
                }
                else if (accurate < 160)
                {
                    text = "Good enough..?";
                }
                else
                {
                    text = "Try again...";
                }

                TapArea.SetText($"Your Offset: {avgDelta}ms\n\nAccuracy (Lower-Better): {accurate}\n\n{text}");

                //TODO: Move this to Event later
                UserSetting.Offset = avgDelta;
                UserSetting.Save();
            }
            else
            {
                TapArea.SetText($"Too less Sample were input...\n\nTry again!");
            }
            
            Invoke(nameof(ResetState), 4.0f);
        }
    }
}
