using My.Admin.Common.Helpers;
using Microsoft.AspNetCore.Http;

namespace My.Admin.Common.Auth
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserIdentiyServer : User
    {
        private readonly IHttpContextAccessor _accessor;

        public UserIdentiyServer(IHttpContextAccessor accessor) : base(accessor)
        {
            _accessor = accessor;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public override long Id
        {
            get
            {
                var id = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.IdentityServerUserId);
                if (id != null && id.Value.NotNull())
                {
                    return id.Value.ToLong();
                }
                return 0;
            }
        }
    }
}