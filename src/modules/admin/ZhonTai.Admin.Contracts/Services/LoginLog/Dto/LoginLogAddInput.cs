namespace ZhonTai.Admin.Services.LoginLog.Dto;

/// <summary>
/// 添加
/// </summary>
public class LoginLogAddInput
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public long? TenantId { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// IP
    /// </summary>
    public string IP { get; set; }

    /// <summary>
    /// 国家
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// 省份
    /// </summary>
    public string Province { get; set; }

    /// <summary>
    /// 城市
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// 网络服务商
    /// </summary>
    public string Isp { get; init; }

    /// <summary>
    /// 耗时（毫秒）
    /// </summary>
    public long ElapsedMilliseconds { get; set; }

    /// <summary>
    /// 操作状态
    /// </summary>
    public bool? Status { get; set; }

    /// <summary>
    /// 操作消息
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// 创建者用户Id
    /// </summary>
    public long? CreatedUserId { get; set; }

    /// <summary>
    /// 创建者用户名
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建者姓名
    /// </summary>
    public string CreatedUserRealName { get; set; }
}