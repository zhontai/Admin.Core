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
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevProjectModel;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevProjectModelField;
using ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectModelField.Dtos;
using ZhonTai.Module.Dev.Api.Core.Consts;
using ZhonTai.Module.Dev.Api.Core.Repositories;

namespace ZhonTai.Module.Dev.Services.DevProjectModelField;

/// <summary>
/// 项目模型字段服务
/// </summary>
[DynamicApi(Area = ApiConsts.AreaName)]
public partial class DevProjectModelFieldService : BaseService, IDynamicApi
{
    private readonly AppRepositoryBase<DevProjectModelFieldEntity> _devProjectModelFieldRep;
    private readonly AppRepositoryBase<DevProjectModelEntity> _devProjectModelRep;

    public DevProjectModelFieldService(AppRepositoryBase<DevProjectModelFieldEntity> devProjectModelFieldRep,
        AppRepositoryBase<DevProjectModelEntity> devProjectModelRep)
    {
        _devProjectModelFieldRep = devProjectModelFieldRep;
        _devProjectModelRep = devProjectModelRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<DevProjectModelFieldGetOutput> GetAsync(long id)
    {
        var output = await _devProjectModelFieldRep.GetAsync<DevProjectModelFieldGetOutput>(id);
        return output;
    }
    
    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IEnumerable<DevProjectModelFieldGetListOutput>> GetListAsync(DevProjectModelFieldGetListInput input)
    {
        var list = await _devProjectModelFieldRep.Select
            .WhereIf(input.ModelId != null, a=>a.ModelId == input.ModelId)
            .WhereIf(!string.IsNullOrEmpty(input.Name), a=>a.Name == input.Name)
            .OrderByDescending(a => a.Id)
            .ToListAsync<DevProjectModelFieldGetListOutput>();
        return list;
        //var dictRepo = LazyGetRequiredService<IDictRepository>();
        //var dictList = await dictRepo.Where(w => new string[] { "fieldType", "fieldProperties" }
        //    .Contains(w.DictType.Code)).ToListAsync();
        //return list.Select(s =>
        //{
        //    s.DataTypeDictName = dictList.FirstOrDefault(f => f.DictType.Code == "fieldType" && f.Value == s.DataType)?.Name;
        //    s.PropertiesDictName = dictList.FirstOrDefault(f => f.DictType.Code == "fieldProperties" && f.Value == s.Properties)?.Name;
        //   return s;
        //});
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<DevProjectModelFieldGetPageOutput>> GetPageAsync(PageInput<DevProjectModelFieldGetPageInput> input)
    {
        var filter = input.Filter;
        var list = await _devProjectModelFieldRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .WhereIf(filter !=null && filter.ModelId != null, a=>a.ModelId == filter.ModelId)
            .WhereIf(filter !=null && !string.IsNullOrEmpty(filter.Name), a=>a.Name == filter.Name)
            .Count(out var total)
            .OrderByDescending(c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<DevProjectModelFieldGetPageOutput>();
    
        //var dictRepo = LazyGetRequiredService<IDictRepository>();
        //var dictList = await dictRepo.Where(w => new string[] { "fieldType", "fieldProperties" }
        //    .Contains(w.DictType.Code)).ToListAsync();
        
        //list = list.Select(s =>
        //{
        //    s.DataTypeDictName = dictList.FirstOrDefault(f => f.DictType.Code == "fieldType" && f.Value == s.DataType)?.Name;
        //    s.PropertiesDictName = dictList.FirstOrDefault(f => f.DictType.Code == "fieldProperties" && f.Value == s.Properties)?.Name;
        
        //   return s;
        //}).ToList();

        //关联查询代码
        //数据转换-单个关联
        var modelIdRows = list.Where(s => s.ModelId > 0).ToList();
        if (modelIdRows.Any())
        {
            var modelIdRowsIds = modelIdRows.Select(s => s.ModelId).Distinct().ToList();
            var modelIdRowsIdsData = await _devProjectModelRep.Where(s => modelIdRowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.Name });
            modelIdRows.ForEach(s =>
            {
                s.ModelId_Text = modelIdRowsIdsData.FirstOrDefault(s2 => s2.Id == s.ModelId)?.Name;
            });
        }

        var data = new PageOutput<DevProjectModelFieldGetPageOutput> { List = list, Total = total };
    
        return data;
    }
    

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(DevProjectModelFieldAddInput input)
    {
        var entity = Mapper.Map<DevProjectModelFieldEntity>(input);
        var id = (await _devProjectModelFieldRep.InsertAsync(entity)).Id;

        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(DevProjectModelFieldUpdateInput input)
    {
        var entity = await _devProjectModelFieldRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("项目模型字段不存在！");
        }

        Mapper.Map(input, entity);
        await _devProjectModelFieldRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _devProjectModelFieldRep.DeleteAsync(id) > 0;
    }


    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> BatchDeleteAsync(long[] ids)
    {
        return await _devProjectModelFieldRep.Where(w=>ids.Contains(w.Id)).ToDelete().ExecuteAffrowsAsync() > 0;
    }

    /// <summary>
    /// 软删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> SoftDeleteAsync(long id)
    {
        return await _devProjectModelFieldRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量软删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> BatchSoftDeleteAsync(long[] ids)
    {
        return await _devProjectModelFieldRep.SoftDeleteAsync(ids);
    }
}