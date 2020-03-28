using Microsoft.AspNetCore.Http;

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
        public long Id
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
        /// 姓名
        /// </summary>
        public string RealName
        {
            get
            {
                var name = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.UserRealName);

                if (name != null && name.Value.NotNull())
                {
                    return name.Value;
                }

                return "";
            }
        }

        /// <summary>
        /// 登录日志Id
        /// </summary>
        public long LoginLogId
        {
            get
            {
                var id = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.UserLoginLogId);
                if (id != null && id.Value.NotNull())
                {
                    return id.Value.ToLong();
                }
                return 0;
            }
        }

        /// <summary>
        /// 用户IP
        /// </summary>
        public string IP
        {
            get
            {
                if (_accessor?.HttpContext?.Connection == null)
                    return "";

                return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }

        /// <summary>
        /// 用户IPv4
        /// </summary>
        public string IPv4
        {
            get
            {
                if (_accessor?.HttpContext?.Connection == null)
                    return "";

                return _accessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }

        /// <summary>
        /// 用户IPv6
        /// </summary>
        public string IPv6
        {
            get
            {
                if (_accessor?.HttpContext?.Connection == null)
                    return "";

                return _accessor.HttpContext.Connection.RemoteIpAddress.MapToIPv6().ToString();
            }
        }
    }

    /// <summary>
    /// Claim属性
    /// </summary>
    public static class ClaimAttributes
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public const string UserId = "id";

        /// <summary>
        /// 用户名
        /// </summary>
        public const string UserName = "na";

        /// <summary>
        /// 姓名
        /// </summary>
        public const string UserRealName = "rna";

        /// <summary>
        /// 登录日志Id
        /// </summary>
        public const string UserLoginLogId = "llid";
    }
}
