namespace Admin.Core.Service.Admin.Auth.Input
{
    /// <summary>
    /// 登录信息
    /// </summary>
    public class AuthLoginInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 密码键
        /// </summary>
        public string PasswordKey { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }

        /// <summary>
        /// 验证码键
        /// </summary>
        public string VerifyCodeKey { get; set; }
    }
}
