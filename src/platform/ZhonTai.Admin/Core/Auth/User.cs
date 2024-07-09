using Microsoft.AspNetCore.Http;
using System;
using ZhonTai.Common.Extensions;
using ZhonTai.Admin.Domain.User;
using Microsoft.Extensions.DependencyInjection;
using ZhonTai.Admin.Domain.User.Dto;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Tools.Cache;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Services.User.Dto;
using System.Linq;
using ZhonTai.Admin.Resources;
using ZhonTai.Admin.Core.Exceptions;

namespace ZhonTai.Admin.Core.Auth;

/// <summary>
/// 用户信息
/// </summary>
public class User : IUser
{
    private readonly IHttpContextAccessor _accessor;
    private readonly AdminLocalizer _adminLocalizer;

    public User(IHttpContextAccessor accessor, AdminLocalizer adminLocalizer)
    {
        _accessor = accessor;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 用户Id
    /// </summary>
    public virtual long Id
    {
        get
        {
            var id = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.UserId);
            if (id != null && id.Value.NotNull())
            {
                return id.Value.ToLong();
            }
            return 0;
        }
    }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName
    {
        get
        {
            var name = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.UserName);

            if (name != null && name.Value.NotNull())
            {
                return name.Value;
            }

            return "";
        }
    }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name
    {
        get
        {
            var name = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.Name);

            if (name != null && name.Value.NotNull())
            {
                return name.Value;
            }

            return "";
        }
    }

    /// <summary>
    /// 租户Id
    /// </summary>
    public virtual long? TenantId
    {
        get
        {
            var tenantId = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.TenantId);
            if (tenantId != null && tenantId.Value.NotNull())
            {
                return tenantId.Value.ToLong();
            }
            return null;
        }
    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public virtual UserType Type
    {
        get
        {
            var userType = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.UserType);
            if (userType != null && userType.Value.NotNull())
            {
                return (UserType)Enum.Parse(typeof(UserType), userType.Value, true);
            }
            return UserType.DefaultUser;
        }
    }

    /// <summary>
    /// 默认用户
    /// </summary>
    public virtual bool DefaultUser
    {
        get
        {
            return Type == UserType.DefaultUser;
        }
    }


    /// <summary>
    /// 平台管理员
    /// </summary>
    public virtual bool PlatformAdmin
    {
        get
        {
            return Type == UserType.PlatformAdmin;
        }
    }

    /// <summary>
    /// 租户管理员
    /// </summary>
    public virtual bool TenantAdmin
    {
        get
        {
            return Type == UserType.TenantAdmin;
        }
    }

    /// <summary>
    /// 租户类型
    /// </summary>
    public virtual TenantType? TenantType
    {
        get
        {
            var tenantType = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.TenantType);
            if (tenantType != null && tenantType.Value.NotNull())
            {
                return (TenantType)Enum.Parse(typeof(TenantType), tenantType.Value, true);
            }
            return null;
        }
    }

    /// <summary>
    /// 数据库注册键
    /// </summary>
    public virtual string DbKey
    {
        get
        {
            var dbKey = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.DbKey);
            if (dbKey != null && dbKey.Value.NotNull())
            {
                return dbKey.Value;
            }
            return "";
        }
    }

    /// <summary>
    /// 获得数据权限
    /// </summary>
    /// <returns></returns>
    DataPermissionDto GetDataPermission()
    {
        var cache = _accessor?.HttpContext?.RequestServices.GetRequiredService<ICacheTool>();
        if (cache == null)
        {
            return null;
        }
        else
        {
            return cache.Get<DataPermissionDto>(CacheKeys.GetDataPermissionKey(Id));
        }
    }

    /// <summary>
    /// 数据权限
    /// </summary>
    public virtual DataPermissionDto DataPermission => GetDataPermission();

    /// <summary>
    /// 获得用户权限
    /// </summary>
    /// <returns></returns>
    UserGetPermissionOutput GetUserPermission()
    {
        var cache = _accessor?.HttpContext?.RequestServices.GetRequiredService<ICacheTool>();
        if (cache == null)
        {
            return null;
        }
        else
        {
            return cache.Get<UserGetPermissionOutput>(CacheKeys.GetUserPermissionKey(Id));
        }
    }

    /// <summary>
    /// 用户权限
    /// </summary>
    public virtual UserGetPermissionOutput UserPermission => GetUserPermission();

    /// <summary>
    /// 检查用户是否拥有某个权限点
    /// </summary>
    /// <param name="permissionCode">权限点编码</param>
    /// <returns></returns>
    public virtual bool HasPermission(string permissionCode)
    {
        if (permissionCode.IsNull())
        {
            throw new AppException(_adminLocalizer["权限点编码不能为空"]);
        }

        return HasPermissions([permissionCode]);
    }

    /// <summary>
    /// 检查用户是否拥有这些权限点
    /// </summary>
    /// <param name="permissionCodes">权限点编码列表</param>
    /// <param name="all">是否全部满足</param>
    /// <returns></returns>
    public virtual bool HasPermissions(string[] permissionCodes, bool all = false)
    {
        if (!(permissionCodes?.Length > 0))
        {
            throw new AppException(_adminLocalizer["权限点编码列表不能为空"]);
        }

        if (PlatformAdmin)
        {
            return true;
        }

        var valid = false;
        if (all)
        {
            valid = UserPermission.Codes.All(a => permissionCodes.Contains(a));
        }
        else
        {
            valid = UserPermission.Codes.Any(a => permissionCodes.Contains(a));
        }

        return valid;
    }
}