using System.Collections;
using UnityEngine;

namespace LST.Player.UI
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
            var yielder = new WaitForSeconds(0.125f);
            while (true)
            {
                yield return yielder;
                TextField.text = (fps * 8).ToString();
                fps = 0;
            }
        }
    }
}
