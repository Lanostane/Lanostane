using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Maths;

namespace Utils.Timelines
{
    public class TimelineItem<T> where T : struct
    {
        internal TimelineList<T> _OwnerList;

        private float _Timing, _Duration;
        private T _Param;
        private bool _IsDelta;
        private EaseType _EaseType;

        public float Timing
        {
            get { return _Timing; }
            set { _Timing = value; ValueChanged(); }
        }

        public float Duration
        {
            get { return _Duration; }
            set { _Duration = value; ValueChanged(); }
        }

        public T Param
        {
            get { return _Param; }
            set { _Param = value; ValueChanged(); }
        }

        public bool IsDeltaMode
        {
            get { return _IsDelta; }
            set { _IsDelta = value; ValueChanged(); }
        }

        public EaseType Easing
        {
            get { return _EaseType; }
            set { _EaseType = value; ValueChanged(); }
        }

        public TimelineItemData<T> ToData()
        {
            return new TimelineItemData<T>()
            {
                Timing = _Timing,
                Duration = _Duration,
                Param = _Param,
                IsDeltaMode = _IsDelta,
                Easing = _EaseType
            };
        }

        private void ValueChanged()
        {
            _OwnerList?.HandleItemUpdate();
        }
    }
}
