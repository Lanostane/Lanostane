using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Lst.BGS
{
    public sealed class ImageBG : BGBase
    {
        public Image Graphic;

        public void SetAlpha(float alpha)
        {
            var color = Color.white;
            color.a = alpha;
            Graphic.color = color;
        }

        public void SetSprite(Sprite sprite)
        {
            Graphic.sprite = sprite;
        }

        public override void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
