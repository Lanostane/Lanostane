﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Unity;

namespace Utils
{
    public enum SortBy : byte
    {
        AscendingOrder,
        DescendingOrder
    }

    public sealed class FastSortedList<T>
    {
        private T[] _Cache = Array.Empty<T>();
        private T[] _UnsortedCache = Array.Empty<T>();
        private readonly List<T> _InternalList = new();
        private bool _HasDirty = false;
        private readonly Func<T, IComparable> _SortFunc;
        private readonly SortBy _SortBy;

        public FastSortedList(Func<T, IComparable> selectSortFunc, SortBy sortBy)
        {
            _SortFunc = selectSortFunc;
            _SortBy = sortBy;
        }

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
                TryUpdateCache();
                return _Cache;
            }
        }

        public T[] UnsortedItems
        {
            get
            {
                TryUpdateCache();
                return _UnsortedCache;
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

        private void TryUpdateCache()
        {
            if (_HasDirty)
            {
                if (_SortBy == SortBy.DescendingOrder)
                {
                    _Cache = _InternalList.OrderByDescending(_SortFunc).ToArray();
                }
                else
                {
                    _Cache = _InternalList.OrderBy(_SortFunc).ToArray();
                }
                _UnsortedCache = _InternalList.ToArray();
                _HasDirty = false;
            }
        }
    }
}
