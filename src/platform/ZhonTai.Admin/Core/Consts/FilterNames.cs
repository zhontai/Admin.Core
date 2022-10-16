using System.ComponentModel;

namespace ZhonTai.Admin.Core.Consts;

/// <summary>
/// 全局过滤器名
/// </summary>
public static partial class FilterNames
{
    /// <summary>
    /// 删除
    /// </summary>
    [Description("删除")]
    public const string Delete = "Delete";

    /// <summary>
    /// 租户
    /// </summary>
    [Description("租户")]
    public const string Tenant = "Tenant";

    /// <summary>
    /// 本人权限
    /// </summary>
    [Description("本人权限")]
    public const string Self = "Selft";

    /// <summary>
    /// 数据权限
    /// </summary>
    [Description("数据权限")]
    public const string Data = "Data";
}