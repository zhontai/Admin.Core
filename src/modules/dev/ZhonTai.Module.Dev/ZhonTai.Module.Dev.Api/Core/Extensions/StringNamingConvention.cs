using System.Linq;
using System.Text.RegularExpressions;
using ZhonTai.DynamicApi.Enums;

/// <summary>
/// 命名规范扩展
/// </summary>
public static class NamingConvention
{
    static string _Sp(string name, string separator)
    {
        return Regex.Replace(
        name,
        "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
        $"{separator}$1",
        RegexOptions.Compiled)
        .Trim();
    }

    /// <summary>
    /// camelCase
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string NamingCamelCase(this string name)
    {
        if (name.Length < 2) return name.ToLower();
        var str = NamingPascalCase(name);
        return char.ToLower(str[0]) + str.Substring(1);
    }

    /// <summary>
    /// PascalCase
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string NamingPascalCase(this string name)
    {
        if (name.Length < 2) return name.ToUpper();
        var str = name.Trim().Replace("-.|", '_');
        return string.Concat(str.Split('_').Select(s => char.ToUpper(s[0]) + s.Substring(1)));
    }
    /// <summary>
    /// kebab-case
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string NamingKebabCase(this string name)
    {
        return _Sp(NamingPascalCase(name), "-").ToLower();
    }
    /// <summary>
    /// snake_case
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string NamingSnakeCase(this string name)
    {
        return _Sp(NamingPascalCase(name), "_").ToLower();
    }

    /// <summary>
    /// Extension.Case
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string NamingExtensionCase(this string name)
    {
        return _Sp(NamingPascalCase(name), ".");
    }

    /// <summary>
    /// 自定义分隔符命名规范
    /// </summary>
    /// <param name="name"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string NamingCustom(this string name, string separator)
    {
        return _Sp(NamingPascalCase(name), separator);
    }

    /// <summary>
    /// 命名规范转换
    /// </summary>
    /// <param name="name"></param>
    /// <param name="convention"></param>
    /// <returns></returns>
    public static string Naming(this string name, NamingConventionEnum convention)
    {
        return convention switch
        {
            NamingConventionEnum.CamelCase => NamingCamelCase(name),
            NamingConventionEnum.PascalCase => NamingPascalCase(name),
            NamingConventionEnum.SnakeCase => NamingSnakeCase(name),
            NamingConventionEnum.KebabCase => NamingKebabCase(name),
            NamingConventionEnum.ExtensionCase => NamingExtensionCase(name),
            _ => name
        };
    }
}
