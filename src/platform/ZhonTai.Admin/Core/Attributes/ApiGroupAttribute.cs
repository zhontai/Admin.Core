using System;

namespace ZhonTai.Admin.Core.Attributes;

/// <summary>
/// 接口分组
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public class ApiGroupAttribute : Attribute
{
    public bool NonGroup { get; set; }

    /// <summary>
    /// 分组名称列表
    /// </summary>
    public string[] GroupNames { get; set; }

    public ApiGroupAttribute(params string[] groupNames)
    {
        GroupNames = groupNames;
    }
}