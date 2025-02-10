using Polly;

namespace ZhonTai.Admin.Core.Helpers;

/// <summary>
/// 策略帮助类
/// </summary>
public class PolicyHelper
{
    public static List<IAsyncPolicy<HttpResponseMessage>> GetPolicyList()
    {
        //隔离策略
        //var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(10, 100);

        //回退策略
        //回退也称为服务降级，用于指定在发生故障时的备用方案。
        //var fallbackPolicy = Policy<string>.Handle<HttpRequestException>().FallbackAsync("backup strategy");

        //缓存策略
        //var cachePolicy = Policy.CacheAsync<HttpResponseMessage>(cacheProvider, TimeSpan.FromSeconds(60));

        //超时策略
        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(9);

        // 重试策略
        // 对于超时或响应状态码>=500的错误，最多重试3次。
        var retryPolicy = Policy.Handle<Exception>()
            .OrResult<HttpResponseMessage>(response =>
            {
                return (int)response.StatusCode >= 500;
            })
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(3),
                TimeSpan.FromSeconds(5)
            });

        //熔断策略
        var circuitBreakerPolicy = Policy.Handle<Exception>()
            .CircuitBreakerAsync
            (
                // 在熔断前允许的异常次数
                exceptionsAllowedBeforeBreaking: 2,
                // 熔断持续时间
                durationOfBreak: TimeSpan.FromMinutes(10),
                // 熔断触发事件
                onBreak: (ex, breakDelay) =>
                {
                    Console.WriteLine("熔断触发事件");
                },
                //熔断恢复事件
                onReset: () =>
                {
                    Console.WriteLine("熔断恢复事件");
                },
                //熔断结束事件
                onHalfOpen: () =>
                {
                    Console.WriteLine("熔断结束事件");
                }
            ).AsAsyncPolicy<HttpResponseMessage>();

        return new List<IAsyncPolicy<HttpResponseMessage>>()
        {
            retryPolicy,
            timeoutPolicy,
            circuitBreakerPolicy
        };
    }
}
