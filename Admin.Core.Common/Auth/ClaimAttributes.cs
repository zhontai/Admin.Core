namespace Admin.Core.Common.Auth
{
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
        /// 认证授权用户Id
        /// </summary>
        public const string IdentityServerUserId = "sub";

        /// <summary>
        /// 用户名
        /// </summary>
        public const string UserName = "na";

        /// <summary>
        /// 姓名
        /// </summary>
        public const string UserNickName = "nn";

        /// <summary>
        /// 刷新有效期
        /// </summary>
        public const string RefreshExpires = "re";

        /// <summary>
        /// 租户Id
        /// </summary>
        public const string TenantId = "tenantid";
    }
}
