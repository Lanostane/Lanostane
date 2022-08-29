using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lanostane.UI.Comps.ChartSelection
{
    public abstract class ChartSelectionItem : MonoBehaviour
    {
        public Image Icon;
        public TextMeshProUGUI TitleText;

        public event Action OnSelected;
    }
}
