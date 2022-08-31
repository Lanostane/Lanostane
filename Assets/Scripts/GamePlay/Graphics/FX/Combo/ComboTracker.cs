using GamePlay.Judge;
using GamePlay.Scoring;
using UnityEngine;
using Utils.Maths;

namespace GamePlay.Graphics.FX.Combo
{
    public sealed class ComboTracker : MonoBehaviour
    {
        public Color PurePerfectColor;
        public Color PerfectColor;
        public Color GoodColor;
        public Color MissColor;

        public ComboSide Side0;
        public ComboSide Side1;
        public ComboSide Side2;
        public ComboSide Side3;

        public ComboSide Side4;
        public ComboSide Side5;
        public ComboSide Side6;
        public ComboSide Side7;

        void Awake()
        {
            ScoreManager.NoteRegistered += NoteRegistered;
        }

        void OnDestroy()
        {
            ScoreManager.NoteRegistered -= NoteRegistered;
        }

        void NoteRegistered(JudgeType type, float degree)
        {
            var color = type switch
            {
                JudgeType.PurePerfect => PurePerfectColor,
                JudgeType.Perfect => PerfectColor,
                JudgeType.Good => GoodColor,
                JudgeType.Miss => MissColor,
                _ => throw new System.NotImplementedException(),
            };

            degree = MathfE.AbsAngle(degree);

            if (degree > 337.5 || degree <= 22.5)
            {
                Side0.Display(type, color);
            }
            else if (22.5 < degree && degree <= 67.5)
            {
                Side1.Display(type, color);
            }
            else if (67.5 < degree && degree <= 112.5)
            {
                Side2.Display(type, color);
            }
            else if (112.5 < degree && degree <= 157.5)
            {
                Side3.Display(type, color);
            }
            else if (157.5 < degree && degree <= 202.5)
            {
                Side4.Display(type, color);
            }
            else if (202.5 < degree && degree <= 247.5)
            {
                Side5.Display(type, color);
            }
            else if (247.5 < degree && degree <= 292.5)
            {
                Side6.Display(type, color);
            }
            else if (292.5 < degree && degree <= 337.5)
            {
                Side7.Display(type, color);
            }
        }
    }
}
