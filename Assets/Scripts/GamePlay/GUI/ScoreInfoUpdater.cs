using GamePlay.Scoring;
using TMPro;
using UnityEngine;

namespace GamePlay.GUI
{
    public class ScoreInfoUpdater : MonoBehaviour
    {
        public TextMeshProUGUI ScoreText;

        void Awake()
        {
            ScoreManager.ScoreUpdated += ScoreUpdated;
        }

        void OnDestroy()
        {
            ScoreManager.ScoreUpdated -= ScoreUpdated;
        }

        void ScoreUpdated()
        {
            if (ScoreManager.IsPerfect)
            {
                ScoreText.text = $"<color=orange>{ScoreManager.ScoreRounded:0000000000}</color>";
            }
            else if (ScoreManager.IsAllCombo)
            {
                ScoreText.text = $"<color=blue>{ScoreManager.ScoreRounded:0000000000}</color>";
            }
            else
            {
                ScoreText.text = $"<color=white>{ScoreManager.ScoreRounded:0000000000}</color>";
            }
        }
    }
}
