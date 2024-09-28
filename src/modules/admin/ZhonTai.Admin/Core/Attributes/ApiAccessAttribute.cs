using System;

namespace ZhonTai.Admin.Core.Attributes;

/// <summary>
/// 接口访问
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ApiAccessAttribute : Attribute
{
    /// <summary>
    /// 默认 false， 满足任意一个可访问。若设置 true 全部满足可访问
    /// </summary>
    public bool All { get; set; } = false;

    /// <summary>
    /// 权限点
    /// </summary>
    public string[] Codes { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="codes">权限点</param>
    public ApiAccessAttribute(params string[] codes)
    {
        Codes = codes;
    }
}
