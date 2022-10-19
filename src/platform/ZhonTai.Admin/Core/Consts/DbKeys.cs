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
    public static string MasterDb { get; set; } = "masterdb";

    /// <summary>
    /// admin库
    /// </summary>
    [Description("admin库")]
    public static string AdminDb { get; set; } = "admindb";
}