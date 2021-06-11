using Admin.Core.Common.BaseModel;
using Admin.Core.Common.Helpers;
using Microsoft.AspNetCore.Http;
using System;

namespace Admin.Core.Common.Auth
{
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
        public string Name
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
        /// 昵称
        /// </summary>
        public string NickName
        {
            get
            {
                var name = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.UserNickName);

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
        /// 数据隔离
        /// </summary>
        public virtual DataIsolationType? DataIsolationType
        {
            get
            {
                var dataIsolationType = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.DataIsolationType);
                if (dataIsolationType != null && dataIsolationType.Value.NotNull())
                {
                    return (DataIsolationType)Enum.Parse(typeof(DataIsolationType), dataIsolationType.Value, true);
                }
                return null;
            }
        }
    }
}