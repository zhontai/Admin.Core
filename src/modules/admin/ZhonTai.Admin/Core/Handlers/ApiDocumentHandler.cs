using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Services.Api;
using ZhonTai.Admin.Services.Api.Dto;

namespace ZhonTai.Admin.Core.Handlers;

public class ApiDocumentHandler : IApiDocumentHandler
{
    private readonly AppConfig _appConfig;
    private readonly ISwaggerProvider _swaggerProvider;
    private readonly IApiService _apiService;
    private readonly ILogger<ApiDocumentHandler> _logger;

    public ApiDocumentHandler(AppConfig appConfig, ISwaggerProvider swaggerProvider, IApiService apiService,
        ILogger<ApiDocumentHandler> logger)
    {
        _appConfig = appConfig;
        _swaggerProvider = swaggerProvider;
        _apiService = apiService;
        _logger = logger;
    }

    public async Task SyncAsync()
    {
        try
        {
            var apis = new List<ApiSyncDto>();
            _appConfig.Swagger.Projects?.ForEach(project =>
            {
                var apiDocument = _swaggerProvider.GetSwagger(project.Code.ToLower());
                apis.Add(new ApiSyncDto()
                {
                    Label = project.Name,
                    Path = project.Code.ToLower()
                });
                foreach (var tag in apiDocument.Tags)
                {
                    var apiSyncDto = new ApiSyncDto()
                    {
                        Label = tag.Description,
                        ParentPath = project.Code.ToLower(),
                        Path = tag.Name
                    };
                    apis.Add(apiSyncDto);
                }

                foreach (var path in apiDocument.Paths)
                {
                    foreach (var openApiOperation in path.Value.Operations)
                    {
                        var apiSyncDto = new ApiSyncDto()
                        {
                            Path = path.Key,
                            ParentPath = openApiOperation.Value.Tags[0].Name,
                            HttpMethods = openApiOperation.Key.ToDescriptionOrString().ToLower(),
                            Label = openApiOperation.Value.Summary
                        };
                        apis.Add(apiSyncDto);
                    }
                }
            });
            var apiSyncInput = new ApiSyncInput()
            {
                Apis = apis
            };
            await _apiService.SyncAsync(apiSyncInput);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "ApiDocument Sync Failed");
        }
    }
}