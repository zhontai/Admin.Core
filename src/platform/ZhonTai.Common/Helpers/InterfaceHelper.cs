using System;
using System.Linq;
using System.Reflection;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// 接口帮助类
/// </summary>
public class InterfaceHelper
{
    public static string[] GetPropertyNames<T>() where T : class
    {
        Type interfaceType = typeof(T);

        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException($"{interfaceType.FullName} is not an interface type.");
        }

        PropertyInfo[] properties = interfaceType.GetProperties();

        string[] propertyNames = properties.Select(p => p.Name).ToArray();

        return propertyNames;
    }
}