using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PersianCharFix.Utility
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this byte value)
        {
            return (from T e in Enum.GetValues(typeof(T)) where (e as Enum).ToByte() == value select e).SingleOrDefault();
        }

        public static T ToEnum<T>(this int value)
        {
            return (from T e in Enum.GetValues(typeof(T)) where (e as Enum).ToInt() == value select e).SingleOrDefault();
        }

        public static string ToDescription(this Enum value)
        {
            var attributes =
                (DescriptionAttribute[])
                    value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static T GetValueFromDescription<T>(this Enum value, string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", nameof(description));
            // or return default(T);
        }

        public static string ToDisplay(this Enum value, DisplayProperty property = DisplayProperty.Name)
        {
            if (value == null)
                return null;
            var attributes =
                (DisplayAttribute[])
                    value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes.Length > 0)
            {
                var attr = attributes[0];
                var propValue = attr.GetType().GetProperty(property.ToString()).GetValue(attr, null);
                return propValue as string;
            }
            return value.ToString();
        }

        public static string ToGetAttribute<T>(this Enum value) where T : Attribute, IAttribute
        {
            var attributes =
                (T[])
                    value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? attributes[0].Name : value.ToString();
        }

        public static Dictionary<int?, string> ToDictionaryNullable<T>(this T value) where T : struct
        {
            if (!typeof(T).IsEnum) throw new NotSupportedException();
            var dic = new Dictionary<int?, string> { [-1] = "انتخاب" };
            return dic.Concat(Enum.GetValues(value.GetType()).Cast<T>().Select(p => p as Enum).ToDictionary(p => p.ToNullableInt(), ToDescription)).ToDictionary(p => p.Key, p => p.Value);
        }

        public static Dictionary<int, string> ToDictionary<T>(this T value) where T : struct
        {
            if (!typeof(T).IsEnum) throw new NotSupportedException();
            return Enum.GetValues(value.GetType()).Cast<T>().Select(p => p as Enum).ToDictionary(p => p.ToInt(), ToDescription);
        }

        public static Dictionary<int, string> ToDictionaryGetDisplay<T>(this T value) where T : struct
        {
            if (!typeof(T).IsEnum) throw new NotSupportedException();
            return Enum.GetValues(value.GetType()).Cast<T>().Select(p => p as Enum).ToDictionary(p => p.ToInt(), p => p.ToDisplay());
        }
    }

    public interface IAttribute
    {
        string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class CategoryAttribute : Attribute, IAttribute
    {
        public CategoryAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class SystemNameAttribute : Attribute, IAttribute
    {
        public SystemNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    public enum DisplayProperty
    {
        Description,
        GroupName,
        Name,
        Prompt,
        ShortName,
        Order
    }
}
