@{
    var gen = Model as ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen.CodeGenEntity;
    if (gen?.Fields?.Any() != true) return;
    
    var entityNamePc = gen.EntityName?.NamingPascalCase();
    var entityNameCc = gen.EntityName?.NamingCamelCase();
    var moduleNamePc = gen.ApiAreaName?.NamingPascalCase();
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Domain.Dict;
using @(gen.Namespace).Api.Contracts.Services.@(entityNamePc);
using @(gen.Namespace).Api.Core.Consts;
using @(gen.Namespace).Api.Core.Repositories;
using @(gen.Namespace).Api.Contracts.Domain.@(entityNamePc);
using @(gen.Namespace).Api.Contracts.Services.@(entityNamePc).Dtos;

namespace @(gen.Namespace).Api.Services.@(entityNamePc);

/// <summary>
/// @(gen.BusName)服务
/// </summary>
[DynamicApi(Area = ApiConsts.AreaName)]
public partial class @(entityNamePc)Service : BaseService, I@(entityNamePc)Service, IDynamicApi
{
    private readonly AppRepositoryBase<@(entityNamePc)Entity> _@(entityNameCc)Repo;
    public @(entityNamePc)Service(AppRepositoryBase<@(entityNamePc)Entity> @(entityNameCc)Repo)
    {
        _@(entityNameCc)Repo = @(entityNameCc)Repo;
    }
@if (gen.GenGet)
{
@:
    @:/// <summary>
    @:/// 查询单条记录
    @:/// </summary>
    @:/// <param name="id">主键ID</param>
    @:/// <returns>@(gen.BusName)信息</returns>
    @:[HttpGet]
    @:public async Task<@(entityNamePc)GetOutput> GetAsync(long id)
    @:{ 
        @:var output = await _@(entityNameCc)Repo.GetAsync<@(entityNamePc)GetOutput>(id);
        @:return output;
    @:}
}    
@if (gen.GenGetList)
{
@:
    @:/// <summary>
    @:/// 列表查询
    @:/// </summary>
    @:/// <param name="input">查询条件</param>
    @:/// <returns>@(gen.BusName)列表</returns>
    @:[HttpPost]
    @:public async Task<IEnumerable<@(entityNamePc)GetListOutput>> GetListAsync(@(entityNamePc)GetListInput input)
    @:{
    @:    var query = _@(entityNameCc)Repo.Select;
            
foreach (var col in gen.Fields.Where(w => w.WhetherQuery))
{
    if (col.IsTextColumn())
    {
        @:query = query.WhereIf(!string.IsNullOrEmpty(input.@(col.ColumnName.NamingPascalCase())), a => a.@(col.ColumnName.NamingPascalCase()) == input.@(col.ColumnName.NamingPascalCase()));
    }
    else
    {
        @:query = query.WhereIf(input.@(col.ColumnName.NamingPascalCase()) != null, a => a.@(col.ColumnName.NamingPascalCase()) == input.@(col.ColumnName.NamingPascalCase()));
    }
}

        @:var list = await query
            @:.OrderByDescending(a => a.Id)
            @:.ToListAsync<@(entityNamePc)GetListOutput>();

if (gen.Fields.Any(a => !string.IsNullOrWhiteSpace(a.DictTypeCode)))
{
    var dictCols = gen.Fields.Where(w => !string.IsNullOrWhiteSpace(w.DictTypeCode));
        @:// 处理字典字段显示名称
        @:if (list.Any())
        @:{
            @:var cloud = LazyGetRequiredService<FreeSqlCloud>();
            @:var adminDb = cloud.Use(AdminDbKeys.AdminDb);
            @:var dictRepo = adminDb.GetRepoBase<DictEntity>();
            @:var dictTypeCodes = new[] { @string.Join(", ", dictCols.Select(s => $"\"{s.DictTypeCode}\"")) };
            @:var dictList = await dictRepo.Where(w => dictTypeCodes.Contains(w.DictType.Code)).ToListAsync();
@:
            @:list = list.Select(s =>
            @:{
    foreach (var col in dictCols)
    {
                @:s.@(col.ColumnName.NamingPascalCase())DictName = dictList.FirstOrDefault(f => f.DictType.Code == "@col.DictTypeCode" && f.Value == @(col.IsNumColumn() ? "\"\" + " : "")s.@(col.ColumnName.NamingPascalCase()))?.Name;
    }
                @:return s;
            @:}).ToList();
        @:}
}

        @:return list;
    @:}
}
@if (gen.GenGetPage)
{
@:
    @:/// <summary>
    @:/// 分页查询
    @:/// </summary>
    @:/// <param name="input">分页查询条件</param>
    @:/// <returns>分页结果</returns>
    @:[HttpPost]
    @:public async Task<PageOutput<@(entityNamePc)GetPageOutput>> GetPageAsync(PageInput<@(entityNamePc)GetPageInput> input)
    @:{
    @:    var filter = input.Filter;
    @:    var query = _@(entityNameCc)Repo.Select
    @:        .WhereDynamicFilter(input.DynamicFilter);

@foreach (var col in gen.Fields.Where(w => w.WhetherQuery))
{
    if (col.IsTextColumn())
    {
        if (col.IsTextQueryContains())
        {
        @:query = query.WhereIf(filter != null && !string.IsNullOrEmpty(filter.@(col.ColumnName.NamingPascalCase())), a => a.@(col.ColumnName.NamingPascalCase()) != null && a.@(col.ColumnName.NamingPascalCase()).Contains(filter.@(col.ColumnName.NamingPascalCase())));
    }
    else
    {
        @:query = query.WhereIf(filter != null && !string.IsNullOrEmpty(filter.@(col.ColumnName.NamingPascalCase())), a => a.@(col.ColumnName.NamingPascalCase()) == filter.@(col.ColumnName.NamingPascalCase()));
    }
}
else if (col.IsNumColumn())
{
        @:query = query.WhereIf(filter != null && filter.@(col.ColumnName.NamingPascalCase()) != null, a => a.@(col.ColumnName.NamingPascalCase()) == filter.@(col.ColumnName.NamingPascalCase()));
}
}

    @:    var total = 0L;
    @:    var list = await query
    @:        .Count(out total)
    @:        .OrderByDescending(c => c.Id)
    @:        .Page(input.CurrentPage, input.PageSize)
    @:        .ToListAsync<@(entityNamePc)GetPageOutput>();
@if (gen.Fields.Any(a => !string.IsNullOrWhiteSpace(a.DictTypeCode)))
{
        var dictCols = gen.Fields.Where(w => !string.IsNullOrWhiteSpace(w.DictTypeCode));
@:
      @:// 处理字典字段显示名称
      @:if (list.Any())
      @:{
        @:var cloud = LazyGetRequiredService<FreeSqlCloud>();
        @:var adminDb = cloud.Use(AdminDbKeys.AdminDb);
        @:var dictRepo = adminDb.GetRepoBase<DictEntity>();
        @:var dictTypeCodes = new[] { @string.Join(", ", dictCols.Select(s => $"\"{s.DictTypeCode}\"")) };
        @:var dictList = await dictRepo.Where(w => dictTypeCodes.Contains(w.DictType.Code)).ToListAsync();
@:
        @:list = list.Select(s =>
            @:{
foreach (var col in dictCols)
{
                @:s.@(col.ColumnName.NamingPascalCase())DictName = dictList.FirstOrDefault(f => f.DictType.Code == "@col.DictTypeCode" && f.Value == @(col.IsNumColumn() ? "\"\" + " : "")s.@(col.ColumnName.NamingPascalCase()))?.Name;
}
                @:return s;
            @:}).ToList();
        @:}
}
@if (gen.Fields.Any(w => w.IsIncludeColumn() && !string.IsNullOrWhiteSpace(w.IncludeEntityKey)))
{
var includeCols = gen.Fields.Where(w => w.IsIncludeColumn() && !string.IsNullOrWhiteSpace(w.IncludeEntityKey));
foreach (var col in includeCols)
{
    if (col.IncludeMode == 0)
    {
@:
        @:// 处理单个关联字段: @col.ColumnName
        @:var @(col.ColumnName.NamingCamelCase())Rows = list.Where(s => s.@(col.ColumnName) > 0).ToList();
        @:if (@(col.ColumnName.NamingCamelCase())Rows.Any())
        @:{
            @:var @(col.ColumnName.NamingCamelCase())Repo = LazyGetRequiredService<Domain.@(col.IncludeEntity.Replace("Entity", "")).I@(col.IncludeEntity.Replace("Entity", ""))Repo>();
            @:var @(col.ColumnName.NamingCamelCase())Ids = @(col.ColumnName.NamingCamelCase())Rows.Select(s => s.@(col.ColumnName)).Distinct().ToList();
            @:var @(col.ColumnName.NamingCamelCase())Data = await @(col.ColumnName.NamingCamelCase())Repo.Where(s => @(col.ColumnName.NamingCamelCase())Ids.Contains(s.Id)).ToListAsync(s => new { s.Id, s.@(col.IncludeEntityKey) });
        @:
            @:@(col.ColumnName.NamingCamelCase())Rows.ForEach(s =>
            @:{
                @:s.@(col.ColumnName)_Text = @(col.ColumnName.NamingCamelCase())Data.FirstOrDefault(s2 => s2.Id == s.@(col.ColumnName))?.@(col.IncludeEntityKey);
            @:});
        @:}
    }
    else if (col.IncludeMode == 1)
    {
        @:// 处理多个关联字段: @col.ColumnName
        @:var @(col.ColumnName.NamingCamelCase())Rows = list.Where(s => s.@(col.ColumnName)_Values != null && s.@(col.ColumnName)_Values.Any()).ToList();
        @:if (@(col.ColumnName.NamingCamelCase())Rows.Any())
        @:{
            @:var @(col.ColumnName.NamingCamelCase())Repo = LazyGetRequiredService<Domain.@(col.IncludeEntity.Replace("Entity", "")).I@(col.IncludeEntity.Replace("Entity", ""))Repo>();
            @:var @(col.ColumnName.NamingCamelCase())Ids = @(col.ColumnName.NamingCamelCase())Rows.SelectMany(s => s.@(col.ColumnName)_Values).Select(s => long.TryParse(s, out long result) ? result : 0).Where(id => id > 0).Distinct().ToList();
        @:
            @:var @(col.ColumnName.NamingCamelCase())Data = await @(col.ColumnName.NamingCamelCase())Repo.Where(s => @(col.ColumnName.NamingCamelCase())Ids.Contains(s.Id)).ToListAsync(s => new { s.Id, s.@(col.IncludeEntityKey) });
        @:
            @:@(col.ColumnName.NamingCamelCase())Rows.ForEach(s =>
            @:{
                @:s.@(col.ColumnName)_Texts = @(col.ColumnName.NamingCamelCase())Data.Where(s2 => s.@(col.ColumnName)_Values.Contains(s2.Id.ToString())).OrderBy(s2 => s.@(col.ColumnName)_Values.IndexOf(s2.Id.ToString())).Select(s2 => s2.@(col.IncludeEntityKey)).ToList();
            @:});
        @:}
    }
}
}

    @:    var data = new PageOutput<@(entityNamePc)GetPageOutput> 
    @:    { 
    @:        List = list, 
    @:        Total = total 
    @:    };
    @:    
    @:    return data;
    @:}
}
@if (gen.GenAdd)
{
@:
    @:/// <summary>
    @:/// 新增
    @:/// </summary>
    @:/// <param name="input">新增数据</param>
    @:/// <returns>新增记录ID</returns>
    @:[HttpPost]
    @:public async Task<long> AddAsync(@(entityNamePc)AddInput input)
    @:{
    @:    var entity = Mapper.Map<@(entityNamePc)Entity>(input);
    @:    var id = (await _@(entityNameCc)Repo.InsertAsync(entity)).Id;
    @:    return id;
    @:}
}
@if (gen.GenUpdate)
{
@:
    @:/// <summary>
    @:/// 更新
    @:/// </summary>
    @:/// <param name="input">更新数据</param>
    @:/// <returns></returns>
    @:[HttpPut]
    @:public async Task UpdateAsync(@(entityNamePc)UpdateInput input)
    @:{
    @:    var entity = await _@(entityNameCc)Repo.GetAsync(input.Id);
    @:    if (entity?.Id <= 0)
    @:    {
    @:        throw ResultOutput.Exception("@(gen.BusName)不存在");
    @:    }

    @:    Mapper.Map(input, entity);
    @:    await _@(entityNameCc)Repo.UpdateAsync(entity);
    @:}
}
@if (gen.GenDelete)
{
@:
    @:/// <summary>
    @:/// 删除
    @:/// </summary>
    @:/// <param name="id">主键ID</param>
    @:/// <returns>是否成功</returns>
    @:[HttpDelete]
    @:public async Task<bool> DeleteAsync(long id)
    @:{
    @:    return await _@(entityNameCc)Repo.DeleteAsync(id) > 0;
    @:}
}
@if (gen.GenBatchDelete)
{
@:
    @:/// <summary>
    @:/// 批量删除
    @:/// </summary>
    @:/// <param name="ids">主键ID数组</param>
    @:/// <returns>是否成功</returns>
    @:[HttpPut]
    @:public async Task<bool> BatchDeleteAsync(long[] ids)
    @:{
    @:    return await _@(entityNameCc)Repo.Where(w => ids.Contains(w.Id)).ToDelete().ExecuteAffrowsAsync() > 0;
    @:}
}
@if (gen.GenSoftDelete)
{
@:
    @:/// <summary>
    @:/// 软删除
    @:/// </summary>
    @:/// <param name="id">主键ID</param>
    @:/// <returns>是否成功</returns>
    @:[HttpDelete]
    @:public async Task<bool> SoftDeleteAsync(long id)
    @:{
    @:    return await _@(entityNameCc)Repo.SoftDeleteAsync(id);
    @:}
}
@if (gen.GenBatchSoftDelete)
{
@:
    @:/// <summary>
    @:/// 批量软删除
    @:/// </summary>
    @:/// <param name="ids">主键ID数组</param>
    @:/// <returns>是否成功</returns>
    @:[HttpPut]
    @:public async Task<bool> BatchSoftDeleteAsync(long[] ids)
    @:{
    @:    return await _@(entityNameCc)Repo.SoftDeleteAsync(ids);
    @:}
}
}