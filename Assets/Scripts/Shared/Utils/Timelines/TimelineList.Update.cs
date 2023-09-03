using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Timelines
{
    public abstract partial class TimelineList<T> : IList<TimelineItem<T>> where T : struct
    {
        public T CurrentValue { get; private set; } = default;

        public event Action<T> ValueUpdated;
        public event Action ItemUpdated;

        public void TimeUpdated(float time)
        {
            var value = GetValueFromTime(time);
            if (!IsEqual(in value, CurrentValue))
            {
                CurrentValue = value;
                ValueUpdated?.Invoke(value);
            }
        }

        public abstract T GetValueFromTime(float time);
        public abstract bool IsEqual(in T a, in T b);
    }
}
