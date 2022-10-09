using System.ComponentModel;

namespace ZhonTai.Admin.Core.Consts;

/// <summary>
/// 数据库键名
/// </summary>
public static partial class DbKeys
{
    /// <summary>
    /// 主库
    /// </summary>
    [Description("主库")]
    public const string MasterDbKey = "masterdb";

    /// <summary>
    /// 多库
    /// </summary>
    [Description("多库")]
    public const string MultiDbKey = "multidb_";

    /// <summary>
    /// 租户数据库
    /// </summary>
    [Description("租户数据库")]
    public const string TenantDbKey = "tenantdb_";
}