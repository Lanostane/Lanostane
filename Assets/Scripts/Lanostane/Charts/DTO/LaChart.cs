using System.Linq;
using Utils;

namespace Lanostane.Charts.DTO
{
#pragma warning disable 0649 //Never Assign Warning
    public class LaChart
    {
        public LaEvent[] events;
        public LaEvent[] bpm;
        public LaScroll[] scroll;
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
                                    Ease = (LST_Ease)j.d_e
                                });
                            }
                        }

                        chart.HoldNotes.Add(new()
                        {
                            Timing = e.Timing,
                            Duration = e.Duration,
                            Degree = e.Degree,
                            Highlight = e.Combination,
                            Size = e.Size.ToValidSize(),
                            Joints = joints.ToArray()
                        });
                        break;

                    case LaEventType.DefaultMotion:
                        chart.Default = new()
                        {
                            Rotation = e.Degree,
                            Height = e.ctp2,
                            Degree = e.ctp1,
                            Radius = e.ctp
                        };
                        break;

                    case LaEventType.RotationMotion:
                        chart.RotationMos.Add(new()
                        {
                            Timing = e.Timing,
                            Duration = e.Duration,
                            Ease = (LST_Ease)e.cfmi,
                            DeltaRotation = e.ctp
                        });
                        break;

                    case LaEventType.VerticalMotion:
                        chart.HeightMos.Add(new()
                        {
                            Timing = e.Timing,
                            Duration = e.Duration,
                            Ease = (LST_Ease)e.cfmi,
                            DeltaHeight = e.ctp
                        });
                        break;

                    case LaEventType.XYLinearMotion:
                        chart.LinearMos.Add(new()
                        {
                            Timing = e.Timing,
                            Duration = e.Duration,
                            Ease = (LST_Ease)e.cfmi,
                            NewDegree = e.ctp,
                            NewRadius = e.ctp1
                        });
                        break;

                    case LaEventType.XYCirclerMotion:
                        chart.CirclerMos.Add(new()
                        {
                            Timing = e.Timing,
                            Duration = e.Duration,
                            Ease = (LST_Ease)e.cfmi,
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

    public class LaEvent
    {
        public int Id;
        public LaEventType Type;
        public float Timing;
        public float Duration;
        public float ctp;
        public float ctp1;
        public float ctp2;
        public int cfmi;
        public bool cflg;
        public float Degree;
        public LST_NoteSize Size;
        public bool Combination;
        public float Bpm;
        public LaJointCollection joints;
    }

    public class LaScroll
    {
        public float Timing;
        public float speed;
    }

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

    public class LaJointCollection
    {
        public int j_count;
        public LaJoint[] j;
    }

    public class LaJoint
    {
        public int idx;
        public float d_deg;
        public float d_time;
        public int d_e;
    }
#pragma warning restore 0649
}
