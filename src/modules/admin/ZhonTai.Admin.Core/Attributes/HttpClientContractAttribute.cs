using System;

namespace ZhonTai.Admin.Core.Attributes;

/// <summary>
/// Http接口客户端契约
/// </summary>
[AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
public sealed class HttpClientContractAttribute: Attribute
{
    /// <summary>
    /// 模块名
    /// </summary>
    public string ModuleName { get; set; }

    public HttpClientContractAttribute(string moduleName)
    {
        ModuleName = moduleName;
    }
}