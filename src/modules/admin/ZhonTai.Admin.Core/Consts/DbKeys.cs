using System.ComponentModel;
using ZhonTai.Admin.Core.Configs;

namespace ZhonTai.Admin.Core.Consts;

/// <summary>
/// 数据库键名
/// </summary>
public class DbKeys
{
    /// <summary>
    /// 数据库注册键
    /// </summary>
    [Description("数据库注册键")]
    public static string AppDb { get; set; } =  AppInfo.GetOptions<DbConfig>()?.Key??"admindb";

    /// <summary>
    /// 任务调度数据库注册键
    /// </summary>
    [Description("任务调度数据库注册键")]
    public static string TaskDb { get; set; } = AppInfo.GetOptions<DbConfig>()?.Key??"admindb";
}
