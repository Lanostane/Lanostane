using LST.Player.Scrolls;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils.Maths;

namespace LST.Player.Graphics
{
    public sealed class SingleNoteGraphicCollection
    {
        private readonly List<ISingleNoteGraphic> _List = new();

        private ISingleNoteGraphic[] _CachedGraphics = Array.Empty<ISingleNoteGraphic>();
        private float[] _CachedTimings = Array.Empty<float>();
        private Millisecond[] _CachedAmounts = Array.Empty<Millisecond>();
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

        public Millisecond[] ScrollAmounts
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

        public void Clear(bool destroy)
        {
            if (destroy)
            {
                foreach (var note in _List)
                {
                    note.DestroyInstance();
                }
            }

            _List.Clear();
            _IsDirty = true;
        }
    }
}
