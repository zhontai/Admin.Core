using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
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
        var langConfig = app.ApplicationServices.GetService<AppConfig>().Lang;

        //多语言
        string[] cultures = langConfig!.Langs?.Length > 0 ? langConfig.Langs : ["zh"];
        var options = new RequestLocalizationOptions()
            .AddSupportedCultures(cultures)
            .AddSupportedUICultures(cultures)
            .SetDefaultCulture(langConfig!.DefaultLang ?? cultures[0]);

        var providers = langConfig.RequestCultureProviders;
        var requestCultureProviders = new List<IRequestCultureProvider>();
        if(providers!=null && providers.Any())
        {
            if (providers.Where(a => a == "QueryString").Any())
                requestCultureProviders.Add(options.RequestCultureProviders[0]);
            if (providers.Where(a => a == "Cookie").Any())
                requestCultureProviders.Add(options.RequestCultureProviders[1]);
            if (providers.Where(a => a == "AcceptLanguageHeader").Any())
                requestCultureProviders.Add(options.RequestCultureProviders[2]);
        }
        options.RequestCultureProviders = requestCultureProviders;

        app.UseRequestLocalization(options);
       
        return app;
    }
}
