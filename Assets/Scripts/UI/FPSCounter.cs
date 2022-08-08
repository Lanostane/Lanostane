using GamePlay.Scoring;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class FPSCounter : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI TextField;

        void Start()
        {
            StartCoroutine(FPSUpdate());
        }

        int fps = 0;
        void Update()
        {
            fps++;
        }

        IEnumerator FPSUpdate()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.25f);
                TextField.text = $" FPS: {fps * 4} / Combo: {ScoreManager.ComboCount}";
                fps = 0;
            }
        }
    }
}
