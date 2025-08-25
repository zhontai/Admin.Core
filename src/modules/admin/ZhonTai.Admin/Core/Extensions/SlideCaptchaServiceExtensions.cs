using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZhonTai.Admin.Core.Captcha;
using ZhonTai.Admin.Core.Consts;

namespace ZhonTai.Admin.Core.Extensions;

/// <summary>
/// 滑块验证码服务扩展
/// </summary>
public static class SlideCaptchaServiceExtensions
{
    /// <summary>
    /// 添加任务调度
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddSlideCaptcha(this IServiceCollection services)
    {
        var configuration = AppInfo.GetRequiredService<IConfiguration>(false);
        //滑块验证码
        services.AddSlideCaptcha(configuration, options =>
        {
            options.StoreageKeyPrefix = CacheKeys.Captcha;
        });
        services.AddScoped<ISlideCaptcha, SlideCaptcha>();
        return services;
    }
}
