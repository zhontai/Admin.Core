using Grpc.Net.Client.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Polly;
using ProtoBuf.Grpc.ClientFactory;
using System.Reflection;
using System.ServiceModel;
using ZhonTai;
using ZhonTai.Admin.Core.Configs;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Grpc扩展
/// </summary>
public static class GrpcExtensions
{
    /// <summary>
    /// 添加Grpc客户端
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <param name="rpcConfig"></param>
    /// <param name="policies"></param>
    /// <returns></returns>
    public static IServiceCollection AddMyGrpcClients(this IServiceCollection services, IEnumerable<Assembly> assemblies, RpcConfig rpcConfig, List<IAsyncPolicy<HttpResponseMessage>> policies)
    {
        ArgumentNullException.ThrowIfNull(assemblies, nameof(assemblies));
        ArgumentNullException.ThrowIfNull(rpcConfig, nameof(rpcConfig));

        foreach (var assembly in assemblies)
        {
            var interfaceTypes = assembly.GetTypes()
            .Where(type => type.GetCustomAttributes<ServiceContractAttribute>(false).Any() && type.IsInterface)
            .ToList();

            foreach (var interfaceType in interfaceTypes)
            {
                typeof(GrpcExtensions)
                .GetMethod(nameof(AddMyCodeFirstGrpcClient))
                ?.MakeGenericMethod(interfaceType)
                ?.Invoke(null, [services, rpcConfig, policies]);
            }
        }

        return services;
    }

    /// <summary>
    /// 添加CodeFirstGrpc客户端
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="rpcConfig"></param>
    /// <param name="policies"></param>
    /// <returns></returns>
    public static IServiceCollection AddMyCodeFirstGrpcClient<T>(this IServiceCollection services, RpcConfig rpcConfig, List<IAsyncPolicy<HttpResponseMessage>> policies) where T : class
    {
        ArgumentNullException.ThrowIfNull(rpcConfig, nameof(rpcConfig));

        services.AddCodeFirstGrpcClient<T>(o =>
        {
            var serviceContract = typeof(T).GetCustomAttributes<ServiceContractAttribute>(false).FirstOrDefault();
            ArgumentNullException.ThrowIfNull(serviceContract, nameof(serviceContract));

            var address = rpcConfig.Endpoints.FirstOrDefault(a => a.Name.EqualsIgnoreCase(serviceContract.ConfigurationName));
            ArgumentNullException.ThrowIfNull(address, nameof(address));

            // Address of grpc server
            o.Address = new Uri(address.GrpcUrl);

            // another channel options (based on best practices docs on https://docs.microsoft.com/en-us/aspnet/core/grpc/performance?view=aspnetcore-6.0)
            o.ChannelOptionsActions.Add(options =>
            {
                options.HttpHandler = new SocketsHttpHandler()
                {
                    // keeps connection alive
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),

                    // allows channel to add additional HTTP/2 connections
                    EnableMultipleHttp2Connections = true
                };
            });
        })
        .ConfigureChannel(options =>
        {
            options.UnsafeUseInsecureChannelCallCredentials = true;
            options.ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } };
        })
        .AddCallCredentials((context, metadata, serviceProvider) =>
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var authorization = httpContextAccessor?.HttpContext?.Request?.Headers?.Authorization.FirstOrDefault();
            if (authorization.NotNull())
                metadata.Add("Authorization", authorization);

            var userAgent = httpContextAccessor?.HttpContext?.Request?.Headers?.UserAgent.FirstOrDefault();
            if (userAgent.NotNull())
                metadata.Add("User-Agent", userAgent);

            return Task.CompletedTask;
        })
        .AddPolicyHandlerList(policies);

        return services;
    }

    /// <summary>
    /// 使用Grpc服务
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder UseMyMapGrpcService(this IEndpointRouteBuilder endpointRouteBuilder, IEnumerable<Assembly> assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies, nameof(assemblies));

        foreach (var assembly in assemblies)
        {
            var grpcServiceTypes = assembly.GetTypes()
            .Where(type =>
                type.GetInterfaces().Length != 0 &&
                type.GetInterfaces().Any(interfaceType => interfaceType.GetCustomAttributes<ServiceContractAttribute>(true).Any())
                && type.IsClass)
            .ToList();

            foreach (var grpcServiceType in grpcServiceTypes)
            {
                typeof(GrpcEndpointRouteBuilderExtensions)
                .GetMethod(nameof(GrpcEndpointRouteBuilderExtensions.MapGrpcService))
                ?.MakeGenericMethod(grpcServiceType)
                ?.Invoke(null, [endpointRouteBuilder]);
            }
        }

        return endpointRouteBuilder;
    }
}
