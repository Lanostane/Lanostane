﻿using Charts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace GamePlay.Graphics
{
    public struct JointInfo
    {
        public float StartTiming;
        public float EndTiming;
        public float Duration;

        public float DeltaDegree;
        public float StartDeg;
        public float EndDeg;
        public LST_Ease Ease;
    }

    public sealed class LongNoteJointCollection : IEnumerable<JointInfo>
    {
        public float StartTiming => _Timing;
        public float EndTiming => _Timing + _Duration;
        public float TotalDuration => _Duration;

        private float _Timing;
        private float _Duration;
        private readonly List<JointInfo> _Joints = new();

        public void Setup(LST_LongNoteInfo info)
        {
            _Timing = info.Timing;
            _Duration = info.Duration;


            if (info.Joints.Length <= 0)
            {
                _Joints.Add(new()
                {
                    StartTiming = info.Timing,
                    EndTiming = info.Timing + info.Duration,
                    Duration = info.Duration,
                    DeltaDegree = 0.0f,
                    StartDeg = MathfE.AbsAngle(info.Degree),
                    EndDeg = MathfE.AbsAngle(info.Degree),
                    Ease = LST_Ease.Linear
                });
                return;
            }

            var timing = info.Timing;
            var degree = MathfE.AbsAngle(info.Degree);
            var fullEndTime = info.Timing + info.Duration;
            foreach (var joint in info.Joints)
            {
                _Joints.Add(new()
                {
                    StartTiming = timing,
                    EndTiming = Mathf.Min(timing + joint.Duration, fullEndTime),
                    Duration = joint.Duration,
                    StartDeg = degree,
                    DeltaDegree = joint.DeltaDegree,
                    EndDeg = degree + joint.DeltaDegree,
                    Ease = joint.Ease
                });

                timing += joint.Duration;
                degree += joint.DeltaDegree;
            }
        }

        public float GetDegreeByProgress(float progress01)
        {
            var time = Mathf.Lerp(_Timing, _Timing + _Duration, progress01);
            return GetDegreeByTime(time);
        }

        public float GetDegreeByTime(float time)
        {
            if (time >= _Timing + _Duration)
            {
                return _Joints.Last().EndDeg;
            }

            if (time <= _Timing)
            {
                return _Joints.First().StartDeg;
            }

            var index = _Joints.FindLastIndex(x => x.StartTiming <= time);
            if (index < 0)
            {
                Debug.LogError("This should not happen :/");
                return 0.0f;
            }
            else
            {
                var joint = _Joints[index];
                float p = Mathf.InverseLerp(joint.StartTiming, joint.EndTiming, time);
                return Mathf.Lerp(joint.StartDeg, joint.EndDeg, joint.Ease.EvalClamped(p));
            }
        }

        public float GetProgressByTime(float time)
        {
            return Mathf.InverseLerp(_Timing, _Timing + _Duration, time);
        }

        public void Clear()
        {
            _Joints.Clear();
        }

        public IEnumerator<JointInfo> GetEnumerator()
        {
            return _Joints.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Joints.GetEnumerator();
        }
    }
}