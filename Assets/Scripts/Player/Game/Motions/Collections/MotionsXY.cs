using Lanostane.Charts;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;
using Utils.Maths;

namespace LST.Player.Motions
{
    public struct XYMotion : IMotion
    {
        public float Timing { get; set; }
        public float Duration { get; set; }

        public float Degree;
        public float Radius;

        public PolarPoint Start;
        public PolarPoint End;
        public LST_Ease Ease;

        public bool IsLinear;
    }

    public sealed class MotionsXY : MotionCollection<XYMotion>
    {
        public void AddMotion(LST_XYLinearMotion linear)
        {
            MotionDataHolder.Add(new()
            {
                Timing = linear.Timing,
                Duration = linear.Duration,
                Ease = linear.Ease,
                IsLinear = true,
                Degree = linear.NewDegree,
                Radius = linear.NewRadius
            });
        }

        public void AddMotion(LST_XYCirclerMotion circler)
        {
            MotionDataHolder.Add(new()
            {
                Timing = circler.Timing,
                Duration = circler.Duration,
                Ease = circler.Ease,
                IsLinear = false,
                Degree = circler.DeltaDegree,
                Radius = circler.DeltaRadius
            });
        }

        public override void UpdateMotionAbsData()
        {
            TempHolder.Clear();

            var currentPoint = new PolarPoint(GamePlayManager.MotionUpdater.StartingRho, GamePlayManager.MotionUpdater.StartingTheta);
            foreach (var xy in MotionDataHolder.OrderBy(x => x.Timing))
            {
                var newXy = xy;

                PolarPoint p = new(xy.Radius, xy.Degree);
                if (xy.IsLinear)
                {
                    newXy.Start = currentPoint;
                    newXy.End = p.Normalized();
                    currentPoint = newXy.End;
                }
                else
                {
                    newXy.Start = currentPoint;
                    newXy.End = currentPoint + p;
                    if (newXy.End.Rho < 0.0f)
                    {
                        currentPoint = newXy.End.Normalized();
                    }
                    else
                    {
                        currentPoint = newXy.End;
                    }
                }

                TempHolder.Add(newXy);
            }

            MotionDataHolder.Clear();
            MotionDataHolder.AddRange(TempHolder);

            //var json = JsonConvert.SerializeObject(MotionDataHolder);
            
            //GUIUtility.systemCopyBuffer = json;
        }

        public override void UpdateChartTime(float chartTime)
        {
            if (TryGetLastMotion(chartTime, out var motion))
            {
                UpdateMotion(motion, chartTime);
            }
        }
    }
}
