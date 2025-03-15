using System;
using System.Reflection;

namespace Sulfur.Contract.Helpers.Exts
{
    public static class TypeExt
    {
        public static bool TryGetCustomAttribute<TAttribute>(this Type type, out TAttribute attribute) where TAttribute : Attribute
        {
            attribute = type.GetCustomAttribute<TAttribute>();
            return attribute != null;
        }

        public static bool TryGetCustomAttribute(this Type type, Type attributeType, out Attribute attribute)
        {
            attribute = type.GetCustomAttribute(attributeType);
            return attribute != null;
        }
    }
}