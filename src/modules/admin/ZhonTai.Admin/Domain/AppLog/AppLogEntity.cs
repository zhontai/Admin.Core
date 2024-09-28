using FreeSql.DataAnnotations;
using System;

namespace ZhonTai.Admin.Domain.AppLog;

/// <summary>
/// 应用程序日志
/// </summary>
[Table(Name = "ad_app_log")]
public partial class AppLogEntity
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Column(Position = 1, IsPrimary = true, IsNullable = false)]
    public long Id { get; set; }

    public DateTime Logged { get; set; }

    [Column(StringLength = 5)]
    public string Level { get; set; }

    [Column(StringLength = -1)]
    public string Message { get; set; }

    [Column(StringLength = 300)]
    public string Logger { get; set; }

    [Column(StringLength = -1)]
    public string Properties { get; set; }

    [Column(StringLength = 300)]
    public string Callsite { get; set; }

    [Column(StringLength = -1)]
    public string Exception { get; set; }
}
