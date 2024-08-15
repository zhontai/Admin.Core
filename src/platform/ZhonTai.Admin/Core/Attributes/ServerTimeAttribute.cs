using System;

namespace ZhonTai.Admin.Core.Attributes;

/// <summary>
/// 服务端时间
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ServerTimeAttribute : Attribute
{
    /// <summary>
    /// 一直更新，默认值false，指定为true一直更新该字段服务器端时间
    /// </summary>
    public bool AlwaysUpdate { get; set; } = false;

    public ServerTimeAttribute(bool alwaysUpdate = false)
    {
        AlwaysUpdate = alwaysUpdate;
    }
}