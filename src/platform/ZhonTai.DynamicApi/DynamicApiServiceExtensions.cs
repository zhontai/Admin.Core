using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ZhonTai.DynamicApi.Helpers;

namespace ZhonTai.DynamicApi;

/// <summary>
/// Add Dynamic WebApi
/// </summary>
public static class DynamicApiServiceExtensions
{
    /// <summary>
    /// Use Dynamic WebApi to Configure
    /// </summary>
    /// <param name="application"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseDynamicApi(this IApplicationBuilder application, Action<IServiceProvider,DynamicApiOptions> optionsAction)
    {
        var options = new DynamicApiOptions();

        optionsAction?.Invoke(application.ApplicationServices,options);

        options.Valid();

        AppConsts.DefaultAreaName = options.DefaultAreaName;
        AppConsts.DefaultHttpVerb = options.DefaultHttpVerb;
        AppConsts.DefaultApiPreFix = options.DefaultApiPrefix;
        AppConsts.ControllerPostfixes = options.RemoveControllerPostfixes;
        AppConsts.ActionPostfixes = options.RemoveActionPostfixes;
        AppConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;
        AppConsts.NamingConvention = options.NamingConvention;
        AppConsts.GetRestFulControllerName = options.GetRestFulControllerName;
        AppConsts.GetRestFulActionName = options.GetRestFulActionName;
        AppConsts.AssemblyDynamicApiOptions = options.AssemblyDynamicApiOptions;

        var partManager = application.ApplicationServices.GetRequiredService<ApplicationPartManager>();

        // Add a custom controller checker
        var featureProviders = application.ApplicationServices.GetRequiredService<DynamicApiControllerFeatureProvider>();
        partManager.FeatureProviders.Add(featureProviders);

        foreach(var assembly in options.AssemblyDynamicApiOptions.Keys)
        {
            var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);

            foreach(var part in partFactory.GetApplicationParts(assembly))
            {
                partManager.ApplicationParts.Add(part);
            }
        }


        var mvcOptions = application.ApplicationServices.GetRequiredService<IOptions<MvcOptions>>();
        var DynamicApiConvention = application.ApplicationServices.GetRequiredService<DynamicApiConvention>();

        mvcOptions.Value.Conventions.Add(DynamicApiConvention);

        return application;
    }

    public static IServiceCollection AddDynamicApiCore<TSelectController, TActionRouteFactory>(this IServiceCollection services)
        where TSelectController: class,ISelectController
        where TActionRouteFactory: class, IActionRouteFactory
    {
        services.AddSingleton<ISelectController, TSelectController>();
        services.AddSingleton<IActionRouteFactory, TActionRouteFactory>();
        services.AddSingleton<DynamicApiConvention>();
        services.AddSingleton<DynamicApiControllerFeatureProvider>();
        return services;
    }

    /// <summary>
    /// Add Dynamic WebApi to Container
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options">configuration</param>
    /// <returns></returns>
    public static IServiceCollection AddDynamicApi(this IServiceCollection services, DynamicApiOptions options)
    {
        if (options == null)
        {
            throw new ArgumentException(nameof(options));
        }

        options.Valid();

        AppConsts.DefaultAreaName = options.DefaultAreaName;
        AppConsts.DefaultHttpVerb = options.DefaultHttpVerb;
        AppConsts.DefaultApiPreFix = options.DefaultApiPrefix;
        AppConsts.ControllerPostfixes = options.RemoveControllerPostfixes;
        AppConsts.ActionPostfixes = options.RemoveActionPostfixes;
        AppConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;
        AppConsts.NamingConvention = options.NamingConvention;
        AppConsts.GetRestFulControllerName = options.GetRestFulControllerName;
        AppConsts.GetRestFulActionName = options.GetRestFulActionName;
        AppConsts.AssemblyDynamicApiOptions = options.AssemblyDynamicApiOptions;
        AppConsts.FormatResult = options.FormatResult;
        AppConsts.FormatResultType = options.FormatResultType;

        var partManager = services.GetSingletonInstanceOrNull<ApplicationPartManager>();

        if (partManager == null)
        {
            throw new InvalidOperationException("\"AddDynamicApi\" must be after \"AddMvc\".");
        }

        // Add a custom controller checker
        partManager.FeatureProviders.Add(new DynamicApiControllerFeatureProvider(options.SelectController));

        services.Configure<MvcOptions>(o =>
        {
            // Register Controller Routing Information Converter
            o.Conventions.Add(new DynamicApiConvention(options.SelectController, options.ActionRouteFactory));
        });

        return services;
    }

    public static IServiceCollection AddDynamicApi(this IServiceCollection services)
    {
        return AddDynamicApi(services, new DynamicApiOptions());
    }

    public static IServiceCollection AddDynamicApi(this IServiceCollection services, Action<DynamicApiOptions> optionsAction)
    {
        var DynamicApiOptions = new DynamicApiOptions();

        optionsAction?.Invoke(DynamicApiOptions);

        return AddDynamicApi(services, DynamicApiOptions);
    }

}