using Admin.Core.Common.Output;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Admin.Core.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum item)
        {
            string name = item.ToString();
            var desc = item.GetType().GetField(name)?.GetCustomAttribute<DescriptionAttribute>();
            return desc?.Description ?? name;
        }

        public static long ToInt64(this Enum item)
        {
            return Convert.ToInt64(item);
        }

        public static List<OptionOutput> ToList(this Enum value, bool ignoreNull = false)
        {
            var enumType = value.GetType();

            if (!enumType.IsEnum)
                return null;

            return Enum.GetValues(enumType).Cast<Enum>()
                .Where(m => !ignoreNull || !m.ToString().Equals("Null")).Select(x => new OptionOutput
                {
                    Label = x.ToDescription(),
                    Value = x
                }).ToList();
        }

        public static List<OptionOutput> ToList<T>(bool ignoreNull = false)
        {
            var enumType = typeof(T);

            if (!enumType.IsEnum)
                return null;

            return Enum.GetValues(enumType).Cast<Enum>()
                 .Where(m => !ignoreNull || !m.ToString().Equals("Null")).Select(x => new OptionOutput
                 {
                     Label = x.ToDescription(),
                     Value = x
                 }).ToList();
        }
    }
}