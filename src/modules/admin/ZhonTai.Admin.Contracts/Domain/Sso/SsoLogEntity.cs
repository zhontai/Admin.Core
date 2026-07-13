using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;

namespace ZhonTai.Admin.Contracts.Domain.Sso;

/// <summary>
/// 单点登录审计日志
/// </summary>
[Table(Name = "ad_sso_log")]
public partial class SsoLogEntity : EntityAdd
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    [Column(StringLength = 50)]
    public string UserName { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public long? TenantId { get; set; }

    /// <summary>
    /// 应用Id
    /// </summary>
    [Column(StringLength = 64)]
    public string AppId { get; set; }

    /// <summary>
    /// 票据
    /// </summary>
    [Column(StringLength = 64)]
    public string Ticket { get; set; }

    /// <summary>
    /// 客户端IP
    /// </summary>
    [Column(StringLength = 50)]
    public string Ip { get; set; }

    /// <summary>
    /// 状态：success / fail
    /// </summary>
    [Column(StringLength = 20)]
    public string Status { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [Column(StringLength = 512)]
    public string Message { get; set; }
}
