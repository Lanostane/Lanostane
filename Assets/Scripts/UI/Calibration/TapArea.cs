using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.Maths;

namespace UI.Calibration
{
    public class TapArea : MonoBehaviour, IPointerDownHandler
    {
        public TextMeshProUGUI Text;
        public event Action PointerDowned;

        public void SetText(string txt)
        {
            Text.text = txt;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDowned?.Invoke();
        }

        public void DoBounce()
        {
            StopAllCoroutines();
            StartCoroutine(Bounce());
        }

        IEnumerator Bounce()
        {
            var time = 0.0f;
            var duration = 0.15f;
            var factor = 1.0f / duration;
            while (time <= duration)
            {
                time += Time.deltaTime;
                var p = Ease.Bounce.In(Mathf.PingPong(time * factor, 1.0f));

                Text.transform.localScale = Vector3.one * Mathf.Lerp(0.9f, 1.0f, p);
                yield return null;
            }

            Text.transform.localScale = Vector3.one;
        }
    }
}
