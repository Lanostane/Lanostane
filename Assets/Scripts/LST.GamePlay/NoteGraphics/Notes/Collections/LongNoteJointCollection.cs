﻿using Lanostane.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Maths;

namespace LST.GamePlay.Graphics
{
    public struct LongNoteJointInfo
    {
        public float StartTiming;
        public float EndTiming;
        public float Duration;

        public float DeltaDegree;
        public float StartDeg;
        public float EndDeg;
        public EaseType Ease;
    }

    public sealed class LongNoteJointCollection : IEnumerable<LongNoteJointInfo>
    {
        public float StartTiming { get; private set; }
        public float EndTiming => StartTiming + TotalDuration;
        public float TotalDuration { get; private set; }

        private readonly List<LongNoteJointInfo> _Joints = new();

        public void Setup(LST_LongNoteInfo info)
        {
            StartTiming = info.Timing;
            TotalDuration = info.Duration;


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
                    Ease = EaseType.Linear
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
            var time = Mathf.Lerp(StartTiming, StartTiming + TotalDuration, progress01);
            return GetDegreeByTime(time);
        }

        public float GetDegreeByTime(float time)
        {
            if (time >= StartTiming + TotalDuration)
            {
                return _Joints.Last().EndDeg;
            }

            if (time <= StartTiming)
            {
                return _Joints.First().StartDeg;
            }

            if (TryGetLastJointIndex(time, out var index))
            {
                var joint = _Joints[index];
                float p = Mathf.InverseLerp(joint.StartTiming, joint.EndTiming, time);
                return Mathf.Lerp(joint.StartDeg, joint.EndDeg, joint.Ease.EvalClamped(p));
            }
            else
            {
                Debug.LogError("This should not happen :/");
                return 0.0f;
            }
        }

        public float GetProgressByTime(float time)
        {
            return Mathf.InverseLerp(StartTiming, StartTiming + TotalDuration, time);
        }

        public bool TryGetLastJointIndex(float timing, out int index)
        {
            index = -1;
            var length = _Joints.Count;
            for (int i = 0; i < length; i++)
            {
                var item = _Joints[i];
                if (item.StartTiming <= timing && i > index)
                {
                    index = i;
                }
            }

            return index > -1;
        }

        public void Clear()
        {
            _Joints.Clear();
        }

        public IEnumerator<LongNoteJointInfo> GetEnumerator()
        {
            return _Joints.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Joints.GetEnumerator();
        }
    }
}
