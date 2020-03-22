
namespace Admin.Core.Common.Cache
{
    /// <summary>
    /// 缓存键
    /// </summary>
    public static class CacheKey
    {
        /// <summary>
        /// 验证码 admin:verify:code:guid
        /// </summary>
        public const string VerifyCodeKey = "admin:verify:code:{0}";

        /// <summary>
        /// 密码 admin:password:guid
        /// </summary>
        public const string PassWordKey = "admin:password:{0}";

        /// <summary>
        /// 用户权限 admin:user:用户主键:permissions
        /// </summary>
        public const string UserPermissions = "admin:user:{0}:permissions";
    }
}
