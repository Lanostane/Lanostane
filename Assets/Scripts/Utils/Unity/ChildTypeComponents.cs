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
        public static void FindAndMatchTo<C>(Transform parent, object holder, Func<C, E> enumIDSelector)
        {
            if (enumIDSelector == null)
                return;

            if (parent == null)
                return;

            if (holder == null)
                return;

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
                return;
            }
                

            foreach (var comp in parent.GetComponentsInChildren<C>(includeInactive: true))
            {
                var id = (int)(object)enumIDSelector.Invoke(comp);
                var item = holderFields
                    .FirstOrDefault(x => x.Item2.EnumValue == id);
                if (item != null)
                {
                    item.Item1.SetValue(holder, comp);
                }
            }
        }
    }
}
