namespace ZhonTai.Admin.Contracts.Services.Sso.Dto;

/// <summary>
/// 单点登录按钮应用输出（脱敏，供前端渲染按钮，不含密钥）
/// </summary>
public class SsoAppManageButtonOutput
{
    /// <summary>
    /// 应用Id
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    /// 图标（前端按钮展示用）
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 排序（升序）
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 回调地址（含 {ticket}/{appId} 占位符）
    /// </summary>
    public string CallbackUrl { get; set; }
}
