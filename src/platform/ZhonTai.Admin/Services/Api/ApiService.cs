using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Api;
using ZhonTai.Admin.Services.Api.Dto;
using ZhonTai.Admin.Domain.Api.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Repositories;
using System;
using ZhonTai.Admin.Core.Configs;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using ZhonTai.Common.Extensions;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Resources;

namespace ZhonTai.Admin.Services.Api;

/// <summary>
/// 接口服务
/// </summary>
[Order(90)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class ApiService : BaseService, IApiService, IDynamicApi
{
    private readonly AdminRepositoryBase<ApiEntity> _apiRep;
    private readonly Lazy<AppConfig> _appConfig;
    private readonly AdminLocalizer _adminLocalizer;

    public ApiService(AdminRepositoryBase<ApiEntity> apiRep, 
        Lazy<AppConfig> appConfig,
        AdminLocalizer adminLocalizer)
    {
        _apiRep = apiRep;
        _appConfig = appConfig;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ApiGetOutput> GetAsync(long id)
    {
        var result = await _apiRep.GetAsync<ApiGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<List<ApiListOutput>> GetListAsync(string key)
    {
        var data = await _apiRep
            .WhereIf(key.NotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync<ApiListOutput>();

        return data;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ApiEntity>> GetPageAsync(PageInput<ApiGetPageDto> input)
    {
        var key = input.Filter?.Label;

        var list = await _apiRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
        .Count(out var total)
        .OrderBy(a => a.ParentId)
        .OrderBy(a => a.Sort)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync();

        var data = new PageOutput<ApiEntity>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(ApiAddInput input)
    {
        var path = input.Path;

        var entity = await _apiRep.Select.DisableGlobalFilter(FilterNames.Delete)
            .Where(w => w.Path.Equals(path) && w.IsDeleted).FirstAsync();

        if (entity?.Id > 0)
        {
            Mapper.Map(input, entity);
            entity.IsDeleted = false;
            entity.Enabled = true;
            await _apiRep.UpdateDiy.DisableGlobalFilter(FilterNames.Delete).SetSource(entity).ExecuteAffrowsAsync();

            return entity.Id;
        }
        entity = Mapper.Map<ApiEntity>(input);

        if (entity.Sort == 0)
        {
            var sort = await _apiRep.Select.DisableGlobalFilter(FilterNames.Delete).Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _apiRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(ApiUpdateInput input)
    {
        var entity = await _apiRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["接口不存在"]);
        }

        Mapper.Map(input, entity);
        await _apiRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _apiRep.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchDeleteAsync(long[] ids)
    {
        await _apiRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task SoftDeleteAsync(long id)
    {
        await _apiRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _apiRep.SoftDeleteAsync(ids);
    }

    /// <summary>
    /// 同步
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SyncAsync(ApiSyncInput input)
    {
        if (!(input?.Apis?.Count > 0)) return;

        //查询分组下所有模块的api
        var groupPaths = input.Apis.FindAll(a => a.ParentPath.IsNull()).Select(a => a.Path);
        var groups = await _apiRep.Select.DisableGlobalFilter(FilterNames.Delete)
            .Where(a => a.ParentId == 0 && groupPaths.Contains(a.Path)).ToListAsync();
        var groupIds = groups.Select(a => a.Id);
        var modules = await _apiRep.Select.DisableGlobalFilter(FilterNames.Delete)
            .Where(a => groupIds.Contains(a.ParentId)).ToListAsync();
        var moduleIds = modules.Select(a => a.Id);
        var apis = await _apiRep.Select.DisableGlobalFilter(FilterNames.Delete)
            .Where(a=> moduleIds.Contains(a.ParentId)).ToListAsync();

        apis = groups.Concat(modules).Concat(apis).ToList();
        var paths = apis.Select(a => a.Path).ToList();

        //path处理
        foreach (var api in input.Apis)
        {
            api.Path = api.Path?.Trim().ToLower();
            api.ParentPath = api.ParentPath?.Trim().ToLower();
        }

        #region 执行插入

        //执行父级api插入
        var parentApis = input.Apis.FindAll(a => a.ParentPath.IsNull());
        var pApis = (from a in parentApis where !paths.Contains(a.Path) select a).ToList();
        if (pApis.Count > 0)
        {
            var insertPApis = Mapper.Map<List<ApiEntity>>(pApis);
            insertPApis = await _apiRep.InsertAsync(insertPApis);
            apis.AddRange(insertPApis);
        }

        //执行子级api插入
        var childApis = input.Apis.FindAll(a => a.ParentPath.NotNull());
        var cApis = (from a in childApis where !paths.Contains(a.Path) select a).ToList();
        if (cApis.Count > 0)
        {
            var insertCApis = Mapper.Map<List<ApiEntity>>(cApis);
            insertCApis = await _apiRep.InsertAsync(insertCApis);
            apis.AddRange(insertCApis);
        }

        #endregion 执行插入

        #region 修改和禁用

        {
            //父级api修改
            ApiEntity a;
            List<string> labels;
            string label;
            string desc;
            for (int i = 0, len = parentApis.Count; i < len; i++)
            {
                ApiSyncDto api = parentApis[i];
                a = apis.Find(a => a.Path == api.Path);
                if (a?.Id > 0)
                {
                    labels = api.Label?.Split("\r\n")?.ToList();
                    label = labels != null && labels.Count > 0 ? labels[0] : string.Empty;
                    desc = labels != null && labels.Count > 1 ? string.Join("\r\n", labels.GetRange(1, labels.Count - 1)) : string.Empty;
                    a.ParentId = 0;
                    a.Label = label;
                    a.Description = desc;
                    a.Sort = i + 1;
                    a.Enabled = true;
                    a.IsDeleted = false;
                }
            }
        }

        {
            //子级api修改
            ApiEntity a;
            ApiEntity pa;
            List<string> labels;
            string label;
            string desc;
            for (int i = 0, len = childApis.Count; i < len; i++)
            {
                ApiSyncDto api = childApis[i];
                a = apis.Find(a => a.Path == api.Path);
                pa = apis.Find(a => a.Path == api.ParentPath);
                if (a?.Id > 0)
                {
                    labels = api.Label?.Split("\r\n")?.ToList();
                    label = labels != null && labels.Count > 0 ? labels[0] : string.Empty;
                    desc = labels != null && labels.Count > 1 ? string.Join("\r\n", labels.GetRange(1, labels.Count - 1)) : string.Empty;

                    a.ParentId = pa.Id;
                    a.Label = label;
                    a.Description = desc;
                    a.HttpMethods = api.HttpMethods;
                    a.Sort = i + 1;
                    a.Enabled = true;
                    a.IsDeleted = false;
                }
            }
        }

        {
            //模块和api禁用
            var inputPaths = input.Apis.Select(a => a.Path).ToList();
            var disabledApis = (from a in apis where !inputPaths.Contains(a.Path) select a).ToList();
            if (disabledApis.Count > 0)
            {
                foreach (var api in disabledApis)
                {
                    api.Enabled = false;
                }
            }
        }

        #endregion 修改和禁用

        //批量更新
        await _apiRep.UpdateDiy.DisableGlobalFilter(FilterNames.Delete).SetSource(apis)
        .UpdateColumns(a => new { a.ParentId, a.Label, a.HttpMethods, a.Description, a.Sort, a.Enabled, a.IsDeleted, a.ModifiedTime })
        .ExecuteAffrowsAsync();
    }

    /// <summary>
    /// 获得项目列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [NoOprationLog]
    public List<ProjectConfig> GetProjects()
    {
        return _appConfig.Value.Swagger.Projects;
    }

#if DEBUG
    /// <summary>
    /// 获得枚举列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [NoOprationLog]
    [AllowAnonymous]
    public List<ApiGetEnumsOutput> GetEnums()
    {
        var enums = new List<ApiGetEnumsOutput>();
        
        var appConfig = _appConfig.Value;
        var assemblyNames = appConfig.AssemblyNames;
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
#endif

}