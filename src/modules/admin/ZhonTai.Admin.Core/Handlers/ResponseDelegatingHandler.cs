using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Exceptions;
using ZhonTai.Common.Helpers;

namespace ZhonTai.Admin.Core.Handlers;

/// <summary>
/// 响应处理器
/// </summary>
public class ResponseDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var res = JsonHelper.Deserialize<ResultOutput<object>>(content);
            if (!res.Success && res.Msg.NotNull())
            {
                throw new AppException(res.Msg);
            }

            response.Content = new StringContent(JsonHelper.Serialize(res.Data));
        }

        return response;
    }
}
