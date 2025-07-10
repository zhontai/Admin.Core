using Lazy.SlideCaptcha.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core.Generator
{
    public class CaptchaImageData
    {
        /// <summary>
        /// 凹槽x坐标
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// 凹槽y坐标
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// 背景图宽
        /// </summary>
        public int BackgroundImageWidth { get; set; }
        /// <summary>
        /// 背景图高
        /// </summary>
        public int BackgroundImageHeight { get; set; }
        /// <summary>
        /// 背景图
        /// </summary>
        public string BackgroundImageBase64 { get; set; }
        /// <summary>
        /// 滑动块图宽
        /// </summary>
        public int SliderImageWidth { get; set; }
        /// <summary>
        /// 滑动块图高
        /// </summary>
        public int SliderImageHeight { get; set; }
        /// <summary>
        /// 滑动块图
        /// </summary>
        public string SliderImageBase64 { get; set; }

        /// <summary>
        /// 凹槽位置百分比
        /// </summary>
        public float Percent
        {
            get
            {
                if (BackgroundImageWidth <= 0) return 0;
                return 1.0f * X / BackgroundImageWidth;
            }
        }

        public void Check()
        {
            // 校验
            if (this.X <= 0) throw new SlideCaptchaException($"CaptchaImageData数据异常: {nameof(X)}小于等于0");
            if (this.Y <= 0) throw new SlideCaptchaException($"CaptchaImageData数据异常: {nameof(Y)}小于等于0");
            if (this.BackgroundImageWidth <= 0) throw new SlideCaptchaException($"CaptchaImageData数据异常: {nameof(BackgroundImageWidth)}小于等于0");
            if (this.BackgroundImageHeight <= 0) throw new SlideCaptchaException($"CaptchaImageData数据异常: {nameof(BackgroundImageHeight)}小于等于0");
            if (this.SliderImageWidth <= 0) throw new SlideCaptchaException($"CaptchaImageData数据异常: {nameof(SliderImageWidth)}小于等于0");
            if (this.SliderImageHeight <= 0) throw new SlideCaptchaException($"CaptchaImageData数据异常: {nameof(SliderImageHeight)}小于等于0");
            if(string.IsNullOrWhiteSpace(this.BackgroundImageBase64)) throw new SlideCaptchaException($"CaptchaImageData数据异常: {nameof(BackgroundImageBase64)}为空");
            if (string.IsNullOrWhiteSpace(this.SliderImageBase64)) throw new SlideCaptchaException($"CaptchaImageData数据异常: {nameof(SliderImageBase64)}为空");
        }
    }
}
