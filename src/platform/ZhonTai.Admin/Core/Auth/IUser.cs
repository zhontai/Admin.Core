using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.User.Dto;
using ZhonTai.Admin.Services.User.Dto;

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
    /// 租户类型
    /// </summary>
    TenantType? TenantType { get; }

    /// <summary>
    /// 数据库注册键
    /// </summary>
    string DbKey { get; }

    /// <summary>
    /// 数据权限
    /// </summary>
    DataPermissionDto DataPermission { get; }

    /// <summary>
    /// 用户权限
    /// </summary>
    UserGetPermissionOutput UserPermission { get; }

    /// <summary>
    /// 检查用户是否拥有某个权限点
    /// </summary>
    /// <param name="permissionCode">权限点编码</param>
    /// <returns></returns>
    bool HasPermission(string permissionCode);

    /// <summary>
    /// 检查用户是否拥有这些权限点
    /// </summary>
    /// <param name="permissionCodes">权限点编码列表</param>
    /// <param name="all">是否全部满足</param>
    /// <returns></returns>
    bool HasPermissions(string[] permissionCodes, bool all = false);
}