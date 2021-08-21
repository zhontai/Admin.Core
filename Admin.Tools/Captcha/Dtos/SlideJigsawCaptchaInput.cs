namespace Admin.Tools.Captcha
{
    public class SlideJigsawCaptchaInput
    {
        /// <summary>
        /// 坐标数据
        /// </summary>
        public string Point { get; set; }

        /// <summary>
        /// 校验唯一标识
        /// </summary>
        public string Token { get; set; }
    }
}
