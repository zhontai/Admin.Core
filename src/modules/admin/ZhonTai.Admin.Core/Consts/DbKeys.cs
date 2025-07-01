using System.ComponentModel;
using ZhonTai.Admin.Core.Configs;

namespace ZhonTai.Admin.Core.Consts;

/// <summary>
/// 数据库键名
/// </summary>
public class DbKeys
{
    private static readonly string _defaultDbKey = AppInfo.GetOptions<DbConfig>()?.Key ?? "admindb";

    static DbKeys()
    {
        if (string.IsNullOrWhiteSpace(_defaultDbKey))
        {
            throw new InvalidOperationException("数据库配置键不能为空");
        }
    }

    /// <summary>
    /// 应用数据库注册键
    /// </summary>
    [Description("应用数据库注册键")]
    [Obsolete("请使用 AdminDb 代替")]
    public static string AppDb { get; set; } = _defaultDbKey;

    /// <summary>
    /// 权限数据库注册键
    /// </summary>
    [Description("权限数据库注册键")]
    public static string AdminDb { get; set; } = _defaultDbKey;

    /// <summary>
    /// 任务调度数据库注册键
    /// </summary>
    [Description("任务调度数据库注册键")]
    public static string TaskDb { get; set; } = _defaultDbKey;

    /// <summary>
    /// 日志数据库注册键
    /// </summary>
    [Description("日志数据库注册键")]
    public static string LogDb { get; set; } = _defaultDbKey;
}