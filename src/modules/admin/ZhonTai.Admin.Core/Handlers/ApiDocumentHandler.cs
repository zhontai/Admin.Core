using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.GrpcServices;
using ZhonTai.Admin.Core.GrpcServices.Dtos;
using ZhonTai.Common.Helpers;

namespace ZhonTai.Admin.Core.Handlers;

/// <summary>
/// Api Document Handler
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
    /// Sync Api Document
    /// </summary>
    /// <returns></returns>
    public async Task SyncAsync()
    {
        var projects = _appConfig.Swagger.Projects;
        if (projects == null || projects.Count == 0)
        {
            return;
        }

        var isFirst = true;
        foreach (var project in projects)
        {
            try
            {
                if (project.AutoSyncToDb == true)
                {
                    await SyncProjectAsync(project, isFirst);
                    isFirst = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API document sync failed");
            }
        }
    }

    private async Task SyncProjectAsync(ProjectConfig project, bool isFirst = false)
    {
        try
        {
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

            Console.WriteLine($"{(isFirst ? Environment.NewLine : "")}Start syncing {project.Name} API document");
            var apiSyncInput = new ApiSyncGrpcInput { Apis = apis };
            await _apiGrpcService.SyncAsync(apiSyncInput);

            ConsoleHelper.WriteSuccessLine($"{project.Name} synced {apis.Count} APIs successfully{Environment.NewLine}");
        }
        catch (Exception)
        {
            ConsoleHelper.WriteErrorLine($"{project.Name} sync failed");
            throw;
        }
    }
}