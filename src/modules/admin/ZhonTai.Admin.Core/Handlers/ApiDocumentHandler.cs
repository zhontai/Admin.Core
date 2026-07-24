using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.GrpcServices;
using ZhonTai.Admin.Core.GrpcServices.Dtos;

namespace ZhonTai.Admin.Core.Handlers;

/// <summary>
/// Api文档处理器
/// </summary>
public class ApiDocumentHandler : IApiDocumentHandler
{
    private readonly AppConfig _appConfig;
    private readonly ISwaggerProvider _swaggerProvider;
    private readonly IApiGrpcService _apiGrpcService;
    private readonly ILogger<ApiDocumentHandler> _logger;

    public ApiDocumentHandler(AppConfig appConfig, ISwaggerProvider swaggerProvider, IApiGrpcService apiGrpcService,
        ILogger<ApiDocumentHandler> logger)
    {
        _appConfig = appConfig;
        _swaggerProvider = swaggerProvider;
        _apiGrpcService = apiGrpcService;
        _logger = logger;
    }

    /// <summary>
    /// 同步Api文档
    /// </summary>
    /// <returns></returns>
    public async Task SyncAsync()
    {
        var projects = _appConfig.Swagger.Projects;
        if (projects == null || projects.Count == 0)
        {
            return;
        }

        var tasks = projects.Select(SyncProjectAsync);

        try
        {
            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            // 记录聚合异常
            if (ex is AggregateException aggEx)
            {
                foreach (var innerEx in aggEx.InnerExceptions)
                {
                    _logger.LogError(innerEx, "API文档同步失败");
                }
            }
            else
            {
                _logger.LogError(ex, "API文档同步失败");
            }
        }
    }

    private async Task SyncProjectAsync(ProjectConfig project)
    {
        try
        {
            if (project.AutoSyncToDb == false)
            {
                return;
            }

            var apis = new List<ApiSyncGrpcInput.Models.ApiSyncModel>();
            var apiDocument = _swaggerProvider.GetSwagger(project.Code.ToLower());

            apis.Add(new ApiSyncGrpcInput.Models.ApiSyncModel()
            {
                Label = project.Name,
                Path = project.Code.ToLower()
            });
            foreach (var tag in apiDocument.Tags)
            {
                var apiSyncDto = new ApiSyncGrpcInput.Models.ApiSyncModel()
                {
                    Label = tag.Description,
                    Path = project.Code.ToLower() + "/" + tag.Name,
                    ParentPath = project.Code.ToLower(),
                };
                apis.Add(apiSyncDto);
            }

            foreach (var path in apiDocument.Paths)
            {
                foreach (var openApiOperation in path.Value.Operations)
                {
                    var apiSyncDto = new ApiSyncGrpcInput.Models.ApiSyncModel()
                    {
                        Path = path.Key,
                        ParentPath = project.Code.ToLower() + "/" + openApiOperation.Value.Tags.First().Name,
                        HttpMethods = openApiOperation.Key.Method.ToLower(),
                        Label = openApiOperation.Value.Summary
                    };
                    apis.Add(apiSyncDto);
                }
            }

            if (apis.Count == 0)
            {
                return;
            }

            var apiSyncInput = new ApiSyncGrpcInput { Apis = apis };
            await _apiGrpcService.SyncAsync(apiSyncInput);

            Console.WriteLine($"{project.Name}同步{apis.Count}个接口成功");
        }
        catch (Exception)
        {
            Console.WriteLine($"{project.Name}同步失败");
            throw;
        }
    }
}