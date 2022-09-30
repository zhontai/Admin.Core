using System.ComponentModel;

namespace ZhonTai.Admin.Core.Consts;

/// <summary>
/// 数据库键名
/// </summary>
public static partial class DbKeys
{
    /// <summary>
    /// admin库
    /// </summary>
    [Description("admin库")]
    public const string AdminDbKey = "admindb";

    /// <summary>
    /// 多库
    /// </summary>
    [Description("多库")]
    public const string MultiDbKey = "multidb_";

    /// <summary>
    /// 租户独立数据库
    /// </summary>
    [Description("租户独立数据库")]
    public const string TenantDbKey = "tenantdb_";
}