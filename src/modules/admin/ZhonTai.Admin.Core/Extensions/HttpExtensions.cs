using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using Refit;
using Polly;
using ZhonTai;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Handlers;
using ZhonTai.Common.Helpers;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Http扩展
/// </summary>
public static class HttpExtensions
{
    /// <summary>
    /// 添加Http客户端
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <param name="rpcConfig"></param>
    /// <param name="policies"></param>
    /// <returns></returns>
    public static IServiceCollection AddMyHttpClients(this IServiceCollection services, IEnumerable<Assembly> assemblies, RpcConfig rpcConfig, List<IAsyncPolicy<HttpResponseMessage>> policies)
    {
        ArgumentNullException.ThrowIfNull(assemblies, nameof(assemblies));
        ArgumentNullException.ThrowIfNull(rpcConfig, nameof(rpcConfig));

        foreach (var assembly in assemblies)
        {
            var interfaceTypes = assembly.GetTypes()
            .Where(type => type.GetCustomAttributes<HttpClientContractAttribute>(false).Any() && type.IsInterface)
            .ToList();

            foreach (var interfaceType in interfaceTypes)
            {
                typeof(HttpExtensions)
                .GetMethod(nameof(AddMyRefitClient))
                ?.MakeGenericMethod(interfaceType)
                ?.Invoke(null, [services, rpcConfig, policies]);
            }
        }

        return services;
    }

    /// <summary>
    /// 添加Refit客户端
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="rpcConfig"></param>
    /// <param name="policies"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static IServiceCollection AddMyRefitClient<T>(this IServiceCollection services, RpcConfig rpcConfig, List<IAsyncPolicy<HttpResponseMessage>> policies) where T : class
    {
        ArgumentNullException.ThrowIfNull(rpcConfig, nameof(rpcConfig));

        var refitSettings = new RefitSettings(new SystemTextJsonContentSerializer(JsonHelper.GetCurrentOptions()));

        services.TryAddScoped<ResponseDelegatingHandler>();

        services
        .AddRefitClient<T>(refitSettings)
        .AddHttpMessageHandler<ResponseDelegatingHandler>()
        .ConfigureHttpClient(c =>
        {
            var httpClientContractAttribute = typeof(T).GetCustomAttributes<HttpClientContractAttribute>(true).FirstOrDefault();
            if (httpClientContractAttribute is null)
                throw new NullReferenceException(nameof(httpClientContractAttribute));

            var address = rpcConfig.Endpoints.FirstOrDefault(a => a.Name.EqualsIgnoreCase(httpClientContractAttribute.ModuleName));
            if (address is null)
                throw new NullReferenceException(nameof(address));

            c.BaseAddress = new Uri(address.HttpUrl);

            var httpContextAccessor = services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>();
            var authorization = httpContextAccessor?.HttpContext?.Request?.Headers?.Authorization.FirstOrDefault();
            if (authorization.NotNull())
            {
                c.DefaultRequestHeaders.Add("Authorization", authorization);
            }
            var userAgent = httpContextAccessor?.HttpContext?.Request?.Headers?.UserAgent.FirstOrDefault();
            if (userAgent.NotNull())
            {
                c.DefaultRequestHeaders.Add("User-Agent", userAgent);
            }
        })
        .AddPolicyHandlerList(policies);

        return services;
    }
}
