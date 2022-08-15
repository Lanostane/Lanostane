using GamePlay.Scoring;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreInfoUpdater : MonoBehaviour
    {
        public TextMeshProUGUI ScoreText;

        void Awake()
        {
            ScoreManager.ScoreUpdated += ScoreUpdated;
        }

        void Start()
        {
            ScoreUpdated();
        }

        void OnDestroy()
        {
            ScoreManager.ScoreUpdated -= ScoreUpdated;
        }

        void ScoreUpdated()
        {
            if (ScoreManager.IsPerfect)
            {
                ScoreText.text = $"<color=orange>{ScoreManager.ScoreString}</color>";
            }
            else if (ScoreManager.IsAllCombo)
            {
                ScoreText.text = $"<color=blue>{ScoreManager.ScoreString}</color>";
            }
            else
            {
                ScoreText.text = $"<color=white>{ScoreManager.ScoreString}</color>";
            }
        }
    }
}
