using System.Reflection;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// 程序集帮助类
/// </summary>
public class AssemblyHelper
{
    /// <summary>
    /// 根据程序集名称判断程序集是否存在
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    public static bool Exists(string assemblyName)
    {
        try
        {
            Assembly.Load(assemblyName);
            return true;
        }
        catch (FileNotFoundException)
        {
            return false;
        }
    }

    /// <summary>
    /// 根据程序集名称列表获取程序集
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    public static Assembly GetAssembly(string assemblyName)
    {
        Assembly assembly = null;

        if (assemblyName.IsNull())
        {
            return assembly;
        }

        try
        {
            assembly = Assembly.Load(assemblyName);
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("程序集不存在：" + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("加载程序集时出错：" + ex.Message);
        }

        return assembly;
    }

    /// <summary>
    /// 根据程序集名称列表获取程序集列表
    /// </summary>
    /// <param name="assemblyNames"></param>
    /// <returns></returns>
    public static Assembly[] GetAssemblyList(string[] assemblyNames)
    {
        List<Assembly> assemblies = [];

        if (!(assemblyNames?.Length > 0))
        {
            return [.. assemblies];
        }

        foreach (var assemblyName in assemblyNames)
        {
            var assembly = GetAssembly(assemblyName);
            if (assembly != null)
            {
                assemblies.Add(assembly);
            }
        }

        return [.. assemblies];
    }
}