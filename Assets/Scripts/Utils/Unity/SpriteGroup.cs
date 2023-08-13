using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Unity
{
    public sealed class SpriteGroup : MonoBehaviour
    {
        private SpriteRenderer[] Renderers = Array.Empty<SpriteRenderer>();

        void Start()
        {
            Renderers = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
        }

        public void SetAlpha(float alpha)
        {
            int length = Renderers.Length;
            for(int i = 0; i < length; i++)
            {
                var renderer = Renderers[i];
                var color = renderer.color;
                color.a = alpha;
                renderer.color = color;
            }
        }
    }
}
