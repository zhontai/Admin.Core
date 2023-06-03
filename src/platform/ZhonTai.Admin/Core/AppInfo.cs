using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using ZhonTai.Admin.Core.Auth;

namespace ZhonTai.Admin.Core;

/// <summary>
/// 应用全局信息
/// </summary>
public static class AppInfo
{
    static AppInfo()
    {
        EffectiveTypes = EffectiveAssemblies.SelectMany(GetTypes);
    }

    private static bool _isRun;

    /// <summary>
    /// 应用是否运行
    /// </summary>
    public static bool IsRun
    {
        get => _isRun;
        set => _isRun = value;
    }

    /// <summary>
    /// 有效程序集
    /// </summary>
    public static readonly IEnumerable<Assembly> EffectiveAssemblies = GetAllAssemblies();

    /// <summary>
    /// 有效程序集类型
    /// </summary>
    public static readonly IEnumerable<Type> EffectiveTypes;

    /// <summary>
    /// 服务提供程序
    /// </summary>
    public static IServiceProvider ServiceProvider => IsRun ? AppInfoBase.ServiceProvider : null;

    /// <summary>
    /// Web主机环境
    /// </summary>
    public static IWebHostEnvironment WebHostEnvironment => AppInfoBase.WebHostEnvironment;

    /// <summary>
    /// 泛型主机环境
    /// </summary>
    public static IHostEnvironment HostEnvironment => AppInfoBase.HostEnvironment;

    /// <summary>
    /// 配置
    /// </summary>
    public static IConfiguration Configuration => AppInfoBase.Configuration;

    /// <summary>
    /// 请求上下文
    /// </summary>
    public static HttpContext HttpContext => ServiceProvider?.GetService<IHttpContextAccessor>()?.HttpContext;

    /// <summary>
    /// 用户
    /// </summary>
    public static IUser User => HttpContext == null ? null : ServiceProvider?.GetService<IUser>();

    /// <summary>
    /// 日志
    /// </summary>
    public static Logger Log => LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

    #region private

    private static IEnumerable<Type> GetTypes(Assembly ass)
    {
        Type[] source = Array.Empty<Type>();
        try
        {
            source = ass.GetTypes();
        }
        catch (Exception e)
        {
            Log.Error(e, "GetTypes Exception:{msg}", e.Message);
            Console.WriteLine($@"Error load `{ass.FullName}` assembly.");
        }

        return source.Where(u => u.IsPublic);
    }

    private static IList<Assembly> GetAllAssemblies()
    {
        var list = new List<Assembly>();
        var deps = DependencyContext.Default;
        var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package");
        foreach (var lib in libs)
        {
            try
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                list.Add(assembly);
            }
            catch (Exception e)
            {
                Log.Error(e, "GetAllAssemblies Exception:{msg}", e.Message);
            }
        }
        return list;
    }
    #endregion

    #region Service
    /// <summary>
    /// 获得服务提供程序
    /// </summary>
    /// <param name="serviceType"></param>
    /// <param name="isBuild"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static IServiceProvider GetServiceProvider(Type serviceType, bool isBuild = false)
    {
        if (HostEnvironment == null || ServiceProvider != null && 
            AppInfoBase.Services
            .Where(u => u.ServiceType == (serviceType.IsGenericType ? serviceType.GetGenericTypeDefinition() : serviceType))
            .Any(u => u.Lifetime == ServiceLifetime.Singleton))
            return ServiceProvider;
        HttpContext httpContext = HttpContext;

        if (httpContext?.RequestServices != null)
            return httpContext.RequestServices;

        if (ServiceProvider != null)
        {
            IServiceScope scope = ServiceProvider.CreateScope();
            return scope.ServiceProvider;
        }

        if (isBuild)
        {
            throw new ApplicationException("The current is not available and must wait until the WebApplication Build is completed.");
        }

        ServiceProvider serviceProvider = AppInfoBase.Services.BuildServiceProvider();

        return serviceProvider;
    }

    /// <summary>
    /// 获得请求生存周期的服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="isBuild"></param>
    /// <returns></returns>
    public static TService GetService<TService>(bool isBuild = true) where TService : class =>
        GetService(typeof(TService), null, isBuild) as TService;

    /// <summary>
    /// 获得请求生存周期的服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <param name="isBuild"></param>
    /// <returns></returns>
    public static TService GetService<TService>(IServiceProvider serviceProvider, bool isBuild = true) where TService : class => 
        GetService(typeof(TService), serviceProvider, isBuild) as TService;

    /// <summary>
    /// 获得服务
    /// </summary>
    /// <param name="type"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="isBuild"></param>
    /// <returns></returns>
    public static object GetService(Type type, IServiceProvider serviceProvider = null, bool isBuild = true) =>
        (serviceProvider ?? GetServiceProvider(type, isBuild)).GetService(type);

    /// <summary>
    /// 获得服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="isBuild"></param>
    /// <returns></returns>
    public static TService GetRequiredService<TService>(bool isBuild = true) where TService : class =>
        GetRequiredService(typeof(TService), null, isBuild) as TService;

    /// <summary>
    /// 获取服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <param name="isBuild"></param>
    /// <returns></returns>
    public static TService GetRequiredService<TService>(IServiceProvider serviceProvider, bool isBuild = true) where TService : class =>
        GetRequiredService(typeof(TService), serviceProvider, isBuild) as TService;

    /// <summary>
    /// 获得服务
    /// </summary>
    /// <param name="type"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="isBuild"></param>
    /// <returns></returns>
    public static object GetRequiredService(Type type, IServiceProvider serviceProvider = null, bool isBuild = true) =>
        (serviceProvider ?? GetServiceProvider(type, isBuild)).GetRequiredService(type);

    #endregion

    #region Options
    /// <summary>
    /// 获得选项
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static TOptions GetOptions<TOptions>(string path) where TOptions : class, new() =>
        Configuration.GetSection(path).Get<TOptions>();

    /// <summary>
    /// 获得选项
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static TOptions GetOptions<TOptions>(IServiceProvider serviceProvider = null) where TOptions : class, new() =>
        GetService<IOptions<TOptions>>(serviceProvider ?? ServiceProvider, false)?.Value;

    /// <summary>
    /// 获得选项
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static TOptions GetOptionsMonitor<TOptions>(IServiceProvider serviceProvider = null) where TOptions : class, new() =>
        GetService<IOptionsMonitor<TOptions>>(serviceProvider ?? ServiceProvider, false)?.CurrentValue;

    /// <summary>
    /// 获得选项
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static TOptions GetOptionsSnapshot<TOptions>(IServiceProvider serviceProvider = null) where TOptions : class, new() =>
        GetService<IOptionsSnapshot<TOptions>>(serviceProvider, false)?.Value;
    #endregion
}
