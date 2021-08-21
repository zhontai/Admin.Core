namespace Admin.Tools.Captcha
{
    public class CaptchaInput
    {
        /// <summary>
        /// 校验唯一标识
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }
    }
}
