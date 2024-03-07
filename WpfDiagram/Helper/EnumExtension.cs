using System;
using System.ComponentModel;
using System.Reflection;

namespace WpfDiagram.Helper
{
    public static class EnumExtension
    {
        /// <summary>
        /// Converts to enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

    }
}
