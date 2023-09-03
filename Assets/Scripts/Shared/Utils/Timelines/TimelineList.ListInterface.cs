using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Timelines
{
    public abstract partial class TimelineList<T> : IList<TimelineItem<T>> where T : struct
    {
        public int Count => _InternalList.Count;
        public bool IsReadOnly => false;

        public TimelineItem<T> this[int index]
        {
            get => _InternalList[index];
            set
            {
                _InternalList[index] = value;
                HandleItemUpdate();
            }
        }

        public int IndexOf(TimelineItem<T> item)
        {
            return _InternalList.IndexOf(item);
        }

        public void Insert(int index, TimelineItem<T> item)
        {
            _InternalList.Insert(index, item);
            HandleItemUpdate();
        }

        public void RemoveAt(int index)
        {
            ClearItemsOwnership();
            _InternalList.RemoveAt(index);
            HandleItemUpdate();
        }

        public void Add(TimelineItem<T> item)
        {
            _InternalList.Add(item);
            HandleItemUpdate();
        }

        public void Clear()
        {
            ClearItemsOwnership();
            _InternalList.Clear();
            HandleItemUpdate();
        }

        public bool Contains(TimelineItem<T> item)
        {
            return _InternalList.Contains(item);
        }

        public void CopyTo(TimelineItem<T>[] array, int arrayIndex)
        {
            _InternalList.CopyTo(array, arrayIndex);
        }

        public bool Remove(TimelineItem<T> item)
        {
            var result = _InternalList.Remove(item);
            if (result)
            {
                item._OwnerList = null;
                HandleItemUpdate();
            }
            return result;
        }

        public IEnumerator<TimelineItem<T>> GetEnumerator()
        {
            return _InternalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _InternalList.GetEnumerator();
        }
    }
}
