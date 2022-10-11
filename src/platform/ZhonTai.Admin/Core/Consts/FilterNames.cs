using System.ComponentModel;

namespace ZhonTai.Admin.Core.Consts;

/// <summary>
/// 全局过滤器名
/// </summary>
public static partial class FilterNames
{
    /// <summary>
    /// 软删除
    /// </summary>
    [Description("软删除")]
    public const string SoftDelete = "SoftDelete";

    /// <summary>
    /// 租户
    /// </summary>
    [Description("租户")]
    public const string Tenant = "Tenant";
}