using Microsoft.AspNetCore.Http;
using System;
using ZhonTai.Common.Extensions;
using ZhonTai.Admin.Domain.User;
using Microsoft.Extensions.DependencyInjection;
using ZhonTai.Admin.Domain.User.Dto;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Tools.Cache;
using ZhonTai.Admin.Domain.Tenant;

namespace ZhonTai.Admin.Core.Auth;

/// <summary>
/// 用户信息
/// </summary>
public class User : IUser
{
    private readonly IHttpContextAccessor _accessor;

    public User(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
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
            return cache.Get<DataPermissionDto>(CacheKeys.DataPermission + Id);
        }
    }

    /// <summary>
    /// 数据权限
    /// </summary>
    public virtual DataPermissionDto DataPermission => GetDataPermission();
}