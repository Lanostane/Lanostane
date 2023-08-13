using LST.Player.Judge;
using LST.Player.Scoring;
using TMPro;
using UnityEngine;

namespace LST.Player.Graphics
{
    public sealed class ComboSide : MonoBehaviour
    {
        public Animator Anim;
        public GameObject PerfectPlusSprite;
        public GameObject PerfectSprite;
        public GameObject GoodSprite;
        public GameObject MissSprite;
        public TextMeshPro ComboText;

        private readonly static int DO_DISPLAY = Animator.StringToHash("DoDisplay");

        public void Display(JudgeType type, Color color)
        {
            PerfectPlusSprite.SetActive(type == JudgeType.PurePerfect);
            PerfectSprite.SetActive(type == JudgeType.Perfect);
            GoodSprite.SetActive(type == JudgeType.Good);
            MissSprite.SetActive(type == JudgeType.Miss);

            Anim.Play(DO_DISPLAY);

            if (type == JudgeType.Miss)
            {
                ComboText.gameObject.SetActive(false);
            }
            else
            {
                ComboText.gameObject.SetActive(true);
                ComboText.SetText(ScoreManager.ComboCountString);
                ComboText.color = color;
            }
        }
    }
}
