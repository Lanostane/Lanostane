using GamePlay.Scoring;
using System.Collections;
using UnityEngine;

namespace UI.Comps
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
                yield return new WaitForSeconds(0.125f);
                TextField.text = $"{fps * 8}";
                fps = 0;
            }
        }
    }
}
