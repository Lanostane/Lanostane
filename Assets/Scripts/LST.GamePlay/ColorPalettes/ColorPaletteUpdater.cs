using Lanostane.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;
using Utils.Maths;

namespace LST.GamePlay.ColorPalettes
{
    internal sealed class ColorPaletteUpdater : MonoBehaviour, IColorPaletteUpdater
    {
        public event Action<LST_ColorPalette> PaletteUpdated;

        private readonly FastSortedList<LST_ColorPaletteChange> _ChangeList = new(x=>x.Timing, SortBy.AscendingOrder);

        private void Awake()
        {
            GamePlays.ColorPaletteUpdater = this;
        }

        private void OnDestroy()
        {
            GamePlays.ColorPaletteUpdater = null;
        }

        public void AddFromChart(LST_Chart chart)
        {
            _ChangeList.AddRange(chart.PaletteSwaps);
        }

        public void TimeUpdate(float chartTime)
        {
            var length = _ChangeList.Length;
            var newPalette = new LST_ColorPalette();
            for (int i = 0; i< length; i++)
            {
                var item = _ChangeList.Items[i];
                var startTime = item.Timing;
                var endTime = startTime + item.Duration;

                if (chartTime >= startTime)
                {
                    if (chartTime <= endTime)
                    {
                        var p = Mathf.InverseLerp(startTime, endTime, chartTime);
                        newPalette = LST_ColorPalette.Lerp(newPalette, item.Palette, item.Ease.EvalClamped(p));
                    }
                    else
                    {
                        newPalette = item.Palette;
                    }
                    continue;
                }

                if (chartTime < startTime)
                    break;
            }

            PaletteUpdated?.Invoke(newPalette);
        }

        public void CleanUp()
        {
            _ChangeList.Clear();   
        }
    }
}
