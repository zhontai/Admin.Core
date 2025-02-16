using Polly;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// HttpClientBuilder扩展
/// </summary>
public static class HttpClientBuilderExtension
{
    /// <summary>
    /// 添加PolicyHandler列表
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="policies"></param>
    /// <returns></returns>
    public static IHttpClientBuilder AddPolicyHandlerList(this IHttpClientBuilder builder, List<IAsyncPolicy<HttpResponseMessage>> policies)
    {
        policies?.ForEach(policy => builder.AddPolicyHandler(policy));
        return builder;
    }
}
