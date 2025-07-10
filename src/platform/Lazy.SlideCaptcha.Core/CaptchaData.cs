namespace Lazy.SlideCaptcha.Core
{
    public class CaptchaData
    {
        public CaptchaData(string id, string backgroundImage, string sliderImage)
        {
            Id = id;
            BackgroundImage = backgroundImage;
            SliderImage = sliderImage;
        }

        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 背景图(含凹槽)
        /// </summary>
        public string BackgroundImage { get; set; }
        /// <summary>
        /// 滑动块图
        /// </summary>
        public string SliderImage { get; set; }
    }
}