using LST.Player.Scrolls;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils.Maths;

namespace LST.Player.Graphics
{
    public sealed class LongNoteGraphicCollection
    {
        private readonly List<ILongNoteGraphic> _List = new();

        private ILongNoteGraphic[] _CachedGraphics = Array.Empty<ILongNoteGraphic>();
        private float[] _CachedTimings = Array.Empty<float>();
        private ScrollTiming[] _CachedScrollTimings = Array.Empty<ScrollTiming>();
        private bool _IsDirty = false;
        private ScrollProgress[] _ScrollProgressBuffer = Array.Empty<ScrollProgress>();

        public ILongNoteGraphic[] Graphics
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

        public ScrollTiming[] HeadScrollTimings
        {
            get
            {
                TryUpdateArrays();
                return _CachedScrollTimings;
            }
        }

        public ScrollProgress[] HeadScrollProgressBuffer
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
            _CachedScrollTimings = _List.Select(x => x.HeadScrollTiming).ToArray();
            _ScrollProgressBuffer = new ScrollProgress[_List.Count];
            _IsDirty = false;
        }

        public void Add(ILongNoteGraphic graphic)
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
