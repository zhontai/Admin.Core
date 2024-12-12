using ZhonTai.DynamicApi.Attributes;
using ZhonTai.DynamicApi;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core;
using ZhonTai.Common.Helpers;
using Yitter.IdGenerator;
using Microsoft.Extensions.Options;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Services.WebSocket.Dto;
using Microsoft.AspNetCore.Authorization;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Services;

/// <summary>
/// WebSocket
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class WebSocketService : IDynamicApi
{
    private readonly IOptions<ImConfig> _imConfig;

    public WebSocketService(IOptions<ImConfig> imConfig)
    {
        _imConfig = imConfig;
    }

    /// <summary>
    /// 获取websocket分区
    /// </summary>
    [HttpPost]
    public object PreConnect(WebSocketPreConnectInput input)
    {
        var websocketId = input.WebsocketId ?? AppInfo.User?.Id;
        if (websocketId == null) websocketId = YitIdHelper.NextId();
        var wsServer = ImHelper.PrevConnectServer(websocketId.Value, IPHelper.GetIP(AppInfo.HttpContext.Request));

        var bizServer = _imConfig.Value.Server;
        if (bizServer.NotNull())
        {
            var servers = _imConfig.Value.Servers;
            foreach (var server in servers)
            {
                wsServer = wsServer.Replace($"ws://{server}", bizServer);
            }
        }
        
        return new
        {
            server = wsServer,
            websocketId
        };
    }

    /// <summary>
    /// 是否使用im
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [NoOperationLog]
    public bool IsUseIm()
    {
        return _imConfig.Value.Enable;
    }
}
