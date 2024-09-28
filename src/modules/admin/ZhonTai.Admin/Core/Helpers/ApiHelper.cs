using System.Collections.Generic;
using System.Linq;
using ZhonTai.Admin.Domain.Api;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Tools.Cache;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Consts;

namespace ZhonTai.Admin.Core.Handlers;

/// <summary>
/// Api帮助类
/// </summary>
[InjectSingleton]
public class ApiHelper
{
    private readonly ICacheTool _cacheTool;
    private readonly IApiRepository _apiRepository;
    

    public ApiHelper(ICacheTool cacheTool, IApiRepository apiRepository)
    {
        _cacheTool = cacheTool;
        _apiRepository = apiRepository;
    }

    public async Task<List<ApiModel>> GetApiListAsync()
    {
        return await _cacheTool.GetOrSetAsync(CacheKeys.ApiList, async () =>
        {
            var apis = await _apiRepository.Select.ToListAsync(a => new { a.Id, a.ParentId, a.Label, a.Path, a.EnabledParams, a.EnabledResult });

            var apiList = new List<ApiModel>();
            foreach (var api in apis)
            {
                var parentLabel = apis.FirstOrDefault(a => a.Id == api.ParentId)?.Label;

                apiList.Add(new ApiModel
                {
                    Label = parentLabel.NotNull() ? $"{parentLabel} / {api.Label}" : api.Label,
                    Path = api.Path?.ToLower().Trim('/'),
                    EnabledParams = api.EnabledParams,
                    EnabledResult = api.EnabledResult,
                });
            }

            return apiList;
        });
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
    /// 启用请求参数
    /// </summary>
    public bool EnabledParams { get; set; }

    /// <summary>
    /// 启用响应结果
    /// </summary>
    public bool EnabledResult { get; set; }
}