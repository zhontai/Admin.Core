using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevGroup;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevProject;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevProjectGen;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevProjectModel;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevProjectModelField;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevTemplate;
using ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectGen.Dtos;
using ZhonTai.Module.Dev.Api.Core.Consts;
using ZhonTai.Module.Dev.Api.Core.Repositories;

namespace ZhonTai.Module.Dev.Services.DevProjectGen;

/// <summary>
/// 项目生成服务
/// </summary>
[DynamicApi(Area = ApiConsts.AreaName)]
public partial class DevProjectGenService : BaseService, IDynamicApi
{
    private readonly AppRepositoryBase<DevProjectGenEntity> _devProjectGenRep;
    private readonly AppRepositoryBase<DevGroupEntity> _devGroupRep;
    private readonly AppRepositoryBase<DevTemplateEntity> _devTemplateRep;
    private readonly AppRepositoryBase<DevProjectEntity> _devProjectRep;
    private readonly AppRepositoryBase<DevProjectModelEntity> _devProjectModelRep;
    private readonly AppRepositoryBase<DevProjectModelFieldEntity> _devProjectModelFieldRep;

    public DevProjectGenService(AppRepositoryBase<DevProjectGenEntity> devProjectGenRep,
        AppRepositoryBase<DevGroupEntity> devGroupRep,
        AppRepositoryBase<DevTemplateEntity> devTemplateRep,
        AppRepositoryBase<DevProjectEntity> devProjectRep,
        AppRepositoryBase<DevProjectModelEntity> devProjectModelRep,
        AppRepositoryBase<DevProjectModelFieldEntity> devProjectModelFieldRep)
    {
        _devProjectGenRep = devProjectGenRep;
        _devGroupRep = devGroupRep;
        _devTemplateRep = devTemplateRep;
        _devProjectRep = devProjectRep;
        _devProjectModelRep = devProjectModelRep;
        _devProjectModelFieldRep = devProjectModelFieldRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<DevProjectGenGetOutput> GetAsync(long id)
    {
        var output = await _devProjectGenRep.GetAsync<DevProjectGenGetOutput>(id);
        return output;
    }
    
    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IEnumerable<DevProjectGenGetListOutput>> GetListAsync(DevProjectGenGetListInput input)
    {
        var list = await _devProjectGenRep.Select
            .WhereIf(input.ProjectId != null, a=>a.ProjectId == input.ProjectId)
            .OrderByDescending(a => a.Id)
            .ToListAsync<DevProjectGenGetListOutput>();
        return list;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<DevProjectGenGetPageOutput>> GetPageAsync(PageInput<DevProjectGenGetPageInput> input)
    {
        var filter = input.Filter;
        var list = await _devProjectGenRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .WhereIf(filter !=null && filter.ProjectId != null, a=>a.ProjectId == filter.ProjectId)
            .Count(out var total)
            .OrderByDescending(c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<DevProjectGenGetPageOutput>();
    

        //关联查询代码
        //数据转换-单个关联
        var projectIdRows = list.Where(s => s.ProjectId > 0).ToList();
        if (projectIdRows.Any())
        {
            var projectIdRowsIds = projectIdRows.Select(s => s.ProjectId).Distinct().ToList();
            var projectIdRowsIdsData = await _devProjectRep.Where(s => projectIdRowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.Name });
            projectIdRows.ForEach(s =>
            {
                s.ProjectId_Text = projectIdRowsIdsData.FirstOrDefault(s2 => s2.Id == s.ProjectId)?.Name;
            });
        }
        //数据转换-多个关联
        var groupIdsRows = list.Where(s => s.GroupIds_Values != null && s.GroupIds_Values.Any()).ToList();
        if (groupIdsRows.Any())
        {
            var groupIdsRowsIds =groupIdsRows.SelectMany(s => s.GroupIds_Values).Select(s => long.TryParse(s, out long s2) ? s2 : 0).Distinct().ToList();
            var groupIdsRowsIdsData = await _devGroupRep.Where(s => groupIdsRowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.Name });
            groupIdsRows.ForEach(s =>
            {
                s.GroupIds_Texts = groupIdsRowsIdsData.Where(s2 => s.GroupIds_Values.Contains(s2.Id.ToString())).OrderBy(s2 => s.GroupIds_Values.IndexOf(s2.Id.ToString())).Select(s2 => s2.Name).ToList();
            });
        }

        var data = new PageOutput<DevProjectGenGetPageOutput> { List = list, Total = total };
    
        return data;
    }
    

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(DevProjectGenAddInput input)
    {
        var entity = Mapper.Map<DevProjectGenEntity>(input);
        var id = (await _devProjectGenRep.InsertAsync(entity)).Id;

        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(DevProjectGenUpdateInput input)
    {
        var entity = await _devProjectGenRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("项目生成不存在！");
        }

        Mapper.Map(input, entity);
        await _devProjectGenRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _devProjectGenRep.DeleteAsync(id) > 0;
    }


    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> BatchDeleteAsync(long[] ids)
    {
        return await _devProjectGenRep.Where(w=>ids.Contains(w.Id)).ToDelete().ExecuteAffrowsAsync() > 0;
    }


    /// <summary>
    /// 批量软删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> BatchSoftDeleteAsync(long[] ids)
    {
        return await _devProjectGenRep.SoftDeleteAsync(ids);
    }
}