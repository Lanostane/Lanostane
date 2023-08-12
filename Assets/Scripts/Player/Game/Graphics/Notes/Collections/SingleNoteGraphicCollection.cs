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
        private ScrollTiming[] _CachedAmounts = Array.Empty<ScrollTiming>();
        private bool _IsDirty = false;
        private ScrollProgress[] _ScrollProgressBuffer = Array.Empty<ScrollProgress>();

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

        public ScrollTiming[] ScrollTimings
        {
            get
            {
                TryUpdateArrays();
                return _CachedAmounts;
            }
        }

        public ScrollProgress[] ScrollProgressBuffer
        {
            get
            {
                TryUpdateArrays();
                return _ScrollProgressBuffer;
            }
        }

        private void TryUpdateArrays()
        {
            if (!_IsDirty)
                return;

            _CachedGraphics = _List.ToArray();
            _CachedTimings = _List.Select(x => x.Timing).ToArray();
            _CachedAmounts = _List.Select(x => x.ScrollTiming).ToArray();
            _ScrollProgressBuffer = new ScrollProgress[_List.Count];
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
