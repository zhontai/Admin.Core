namespace ZhonTai.Admin.Services.Api.Dto;

/// <summary>
/// 设置启用请求参数
/// </summary>
public class ApiSetEnableParamsInput
{
    /// <summary>
    /// 接口Id
    /// </summary>
    public long ApiId { get; set; }

    /// <summary>
    /// 是否启用请求参数
    /// </summary>
    public bool EnabledParams { get; set; }
}