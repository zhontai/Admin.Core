using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Net.Http;
using Refit;
using Polly;
using ZhonTai;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Handlers;

namespace Microsoft.Extensions.DependencyInjection;

public static class HttpExtensions
{
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
                var method = typeof(HttpExtensions)
                    .GetMethod(nameof(AddMyRefitClient))
                    ?.MakeGenericMethod(interfaceType)
                    ?.Invoke(null, [services, rpcConfig, policies]);
            }
        }

        return services;
    }

    public static IServiceCollection AddMyRefitClient<T>(this IServiceCollection services, RpcConfig rpcConfig, List<IAsyncPolicy<HttpResponseMessage>> policies) where T : class
    {
        ArgumentNullException.ThrowIfNull(rpcConfig, nameof(rpcConfig));

        var refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "yyyy-MM-dd HH:mm:ss.FFFFFFFK"
        }));

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
