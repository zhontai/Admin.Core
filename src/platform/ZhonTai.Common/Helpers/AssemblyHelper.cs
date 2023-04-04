using System.Collections.Generic;
using System.Reflection;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// 程序集帮助类
/// </summary>
public class AssemblyHelper
{
    /// <summary>
    /// 根据程序集名称列表获取程序集列表
    /// </summary>
    /// <param name="assemblyNames"></param>
    /// <returns></returns>
    public static Assembly[] GetAssemblyList(string[] assemblyNames)
    {
        List<Assembly> assemblies = new();

        if (!(assemblyNames?.Length > 0))
        {
            return assemblies.ToArray();
        }

        foreach (var assemblyName in assemblyNames)
        {
            var assembly = Assembly.Load(assemblyName);
            if (assembly != null)
            {
                assemblies.Add(assembly);
            }
        }

        return assemblies.ToArray();
    }
}