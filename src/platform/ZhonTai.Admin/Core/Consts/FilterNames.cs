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
    /// 本人
    /// </summary>
    [Description("本人")]
    public const string Self = "Self";

    /// <summary>
    /// 部门
    /// </summary>
    [Description("部门")]
    public const string Org = "Org";
}