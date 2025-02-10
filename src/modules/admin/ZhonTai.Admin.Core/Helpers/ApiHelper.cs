using System.Reflection;
using Microsoft.Extensions.Options;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Tools.Cache;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.GrpcServices;
using ZhonTai.Admin.Services.Api.Dto;
using ZhonTai.Common.Helpers;
using ZhonTai.Common.Extensions;

namespace ZhonTai.Admin.Core.Helpers;

/// <summary>
/// Api帮助类
/// </summary>
[InjectSingleton]
public class ApiHelper
{
    private readonly ICacheTool _cacheTool;
    private readonly IOptions<AppConfig> _appConfig;
    private readonly IApiGrpcService _apiGrpcService;

    public ApiHelper(ICacheTool cacheTool, IOptions<AppConfig> appConfig, IApiGrpcService apiGrpcService)
    {
        _cacheTool = cacheTool;
        _appConfig = appConfig;
        _apiGrpcService = apiGrpcService;
    }

    public async Task<List<ApiModel>> GetApiListAsync()
    {
        return await _cacheTool.GetOrSetAsync(CacheKeys.ApiList, async () =>
        {
            var res = await _apiGrpcService.GetApiList();
            var apis = res?.Data;

            var apiList = new List<ApiModel>();
            foreach (var api in apis)
            {
                var parentLabel = apis.FirstOrDefault(a => a.Id == api.ParentId)?.Label;

                apiList.Add(new ApiModel
                {
                    Label = parentLabel.NotNull() ? $"{parentLabel} / {api.Label}" : api.Label,
                    Path = api.Path?.ToLower().Trim('/'),
                    EnabledLog = api.EnabledLog,
                    EnabledParams = api.EnabledParams,
                    EnabledResult = api.EnabledResult,
                });
            }

            return apiList;
        });
    }

    public List<ApiGetEnumsOutput> GetEnumList()
    {
        var enums = new List<ApiGetEnumsOutput>();

        var appConfig = _appConfig.Value;
        var assemblyNames = appConfig.EnumListAssemblyNames;
        if (!(assemblyNames?.Length > 0))
        {
            return enums;
        }

        foreach (var assemblyName in assemblyNames)
        {
            var assembly = Assembly.Load(assemblyName);
            var enumTypes = assembly.GetTypes().Where(m => m.IsEnum);
            foreach (var enumType in enumTypes)
            {
                var summaryList = SummaryHelper.GetEnumSummaryList(enumType);

                var enumDescriptor = new ApiGetEnumsOutput
                {
                    Name = enumType.Name,
                    Desc = enumType.ToDescription() ?? (summaryList.TryGetValue("", out var comment) ? comment : ""),
                    Options = Enum.GetValues(enumType).Cast<Enum>().Select(x => new ApiGetEnumsOutput.Models.Options
                    {
                        Name = x.ToString(),
                        Desc = x.ToDescription(false) ?? (summaryList.TryGetValue(x.ToString(), out var comment) ? comment : ""),
                        Value = x.ToInt64()
                    }).ToList()
                };

                enums.Add(enumDescriptor);
            }
        }

        return enums;
    }
}

public class ApiModel
{
    /// <summary>
    /// 接口名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    public string Path { get; set; }
    /// <summary>
    /// 启用接口日志
    /// </summary>
    public bool EnabledLog { get; set; }
    /// <summary>
    /// 启用请求参数
    /// </summary>
    public bool EnabledParams { get; set; }

    /// <summary>
    /// 启用响应结果
    /// </summary>
    public bool EnabledResult { get; set; }
}