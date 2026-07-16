using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Contracts.Services.Sso.Dto;

/// <summary>
/// 新增应用输入（密钥由系统自动生成）
/// </summary>
public class SsoAppManageAddInput
{
    /// <summary>
    /// 应用Id
    /// </summary>
    [Required(ErrorMessage = "应用Id不能为空")]
    public string AppId { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    /// 回调地址
    /// </summary>
    public string CallbackUrl { get; set; }

    /// <summary>
    /// 状态：1启用 0禁用
    /// </summary>
    public int Status { get; set; } = 1;
}
