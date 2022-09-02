using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;

namespace ZhonTai.Admin.Domain;

/// <summary>
/// 日志
/// </summary>
public abstract class LogAbstract : EntityAdd, ITenant
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [Column(Position = 2, CanUpdate = false)]
    public long? TenantId { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Column(StringLength = 60)]
    public string Name { get; set; }

    /// <summary>
    /// IP
    /// </summary>
    [Column(StringLength = 100)]
    public string IP { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    [Column(StringLength = 100)]
    public string Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [Column(StringLength = 100)]
    public string Os { get; set; }

    /// <summary>
    /// 设备
    /// </summary>
    [Column(StringLength = 50)]
    public string Device { get; set; }

    /// <summary>
    /// 浏览器信息
    /// </summary>
    [Column(StringLength = -1)]
    public string BrowserInfo { get; set; }

    /// <summary>
    /// 耗时（毫秒）
    /// </summary>
    public long ElapsedMilliseconds { get; set; }

    /// <summary>
    /// 操作状态
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 操作消息
    /// </summary>
    [Column(StringLength = 500)]
    public string Msg { get; set; }

    /// <summary>
    /// 操作结果
    /// </summary>
    [Column(StringLength = -1)]
    public string Result { get; set; }
}