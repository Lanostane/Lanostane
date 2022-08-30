using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ChildTypeEnumValueAttribute : Attribute
    {
        public int EnumValue;

        public ChildTypeEnumValueAttribute(int enumValue)
        {
            EnumValue = enumValue;
        }
    }

    public static class ChildTypeComponents<E> where E : struct, Enum
    {
        public static IEnumerable<C> FindAndMatchTo<C>(Component holder, Func<C, E> enumIDSelector)
        {
            if (holder == null)
                return Enumerable.Empty<C>();

            return FindAndMatchTo<C>(holder.transform, holder, enumIDSelector);
        }

        public static IEnumerable<C> FindAndMatchTo<C>(Transform parent, object holder, Func<C, E> enumIDSelector)
        {
            if (enumIDSelector == null)
                return Enumerable.Empty<C>();

            if (parent == null)
                return Enumerable.Empty<C>();

            if (holder == null)
                return Enumerable.Empty<C>();

            var holderFields = holder.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => typeof(C).IsAssignableFrom(x.FieldType))
                .Where(x => x.GetCustomAttribute<ChildTypeEnumValueAttribute>() != null)
                .Select(x =>
                    (
                        x,
                        x.GetCustomAttribute<ChildTypeEnumValueAttribute>()
                    ).ToTuple()
                );

            if (holderFields.Count() <= 0)
            {
                Debug.Log("Holder Fields count were 0");
                return Enumerable.Empty<C>();
            }

            var comps = parent.GetComponentsInChildren<C>(includeInactive: true);
            foreach (var comp in comps)
            {
                var id = (int)(object)enumIDSelector.Invoke(comp);
                var item = holderFields
                    .FirstOrDefault(x => x.Item2.EnumValue == id);
                if (item != null)
                {
                    item.Item1.SetValue(holder, comp);
                }
            }

            return comps;
        }
    }
}
