namespace ZhonTai.Admin.Services.Auth.Dto;

/// <summary>
/// 验证码
/// </summary>
public class AuthGetVerifyCodeOutput
{
    /// <summary>
    /// 缓存键
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 图片
    /// </summary>
    public string Img { get; set; }
}