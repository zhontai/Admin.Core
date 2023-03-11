using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.User;

namespace ZhonTai.Admin.Services.Auth.Dto;

public class AuthLoginOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public UserType Type { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public long? TenantId { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    public long? OrgId { get; set; }

    /// <summary>
    /// 租户信息
    /// </summary>
    public AuthLoginTenantDto Tenant { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }
}

/// <summary>
/// 租户信息
/// </summary>
public class AuthLoginTenantDto
{
    /// <summary>
    /// 租户类型
    /// </summary>
    public TenantType? TenantType { get; set; }

    /// <summary>
    /// 数据库注册键
    /// </summary>
    public string DbKey { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }
}