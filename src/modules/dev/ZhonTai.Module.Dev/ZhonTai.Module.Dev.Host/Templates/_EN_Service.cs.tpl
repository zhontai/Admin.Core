@{
    var gen = Model as ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    if (gen.Fields == null) return;
    if (gen.Fields.Count() == 0) return;

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

using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;


using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Domain.Dict;

using @(gen.Namespace).Domain.@(entityNamePc);
using @(gen.Namespace).Services.@(entityNamePc).Dto;
using @(gen.Namespace).Core.Consts;


namespace @(gen.Namespace).Services.@(entityNamePc)
{
    /// <summary>
    /// @(gen.BusName)服务
    /// </summary>
    [DynamicApi(Area = @(moduleNamePc)Consts.AreaName)]
    public partial class @(entityNamePc)Service : BaseService, I@(entityNamePc)Service, IDynamicApi
    {
        private I@(entityNamePc)Repository _@(entityNameCc)Repository => LazyGetRequiredService<I@(entityNamePc)Repository>();

        public @(entityNamePc)Service()
        {
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<@(entityNamePc)GetOutput> GetAsync(long id)
        {
            var output = await _@(entityNameCc)Repository.GetAsync<@(entityNamePc)GetOutput>(id);
            return output;
        }
        
@if(gen.GenGetList){
        @:/// <summary>
        @:/// 列表查询
        @:/// </summary>
        @:/// <param name="input"></param>
        @:/// <returns></returns>
        @:[HttpPost]
        @:public async Task<IEnumerable<@(entityNamePc)GetListOutput>> GetListAsync(@(entityNamePc)GetListInput input)
        @:{
        @:    var list = await _@(entityNameCc)Repository.Select
            @foreach(var col in gen.Fields.Where(w=>w.WhetherQuery)){
                if(col.IsTextColumn()){
                @:.WhereIf(!string.IsNullOrEmpty(input.@(col.ColumnName.NamingPascalCase())), a=>a.@(col.ColumnName.NamingPascalCase()) == input.@(col.ColumnName.NamingPascalCase()))
                }else{
                @:.WhereIf(input.@(col.ColumnName.NamingPascalCase()) != null, a=>a.@(col.ColumnName.NamingPascalCase()) == input.@(col.ColumnName.NamingPascalCase()))
                }
            }
         @:       .OrderByDescending(a => a.Id)
         @:       .ToListAsync<@(entityNamePc)GetListOutput>();

            @if(gen.Fields.Any(a=>!string.IsNullOrWhiteSpace(a.DictTypeCode))) {

                            var usedDicCols = gen.Fields.Where(w => !string.IsNullOrWhiteSpace(w.DictTypeCode));

            @:var dictRepo = LazyGetRequiredService<IDictRepository>();
            @:var dictList = await dictRepo.Where(w => new string[] { @(string.Concat("\"" , string.Join("\", \"", usedDicCols.Select(s=>s.DictTypeCode)), "\"")) }
            @:    .Contains(w.DictType.Code)).ToListAsync();


            @:return list.Select(s =>
            @:{
                            foreach(var col in usedDicCols)
                            {

            @:    s.@(col.ColumnName.NamingPascalCase())DictName = dictList.FirstOrDefault(f => f.DictType.Code == "@(col.DictTypeCode)" && f.Value == @if(col.IsNumColumn())@("\"\" + ")s.@(col.ColumnName.NamingPascalCase()))?.Name;
                                
                            }
            @:   return s;
            @:});

            }else
            {
            @:return list;
            }
        @:}
}
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageOutput<@(entityNamePc)GetPageOutput>> GetPageAsync(PageInput<@(entityNamePc)GetPageInput> input)
        {
            var filter = input.Filter;
            var list = await _@(entityNameCc)Repository.Select
                .WhereDynamicFilter(input.DynamicFilter)
            @foreach(var col in gen.Fields.Where(w=>w.WhetherQuery)){
                if(col.IsTextColumn()){
                    if(col.IsTextQueryContains()){
                @:.WhereIf(filter !=null && !string.IsNullOrEmpty(filter.@(col.ColumnName.NamingPascalCase())), a=> a.@(col.ColumnName.NamingPascalCase()) != null && a.@(col.ColumnName.NamingPascalCase()).Contains(filter.@(col.ColumnName.NamingPascalCase())))
                    }else{
                @:.WhereIf(filter !=null && !string.IsNullOrEmpty(filter.@(col.ColumnName.NamingPascalCase())), a=>a.@(col.ColumnName.NamingPascalCase()) == filter.@(col.ColumnName.NamingPascalCase()))
                    }

                }else if(col.IsNumColumn()){
                @:.WhereIf(filter !=null && filter.@(col.ColumnName.NamingPascalCase()) != null, a=>a.@(col.ColumnName.NamingPascalCase()) == filter.@(col.ColumnName.NamingPascalCase()))
                }
            }
                .Count(out var total)
                .OrderByDescending(c => c.Id)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<@(entityNamePc)GetPageOutput>();
        
            @if(gen.Fields.Any(a=>!string.IsNullOrWhiteSpace(a.DictTypeCode))) {

                            var usedDicCols = gen.Fields.Where(w => !string.IsNullOrWhiteSpace(w.DictTypeCode));

            @:var dictRepo = LazyGetRequiredService<IDictRepository>();
            @:var dictList = await dictRepo.Where(w => new string[] { @(string.Concat("\"" , string.Join("\", \"", usedDicCols.Select(s=>s.DictTypeCode)), "\"")) }
            @:    .Contains(w.DictType.Code)).ToListAsync();

            @:
            @:list = list.Select(s =>
            @:{
                            foreach(var col in usedDicCols)
                            {

            @:    s.@(col.ColumnName.NamingPascalCase())DictName = dictList.FirstOrDefault(f => f.DictType.Code == "@(col.DictTypeCode)" && f.Value == @if(col.IsNumColumn())@("\"\" + ")s.@(col.ColumnName.NamingPascalCase()))?.Name;
                                
                            }
            @:
            @:   return s;
            @:}).ToList();

            }

            //关联查询代码
            @foreach (var col in gen.Fields.Where(w=>w.IsIncludeColumn()&&!string.IsNullOrWhiteSpace(w.IncludeEntityKey))){
                if(col.IncludeMode==0){
            @://数据转换-单个关联
            @:var @(col.ColumnName.NamingCamelCase())Rows = list.Where(s => s.@(col.ColumnName) > 0).ToList();
            @:if (@(col.ColumnName.NamingCamelCase())Rows.Any())
            @:{
            @:    var @(col.ColumnName.NamingCamelCase())Repo = LazyGetRequiredService<Domain.@(col.IncludeEntity.Replace("Entity", "")).I@(col.IncludeEntity.Replace("Entity", ""))Repository>();
            @:    var @(col.ColumnName.NamingCamelCase())RowsIds = @(col.ColumnName.NamingCamelCase())Rows.Select(s => s.@(col.ColumnName)).Distinct().ToList();
            @:    var @(col.ColumnName.NamingCamelCase())RowsIdsData = await @(col.ColumnName.NamingCamelCase())Repo.Where(s => @(col.ColumnName.NamingCamelCase())RowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.@(col.IncludeEntityKey) });
            @:    @(col.ColumnName.NamingCamelCase())Rows.ForEach(s =>
            @:    {
            @:        s.@(col.ColumnName)_Text = @(col.ColumnName.NamingCamelCase())RowsIdsData.FirstOrDefault(s2 => s2.Id == s.@(col.ColumnName))?.@(col.IncludeEntityKey);
            @:    });
            @:}
            }else if(col.IncludeMode==1){
                
            @://数据转换-多个关联
            @:var @(col.ColumnName.NamingCamelCase())Rows = list.Where(s => s.@(col.ColumnName)_Values != null && s.@(col.ColumnName)_Values.Any()).ToList();
            @:if (@(col.ColumnName.NamingCamelCase())Rows.Any())
            @:{
            @:    var @(col.ColumnName.NamingCamelCase())Repo = LazyGetRequiredService<Domain.@(col.IncludeEntity.Replace("Entity", "")).I@(col.IncludeEntity.Replace("Entity", ""))Repository>();
            @:    var @(col.ColumnName.NamingCamelCase())RowsIds =@(col.ColumnName.NamingCamelCase())Rows.SelectMany(s => s.@(col.ColumnName)_Values).Select(s => long.TryParse(s, out long s2) ? s2 : 0).Distinct().ToList();
            @:    var @(col.ColumnName.NamingCamelCase())RowsIdsData = await @(col.ColumnName.NamingCamelCase())Repo.Where(s => @(col.ColumnName.NamingCamelCase())RowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.@(col.IncludeEntityKey) });
            @:    @(col.ColumnName.NamingCamelCase())Rows.ForEach(s =>
            @:    {
            @:        s.@(col.ColumnName)_Texts = @(col.ColumnName.NamingCamelCase())RowsIdsData.Where(s2 => s.@(col.ColumnName)_Values.Contains(s2.Id.ToString())).OrderBy(s2 => s.@(col.ColumnName)_Values.IndexOf(s2.Id.ToString())).Select(s2 => s2.@(col.IncludeEntityKey)).ToList();
            @:    });
            @:}
            }

            }

            var data = new PageOutput<@(entityNamePc)GetPageOutput> { List = list, Total = total };
        
            return data;
        }
        

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> AddAsync(@(entityNamePc)AddInput input)
        {
            var entity = Mapper.Map<@(entityNamePc)Entity>(input);
            var id = (await _@(entityNameCc)Repository.InsertAsync(entity)).Id;

            return id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateAsync(@(entityNamePc)UpdateInput input)
        {
            var entity = await _@(entityNameCc)Repository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                throw ResultOutput.Exception("@(gen.BusName)不存在！");
            }

            Mapper.Map(input, entity);
            await _@(entityNameCc)Repository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(long id)
        {
            return await _@(entityNameCc)Repository.DeleteAsync(id) > 0;
        }


@if(gen.GenBatchDelete){
        @:/// <summary>
        @:/// 批量删除
        @:/// </summary>
        @:/// <param name="ids"></param>
        @:/// <returns></returns>
        @:[HttpPut]
        @:public async Task<bool> BatchDeleteAsync(long[] ids)
        @:{
        @:    return await _@(entityNameCc)Repository.Where(w=>ids.Contains(w.Id)).ToDelete().ExecuteAffrowsAsync() > 0;
        @:}
}

@if(gen.GenSoftDelete){
        @:/// <summary>
        @:/// 软删除
        @:/// </summary>
        @:/// <param name="id"></param>
        @:/// <returns></returns>
        @:[HttpDelete]
        @:public async Task<bool> SoftDeleteAsync(long id)
        @:{
        @:    return await _@(entityNameCc)Repository.SoftDeleteAsync(id);
        @:}
}

@if (gen.GenBatchSoftDelete)
{
        @:/// <summary>
        @:/// 批量软删除
        @:/// </summary>
        @:/// <param name="ids"></param>
        @:/// <returns></returns>
        @:[HttpPut]
        @:public async Task<bool> BatchSoftDeleteAsync(long[] ids)
        @:{
        @:    return await _@(entityNameCc)Repository.SoftDeleteAsync(ids);
        @:}

}
    }
}