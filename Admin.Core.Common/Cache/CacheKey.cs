
using System.ComponentModel;

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
        [Description("验证码")]
        public const string VerifyCodeKey = "admin:verify:code:{0}";

        /// <summary>
        /// 密码加密 admin:password:encrypt:guid
        /// </summary>
        [Description("密码加密")]
        public const string PassWordEncryptKey = "admin:password:encrypt:{0}";

        /// <summary>
        /// 用户权限 admin:user:用户主键:permissions
        /// </summary>
        [Description("用户权限")]
        public const string UserPermissions = "admin:user:{0}:permissions";
    }
}
