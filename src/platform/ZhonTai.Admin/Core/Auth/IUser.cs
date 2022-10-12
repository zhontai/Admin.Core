
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Admin.Domain.User;

namespace ZhonTai.Admin.Core.Auth;

/// <summary>
/// 用户信息接口
/// </summary>
public interface IUser
{
    /// <summary>
    /// 用户Id
    /// </summary>
    long Id { get; }

    /// <summary>
    /// 用户名
    /// </summary>
    string UserName { get; }

    /// <summary>
    /// 姓名
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 用户类型
    /// </summary>
    UserType Type { get; }

    /// <summary>
    /// 默认用户
    /// </summary>
    bool DefaultUser { get; }

    /// <summary>
    /// 平台管理员
    /// </summary>
    bool PlatformAdmin { get; }

    /// <summary>
    /// 租户管理员
    /// </summary>
    bool TenantAdmin { get; }

    /// <summary>
    /// 租户Id
    /// </summary>
    long? TenantId { get; }

    /// <summary>
    /// 部门Id
    /// </summary>
    long? OrgId { get; }

    /// <summary>
    /// 租户类型
    /// </summary>
    TenantType? TenantType { get; }

    /// <summary>
    /// 数据库注册键
    /// </summary>
    string DbKey { get; }
}