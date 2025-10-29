using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevGroup;
using ZhonTai.Module.Dev.Api.Contracts.Services.DevGroup.Dtos;
using ZhonTai.Module.Dev.Api.Core.Consts;
using ZhonTai.Module.Dev.Api.Core.Repositories;

namespace ZhonTai.Module.Dev.Services.DevGroup;

/// <summary>
/// 模板组服务
/// </summary>
[DynamicApi(Area = ApiConsts.AreaName)]
public partial class DevGroupService : BaseService, IDynamicApi
{
    private readonly AppRepositoryBase<DevGroupEntity> _devGroupRep;

    public DevGroupService(AppRepositoryBase<DevGroupEntity> devGroupRep)
    {
        _devGroupRep = devGroupRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<DevGroupGetOutput> GetAsync(long id)
    {
        var output = await _devGroupRep.GetAsync<DevGroupGetOutput>(id);
        return output;
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IEnumerable<DevGroupGetListOutput>> GetListAsync(DevGroupGetListInput input)
    {
        var list = await _devGroupRep.Select
            .WhereIf(input.Id > 0, a => a.Id == input.Id)
            .WhereIf(!string.IsNullOrEmpty(input.Name), a => a.Name == input.Name)
            .OrderByDescending(a => a.Id)
            .ToListAsync<DevGroupGetListOutput>();
        return list;
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<DevGroupGetPageOutput>> GetPageAsync(PageInput<DevGroupGetPageInput> input)
    {
        var filter = input.Filter;
        var list = await _devGroupRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .WhereIf(filter != null && !string.IsNullOrEmpty(filter.Name), a => a.Name != null && a.Name.Contains(filter.Name))
            .Count(out var total)
            .OrderByDescending(c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<DevGroupGetPageOutput>();


        //关联查询代码

        var data = new PageOutput<DevGroupGetPageOutput> { List = list, Total = total };

        return data;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(DevGroupAddInput input)
    {
        var entity = Mapper.Map<DevGroupEntity>(input);
        var id = (await _devGroupRep.InsertAsync(entity)).Id;

        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(DevGroupUpdateInput input)
    {
        var entity = await _devGroupRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("模板组不存在！");
        }

        Mapper.Map(input, entity);
        await _devGroupRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _devGroupRep.DeleteAsync(id) > 0;
    }


    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> BatchDeleteAsync(long[] ids)
    {
        return await _devGroupRep.Where(w => ids.Contains(w.Id)).ToDelete().ExecuteAffrowsAsync() > 0;
    }

    /// <summary>
    /// 软删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> SoftDeleteAsync(long id)
    {
        return await _devGroupRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量软删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> BatchSoftDeleteAsync(long[] ids)
    {
        return await _devGroupRep.SoftDeleteAsync(ids);
    }
}