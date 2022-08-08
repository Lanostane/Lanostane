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
            if (type == JudgeType.Miss)
            {
                Anim.Play(DO_DISPLAY);

                ComboText.gameObject.SetActive(false);
                TypeText.text = "Miss...";
                TypeText.color = color;
            }
            else
            {
                Anim.Play(DO_DISPLAY);

                ComboText.gameObject.SetActive(true);

                TypeText.color = color;
                ComboText.color = color;

                TypeText.text = type == JudgeType.Perfect ? "Perfect!" : "Good!";
                ComboText.text = ScoreManager.ComboCount.ToString();
            }
        }
    }
}
