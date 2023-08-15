using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Unity
{
    [RequireComponent(typeof(Canvas))]
    public sealed class AutoAttatchToMainCamera : MonoBehaviour
    {
        public RenderMode Mode = RenderMode.ScreenSpaceCamera;
        public float PlaneDistance = 0.0f;

        private void Awake()
        {
            if (Mode == RenderMode.ScreenSpaceOverlay)
            {
                Debug.LogError("ScreenSpaceOverlay is not supported!!!");
                return;
            }

            var canvas = GetComponent<Canvas>();
            canvas.renderMode = Mode;
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = PlaneDistance;
        }
    }
}
