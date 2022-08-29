using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lanostane.UI.Comps
{
    public sealed class DifficultyCircle : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public Image Base;
        public Image Gradient;
        public Image Selected;

        public void SetSelected(bool selected)
        {
            Selected.gameObject.SetActive(selected);
        }

        public void SetDifficulty(float diff)
        {
            if (float.IsNaN(diff))
            {
                Text.SetText("?");
                return;
            }

            var intDiff = Mathf.FloorToInt(diff);
            var detailDiff = Mathf.RoundToInt((diff % (float)intDiff) * 10.0f);

            if (detailDiff > 0)
            {
                Text.SetText($"{intDiff}<sub>.{detailDiff}</sup>");
            }
            else
            {
                Text.SetText($"{intDiff}");
            }
        }

        public void SetColor(Color baseColor, Color gradientColor)
        {
            Base.color = baseColor;
            Gradient.color = gradientColor;
            Selected.color = gradientColor;
        }
    }
}
