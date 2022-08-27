using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Comps
{
    [ExecuteInEditMode]
    public class GradientButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Image BaseImage;
        public Image GradientImage;

        public Color BaseColor;
        public Color SecondaryColor;

        public UnityEvent Pressed;
        public UnityEvent Down;
        public UnityEvent Up;
        
        void Update()
        {
            BaseImage.color = BaseColor;
            GradientImage.color = SecondaryColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Pressed?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Down?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Up?.Invoke();
        }
    }
}
