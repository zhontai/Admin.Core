using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;

namespace ZhonTai.Admin.Contracts.Domain.Sso;

/// <summary>
/// 单点登录第三方应用配置
/// </summary>
[Table(Name = "ad_sso_app")]
[Index("idx_{tablename}_01", nameof(AppId), true)]
public partial class SsoAppManageEntity : EntityBase
{
    /// <summary>
    /// 应用Id
    /// </summary>
    [Column(StringLength = 64)]
    public string AppId { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    [Column(StringLength = 128)]
    public string AppName { get; set; }

    /// <summary>
    /// 应用密钥（服务端对服务端签名校验使用）
    /// </summary>
    [Column(StringLength = 128)]
    public string AppSecret { get; set; }

    /// <summary>
    /// 回调地址（配置时建议包含占位符 ?ticket={ticket}&amp;app_id={appId}，由前端替换为实际值）
    /// </summary>
    [Column(StringLength = 512)]
    public string CallbackUrl { get; set; }

    /// <summary>
    /// 排序（升序，越小越靠前）
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 图标（前端按钮展示用，如 el-icon 名称，可空）
    /// </summary>
    [Column(StringLength = 64)]
    public string Icon { get; set; }

    /// <summary>
    /// 状态：1启用 0禁用
    /// </summary>
    public int Status { get; set; } = 1;
}
