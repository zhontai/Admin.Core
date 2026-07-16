namespace ZhonTai.Admin.Contracts.Services.Sso.Dto;

/// <summary>
/// 应用列表输出
/// </summary>
public class SsoAppManageGetListOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 应用Id
    /// </summary>
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
    public int Status { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }
}
