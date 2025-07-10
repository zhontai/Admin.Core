using Lazy.SlideCaptcha.Core.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core
{
    public class CaptchaOptions
    {
        /// <summary>
        /// 过期时长
        /// </summary>
        public int ExpirySeconds { get; set; } = 60;

        /// <summary>
        /// 存储键前缀
        /// </summary>
        public string StoreageKeyPrefix { get; set; } = "slide-captcha";

        /// <summary>
        /// 容错值(校验时用，缺口位置与实际滑动位置匹配容错范围)
        /// </summary>
        public float Tolerant { get; set; } = 0.02f;

        /// <summary>
        /// 干扰块数量
        /// </summary>
        public int InterferenceCount { get; set; } = 0;

        /// <summary>
        /// 背景图
        /// </summary>
        public List<Resource> Backgrounds { get; set; } = new List<Resource>();

        /// <summary>
        /// 模板图(必须是slider,hole的顺序依次出现)
        /// </summary>
        public List<TemplatePair> Templates { get; set; } = new List<TemplatePair>();
    }
}
