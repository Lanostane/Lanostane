using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public class FastList<T>
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
    }
}
