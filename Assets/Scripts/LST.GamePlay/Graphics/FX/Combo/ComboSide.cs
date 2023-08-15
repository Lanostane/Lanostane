using LST.GamePlay.Judge;
using LST.GamePlay.Scoring;
using TMPro;
using UnityEngine;

namespace LST.GamePlay.Graphics
{
    public sealed class ComboSide : MonoBehaviour
    {
        public Animator Anim;
        public GameObject PerfectPlusSprite;
        public GameObject PerfectSprite;
        public GameObject GoodSprite;
        public GameObject MissSprite;
        public TextMeshPro ComboText;

        private readonly static int s_HashDoDisplay = Animator.StringToHash("DoDisplay");

        public void Display(JudgeType type, Color color)
        {
            PerfectPlusSprite.SetActive(type == JudgeType.PurePerfect);
            PerfectSprite.SetActive(type == JudgeType.Perfect);
            GoodSprite.SetActive(type == JudgeType.Good);
            MissSprite.SetActive(type == JudgeType.Miss);

            Anim.Play(s_HashDoDisplay);

            if (type == JudgeType.Miss)
            {
                ComboText.gameObject.SetActive(false);
            }
            else
            {
                ComboText.gameObject.SetActive(true);
                ScoreStrings.SetComboBuffer(ScoreManager.ComboCount);
                ComboText.SetCharArray(ScoreStrings.ComboBuffer, 0, ScoreStrings.ComboCharsCount);
                ComboText.color = color;
            }
        }
    }
}
