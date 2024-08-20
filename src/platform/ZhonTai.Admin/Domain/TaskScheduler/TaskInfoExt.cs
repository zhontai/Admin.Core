using FreeSql.DataAnnotations;
using System;

namespace ZhonTai.Admin.Domain;

/// <summary>
/// 任务邮件
/// </summary>
public class TaskInfoExt
{
    /// <summary>
    /// 任务Id
    /// </summary>
    public string TaskId { get; set; }

    /// <summary>
    /// 报警邮件，多个邮件地址则逗号分隔
    /// </summary>
    [Column(StringLength = 500)]
    public string AlarmEmail { get; set; }

    /// <summary>
    /// 失败重试次数
    /// </summary>
    public int? FailRetryCount { get; set; }

    /// <summary>
    /// 失败重试间隔（秒）
    /// </summary>
    public int? FailRetryInterval { get; set; }

    /// <summary>
    /// 添加时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 添加用户Id
    /// </summary>
    public long? CreatedUserId { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? ModifiedTime { get; set; }

    /// <summary>
    /// 修改用户Id
    /// </summary>
    public long? ModifiedUserId { get; set; }
}