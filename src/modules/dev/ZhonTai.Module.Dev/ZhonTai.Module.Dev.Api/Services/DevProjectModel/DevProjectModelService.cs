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
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevProject;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevProjectModel;
using ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectModel.Dtos;
using ZhonTai.Module.Dev.Api.Core.Consts;
using ZhonTai.Module.Dev.Api.Core.Repositories;

namespace ZhonTai.Module.Dev.Services.DevProjectModel;

/// <summary>
/// 项目模型服务
/// </summary>
[DynamicApi(Area = ApiConsts.AreaName)]
public partial class DevProjectModelService : BaseService, IDynamicApi
{
    private readonly AppRepositoryBase<DevProjectModelEntity> _devProjectModelRep;
    private readonly AppRepositoryBase<DevProjectEntity> _devProjectRep;

    public DevProjectModelService(AppRepositoryBase<DevProjectModelEntity> devProjectModelRep,
        AppRepositoryBase<DevProjectEntity> devProjectRep)
    {
        _devProjectModelRep = devProjectModelRep;
        _devProjectRep = devProjectRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<DevProjectModelGetOutput> GetAsync(long id)
    {
        var output = await _devProjectModelRep.GetAsync<DevProjectModelGetOutput>(id);
        return output;
    }
    
    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IEnumerable<DevProjectModelGetListOutput>> GetListAsync(DevProjectModelGetListInput input)
    {
        var list = await _devProjectModelRep.Select
            .WhereIf(input.ProjectId != null, a=>a.ProjectId == input.ProjectId)
            .WhereIf(!string.IsNullOrEmpty(input.Name), a=>a.Name == input.Name)
            .WhereIf(!string.IsNullOrEmpty(input.Code), a=>a.Code == input.Code)
            .OrderByDescending(a => a.Id)
            .ToListAsync<DevProjectModelGetListOutput>();
        return list;
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<DevProjectModelGetPageOutput>> GetPageAsync(PageInput<DevProjectModelGetPageInput> input)
    {
        var filter = input.Filter;
        var list = await _devProjectModelRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .WhereIf(filter !=null && filter.ProjectId != null, a=>a.ProjectId == filter.ProjectId)
            .WhereIf(filter !=null && !string.IsNullOrEmpty(filter.Name), a=> a.Name != null && a.Name.Contains(filter.Name))
            .WhereIf(filter !=null && !string.IsNullOrEmpty(filter.Code), a=> a.Code != null && a.Code.Contains(filter.Code))
            .Count(out var total)
            .OrderByDescending(c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<DevProjectModelGetPageOutput>();
    

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

        var data = new PageOutput<DevProjectModelGetPageOutput> { List = list, Total = total };
    
        return data;
    }
    

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(DevProjectModelAddInput input)
    {
        var entity = Mapper.Map<DevProjectModelEntity>(input);
        var id = (await _devProjectModelRep.InsertAsync(entity)).Id;

        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(DevProjectModelUpdateInput input)
    {
        var entity = await _devProjectModelRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("项目模型不存在！");
        }

        Mapper.Map(input, entity);
        await _devProjectModelRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _devProjectModelRep.DeleteAsync(id) > 0;
    }


    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> BatchDeleteAsync(long[] ids)
    {
        return await _devProjectModelRep.Where(w=>ids.Contains(w.Id)).ToDelete().ExecuteAffrowsAsync() > 0;
    }

    /// <summary>
    /// 软删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> SoftDeleteAsync(long id)
    {
        return await _devProjectModelRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量软删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> BatchSoftDeleteAsync(long[] ids)
    {
        return await _devProjectModelRep.SoftDeleteAsync(ids);
    }
}