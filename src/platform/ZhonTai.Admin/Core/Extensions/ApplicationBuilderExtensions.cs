using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ZhonTai.Admin.Core.Configs;

namespace ZhonTai.Admin.Core.Extensions;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// 使用多语言
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseMyLocalization(this IApplicationBuilder app)
    {
        var appConfig = app.ApplicationServices.GetService<AppConfig>();

        //多语言
        string[] cultures = [appConfig!.Lang ?? "zh"];
        var options = new RequestLocalizationOptions()
            .AddSupportedCultures(cultures)
            .AddSupportedUICultures(cultures)
            .SetDefaultCulture(appConfig!.Lang ?? cultures[0]);
        
        //只保留从请求头 Accept-Language 解析
        var acceptLanguageHeaderRequestCultureProvider = options.RequestCultureProviders[2];
        options.RequestCultureProviders = [acceptLanguageHeaderRequestCultureProvider];

        app.UseRequestLocalization(options);
       
        return app;
    }
}
