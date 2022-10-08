using ZhonTai.Admin.Core.Entities;
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
    /// 租户类型
    /// </summary>
    public TenantType? TenantType { get; set; }

    /// <summary>
    /// 数据隔离
    /// </summary>
    public DataIsolationType? DataIsolationType { get; set; }
}