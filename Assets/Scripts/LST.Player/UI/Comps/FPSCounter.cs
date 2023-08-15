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

        int _Fps = 0;
        void Update()
        {
            _Fps++;
        }

        IEnumerator FPSUpdate()
        {
            var yielder = new WaitForSeconds(0.125f);
            while (true)
            {
                yield return yielder;
                TextField.text = (_Fps * 8).ToString();
                _Fps = 0;
            }
        }
    }
}
