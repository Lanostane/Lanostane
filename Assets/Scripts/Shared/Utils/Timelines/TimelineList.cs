using Codice.CM.WorkspaceServer.Lock;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using Utils;
using Utils.Maths;

namespace Utils.Timelines
{
    public abstract partial class TimelineList<T> : IList<TimelineItem<T>> where T : struct
    {
        private readonly FastList<TimelineItem<T>> _InternalList = new();

        protected TimelineItemData<T>[] JobDatas { get; private set; } = Array.Empty<TimelineItemData<T>>();

        internal void HandleItemUpdate()
        {
            ReorderItems();
            ItemUpdated?.Invoke();
        }

        private void ClearItemsOwnership()
        {
            var items = _InternalList.Items;
            var length = items.Length;
            for (int i = 0; i<length; i++)
            {
                items[i]._OwnerList = null;
            }
        }

        private void ReorderItems()
        {
            var ordered = _InternalList.OrderBy(x => x.Timing).ToArray();
            var length = ordered.Length;

            JobDatas = ordered.Select(x=>x.ToData()).ToArray();

            for (var i = 0; i < length; i++)
            {
                var item = ordered[i];
                item._OwnerList = this;
                ordered[i] = item;
            }

            _InternalList.Clear();
            _InternalList.AddRange(ordered);
        }
    }
}
