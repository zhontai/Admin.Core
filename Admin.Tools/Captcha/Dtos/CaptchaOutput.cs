namespace Admin.Tools.Captcha
{
    /// <summary>
    /// 验证数据
    /// </summary>
    public class CaptchaOutput
    {
        /// <summary>
        /// 校验唯一标识
        /// </summary>
        public string Token { get; set; }

        public object Data { get; set; }
    }
}
