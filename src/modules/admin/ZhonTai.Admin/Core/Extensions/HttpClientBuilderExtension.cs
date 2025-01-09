using Polly;
using System.Collections.Generic;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection;

public static class HttpClientBuilderExtension
{
    public static IHttpClientBuilder AddPolicyHandlerList(this IHttpClientBuilder builder, List<IAsyncPolicy<HttpResponseMessage>> policies)
    {
        policies?.ForEach(policy => builder.AddPolicyHandler(policy));
        return builder;
    }
}
