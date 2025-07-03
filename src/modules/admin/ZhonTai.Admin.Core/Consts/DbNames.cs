using System.ComponentModel;

namespace ZhonTai.Admin.Core.Consts;

/// <summary>
/// 数据库名
/// </summary>
public static partial class DbNames
{
    /// <summary>
    /// 权限库
    /// </summary>
    [Description("权限库")]
    public const string Admin = "Admin";

    /// <summary>
    /// 日志库
    /// </summary>
    [Description("日志库")]
    public const string Log = "Log";

    /// <summary>
    /// 任务库
    /// </summary>
    [Description("任务库")]
    public const string Task = "Task";
}