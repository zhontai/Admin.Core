namespace ZhonTai.Admin.Services.Auth.Dto;

public class AuthGetPasswordEncryptKeyOutput
{
    /// <summary>
    /// 缓存键
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 密码加密密钥
    /// </summary>
    public string EncryptKey { get; set; }

    /// <summary>
    /// 密码加密向量
    /// </summary>
    public string Iv { get;  set; }
}