﻿using System;
using System.Linq;
using UnityEngine.Scripting;
using Utils;

namespace Lanostane.Models.ThirdParty
{
#pragma warning disable 0649 //Never Assign Warning
    [Preserve]
    public class LaChart
    {
        public LaEvent[] events = Array.Empty<LaEvent>();
        public LaEvent[] bpm = Array.Empty<LaEvent>();
        public LaScroll[] scroll = Array.Empty<LaScroll>();
        public float eos;

        public LST_Chart CreateLanostaneChart()
        {
            var chart = new LST_Chart
            {
                SongLength = eos
            };

            foreach (var e in events)
            {
                switch (e.Type)
                {
                    case LaEventType.Click:
                        chart.TapNotes.Add(new()
                        {
                            Timing = e.Timing,
                            Degree = e.Degree,
                            Highlight = e.Combination,
                            Size = e.Size.ToValidSize()
                        });
                        break;

                    case LaEventType.Catch:
                        chart.CatchNotes.Add(new()
                        {
                            Timing = e.Timing,
                            Degree = e.Degree,
                            Highlight = e.Combination,
                            Size = e.Size.ToValidSize()
                        });
                        break;

                    case LaEventType.FlickIn:
                    case LaEventType.FlickOut:
                        chart.FlickNotes.Add(new()
                        {
                            Timing = e.Timing,
                            Degree = e.Degree,
                            Highlight = e.Combination,
                            Size = e.Size.ToValidSize(),
                            Direction = e.Type == LaEventType.FlickIn ? LST_FlickDir.In : LST_FlickDir.Out
                        });
                        break;

                    case LaEventType.Hold:
                        var joints = TempList<LST_Joint>.GetList();

                        if (e.joints != null)
                        {
                            for (int i = 0; i < e.joints.j.Length; i++)
                            {
                                var j = e.joints.j[i];
                                joints.Add(new()
                                {
                                    Duration = j.d_time,
                                    DeltaDegree = j.d_deg,
                                    Ease = j.d_e.ToLSTEase()
                                });
                            }
                        }

                        var holdNote = new LST_Hold()
                        {
                            Timing = e.Timing,
                            Duration = e.Duration,
                            Degree = e.Degree,
                            Highlight = e.Combination,
                            Size = e.Size.ToValidSize()
                        };
                        holdNote.Joints.AddRange(joints);
                        chart.HoldNotes.Add(holdNote);
                        break;

                    case LaEventType.DefaultMotion:
                        chart.Default.Rotation = e.Degree;
                        chart.Default.Height = e.ctp2;
                        chart.Default.Degree = e.ctp1;
                        chart.Default.Radius = e.ctp;
                        break;

                    case LaEventType.RotationMotion:
                        chart.RotationMos.Add(new()
                        {
                            Timing = e.Timing,
                            Duration = e.Duration,
                            Ease = e.cfmi.ToLSTEase(),
                            DeltaRotation = e.ctp
                        });
                        break;

                    case LaEventType.VerticalMotion:
                        chart.HeightMos.Add(new()
                        {
                            Timing = e.Timing,
                            Duration = e.Duration,
                            Ease = e.cfmi.ToLSTEase(),
                            DeltaHeight = e.ctp
                        });
                        break;

                    case LaEventType.XYLinearMotion:
                        chart.LinearMos.Add(new()
                        {
                            Timing = e.Timing,
                            Duration = e.Duration,
                            Ease = e.cfmi.ToLSTEase(),
                            NewDegree = e.ctp,
                            NewRadius = e.ctp1
                        });
                        break;

                    case LaEventType.XYCirclerMotion:
                        chart.CirclerMos.Add(new()
                        {
                            Timing = e.Timing,
                            Duration = e.Duration,
                            Ease = e.cfmi.ToLSTEase(),
                            DeltaDegree = e.ctp,
                            DeltaRadius = e.ctp1
                        });
                        break;
                }
            }

            foreach (var b in bpm)
            {
                if (b.Type == LaEventType.DefaultBPM)
                {
                    chart.StartBPM = b.Bpm;
                }

                chart.BPMs.Add(new()
                {
                    Timing = b.Timing,
                    BPM = b.Bpm
                });
            }

            foreach (var s in scroll)
            {
                chart.Scrolls.Add(new()
                {
                    Timing = s.Timing,
                    Speed = s.speed
                });
            }

            return chart;
        }
    }

    [Preserve]
    public class LaEvent
    {
        public int Id;
        public LaEventType Type;
        public float Timing;
        public float Duration;
        public float ctp;
        public float ctp1;
        public float ctp2;
        public LaEaseType cfmi;
        public bool cflg;
        public float Degree;
        public LST_NoteSize Size;
        public bool Combination;
        public float Bpm;
        public LaJointCollection joints;

        public LaEvent() { }
    }

    [Preserve]
    public class LaScroll
    {
        public float Timing;
        public float speed;

        public LaScroll() { }
    }

    [Preserve]
    public enum LaEventType
    {
        Click = 0,
        Catch = 4,
        FlickIn = 2,
        FlickOut = 3,

        Hold = 5,

        XYLinearMotion = 11,
        XYCirclerMotion = 8,
        VerticalMotion = 10,
        RotationMotion = 13,

        DefaultMotion = 12,
        DefaultBPM = 6
    }

    [Preserve]
    public enum LaEaseType
    {
        Linear = 0,

        InQuart = 1,
        OutQuart = 2,
        InOutQuart = 3,

        InCubic = 4,
        OutCubic = 5,
        InOutCubic = 6,

        InExpo = 7,
        OutExpo = 8,
        InOutExpo = 9,

        InSine = 10,
        OutSine = 11,
        InOutSine = 12
    }

    [Preserve]
    public class LaJointCollection
    {
        public int j_count;
        public LaJoint[] j;

        public LaJointCollection() { }
    }

    [Preserve]
    public class LaJoint
    {
        public int idx;
        public float d_deg;
        public float d_time;
        public LaEaseType d_e;

        public LaJoint() { }
    }

    public static class LaEaseConverter
    {
        public static LST_Ease ToLSTEase(this LaEaseType laEase)
        {
            return laEase switch
            {
                LaEaseType.Linear => LST_Ease.Linear,
                LaEaseType.InQuart => LST_Ease.InQuart,
                LaEaseType.OutQuart => LST_Ease.OutQuart,
                LaEaseType.InOutQuart => LST_Ease.InOutQuart,
                LaEaseType.InCubic => LST_Ease.InCubic,
                LaEaseType.OutCubic => LST_Ease.OutCubic,
                LaEaseType.InOutCubic => LST_Ease.InOutCubic,
                LaEaseType.InExpo => LST_Ease.InExpo,
                LaEaseType.OutExpo => LST_Ease.OutExpo,
                LaEaseType.InOutExpo => LST_Ease.InOutExpo,
                LaEaseType.InSine => LST_Ease.InSine,
                LaEaseType.OutSine => LST_Ease.OutSine,
                LaEaseType.InOutSine => LST_Ease.InOutSine,
                _ => LST_Ease.Linear,
            };
        }
    }
#pragma warning restore 0649
}