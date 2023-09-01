using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public sealed class FastList<T> : IList<T>
    {
        private T[] _Cache = Array.Empty<T>();
        private readonly List<T> _InternalList = new();
        private bool _HasDirty = false;

        public int Length
        {
            get
            {
                return Items.Length;
            }
        }

        public T[] Items
        {
            get
            {
                if (_HasDirty)
                {
                    _Cache = _InternalList.ToArray();
                    _HasDirty = false;
                }

                return _Cache;
            }
        }

        public int Count => Length;
        public bool IsReadOnly => false;

        public T this[int index]
        {
            get => _InternalList[index];
            set => _InternalList[index] = value;
        }

        public void ForEach(Action<T> action)
        {
            if (action == null)
                return;

            var items = Items;
            var length = Length;
            for (int i = 0; i<length; i++)
            {
                action.Invoke(items[i]);
            }
        }

        public void Add(T item)
        {
            _InternalList.Add(item);
            _HasDirty = true;
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items == null || items.Count() <= 0)
                return;

            foreach (var item in items)
                _InternalList.Add(item);
            _HasDirty = true;
        }

        public void AddRange(params T[] items)
        {
            if (items == null || items.Length <= 0)
                return;

            foreach (var item in items)
                _InternalList.Add(item);
            _HasDirty = true;
        }

        public void Clear()
        {
            _InternalList.Clear();
            _HasDirty = true;
        }

        public int IndexOf(T item)
        {
            return _InternalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _InternalList.Insert(index, item);
            _HasDirty = true;
        }

        public void RemoveAt(int index)
        {
            _InternalList.RemoveAt(index);
            _HasDirty = true;
        }

        public bool Contains(T item)
        {
            return _InternalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _InternalList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var removed = _InternalList.Remove(item);
            _HasDirty = removed;
            return removed;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _InternalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _InternalList.GetEnumerator();
        }
    }
}
