using System;
using System.Collections.Generic;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    public abstract class PropertyValidatorBase
    {
        public abstract void ValidateProperty(SerializedProperty property);
    }

    public static class ValidatorAttributeExtensions
    {
        private static readonly Dictionary<Type, PropertyValidatorBase> s_ValidatorsByAttributeType;

        static ValidatorAttributeExtensions()
        {
            s_ValidatorsByAttributeType = new()
            {
                [typeof(MinValueAttribute)] = new MinValuePropertyValidator(),
                [typeof(MaxValueAttribute)] = new MaxValuePropertyValidator(),
                [typeof(RequiredAttribute)] = new RequiredPropertyValidator(),
                [typeof(ValidateInputAttribute)] = new ValidateInputPropertyValidator()
            };
        }

        public static PropertyValidatorBase GetValidator(this ValidatorAttribute attr)
        {
            PropertyValidatorBase validator;
            if (s_ValidatorsByAttributeType.TryGetValue(attr.GetType(), out validator))
            {
                return validator;
            }
            else
            {
                return null;
            }
        }
    }
}
