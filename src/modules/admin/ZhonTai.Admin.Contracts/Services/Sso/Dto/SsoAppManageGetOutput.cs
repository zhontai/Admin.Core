namespace ZhonTai.Admin.Contracts.Services.Sso.Dto;

/// <summary>
/// 应用详情输出
/// </summary>
public class SsoAppManageGetOutput : SsoAppManageGetListOutput
{
    /// <summary>
    /// 应用密钥（仅详情返回，列表不返回）
    /// </summary>
    public string AppSecret { get; set; }
}
