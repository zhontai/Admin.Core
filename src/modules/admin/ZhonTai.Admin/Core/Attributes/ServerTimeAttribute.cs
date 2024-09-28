using System;

namespace ZhonTai.Admin.Core.Attributes;

/// <summary>
/// 服务端时间
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ServerTimeAttribute : Attribute
{
    /// <summary>
    /// 更新设置该字段服务器端时间，默认值false，指定为true更新时设置
    /// </summary>
    public bool CanUpdate { get; set; } = false;

    /// <summary>
    /// 插入设置该字段服务器端时间，默认值true，指定为false插入时不设置
    /// </summary>
    public bool CanInsert { get; set; } = true;
}