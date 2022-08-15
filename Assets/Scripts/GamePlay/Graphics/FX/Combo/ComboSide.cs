using GamePlay.Judge;
using GamePlay.Scoring;
using TMPro;
using UnityEngine;

namespace GamePlay.Graphics.FX.Combo
{
    public sealed class ComboSide : MonoBehaviour
    {
        public Animator Anim;
        public Transform ResizeTransform;
        public TextMeshPro ComboText;
        public TextMeshPro TypeText;

        private readonly static int DO_DISPLAY = Animator.StringToHash("DoDisplay");

        public void Display(JudgeType type, Color color)
        {
            var text = type switch
            {
                JudgeType.PerfectPlus => "Perfect<sup>+</sup>!",
                JudgeType.Perfect => "Perfect!",
                JudgeType.Good => "Good!",
                JudgeType.Miss => "Miss...",
                _ => throw new System.NotImplementedException(),
            };


            TypeText.text = text;
            TypeText.color = color;
            ComboText.text = ScoreManager.ComboCount.ToString();
            ComboText.color = color;

            Anim.Play(DO_DISPLAY);

            if (type == JudgeType.Miss)
            {
                ComboText.gameObject.SetActive(false);
            }
            else
            {
                ComboText.gameObject.SetActive(true);
            }
        }
    }
}
