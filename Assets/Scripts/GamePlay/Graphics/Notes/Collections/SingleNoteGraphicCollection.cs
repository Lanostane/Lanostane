using GamePlay.Scrolls;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace GamePlay.Graphics.Collections
{
    public sealed class SingleNoteGraphicCollection
    {
        private readonly List<ISingleNoteGraphic> _List = new();

        private ISingleNoteGraphic[] _CachedGraphics = Array.Empty<ISingleNoteGraphic>();
        private float[] _CachedTimings = Array.Empty<float>();
        private MiliSec[] _CachedAmounts = Array.Empty<MiliSec>();
        private bool _IsDirty = false;
        private ScrollAmountInfo[] _CachedScrollAmountsBuffer = Array.Empty<ScrollAmountInfo>();

        public ISingleNoteGraphic[] Graphics
        {
            get
            {
                TryUpdateArrays();
                return _CachedGraphics;
            }
        }

        public float[] Timings
        {
            get
            {
                TryUpdateArrays();
                return _CachedTimings;
            }
        }

        public MiliSec[] ScrollAmounts
        {
            get
            {
                TryUpdateArrays();
                return _CachedAmounts;
            }
        }

        public ScrollAmountInfo[] ScrollAmountsBuffer
        {
            get
            {
                TryUpdateArrays();
                return _CachedScrollAmountsBuffer;
            }
        }

        private void TryUpdateArrays()
        {
            if (!_IsDirty)
                return;

            _CachedGraphics = _List.ToArray();
            _CachedTimings = _List.Select(x => x.Timing).ToArray();
            _CachedAmounts = _List.Select(x => x.ScrollTiming).ToArray();
            _CachedScrollAmountsBuffer = new ScrollAmountInfo[_List.Count];
            _IsDirty = false;
        }

        public void Add(ISingleNoteGraphic graphic)
        {
            _List.Add(graphic);
            _IsDirty = true;
        }

        public void Clear()
        {
            _List.Clear();
            _IsDirty = true;
        }
    }
}
