@{
    var gen = Model as ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    if (gen.Fields == null) return;
    if (gen.Fields.Count() == 0) return;

    var entityNamePc = gen.EntityName.NamingPascalCase();
}
using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
@foreach (var ns in gen.GetUsings())
{
@:using @(ns);    
}
using @(gen.Namespace).Services.@(entityNamePc).Dto;

namespace @(gen.Namespace).Services.@(entityNamePc)
{
    /// <summary>
    /// @(gen.BusName)服务
    /// </summary>
    public interface I@(entityNamePc)Service
    {
        /// <summary>
        /// 查询
        /// </summary>
        Task<@(entityNamePc)GetOutput> GetAsync(long id);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        Task<PageOutput<@(entityNamePc)GetPageOutput>> GetPageAsync(PageInput<@(entityNamePc)GetPageInput> input);
        
    @if(gen.GenGetList){
        @:
        @:/// <summary>
        @:/// 列表查询
        @:/// </summary>
        @:Task<IEnumerable<@(entityNamePc)GetListOutput>> GetListAsync(@(entityNamePc)GetListInput input);
    }
    
        /// <summary>
        /// 新增
        /// </summary>
        Task<long> AddAsync(@(entityNamePc)AddInput input);
        
        /// <summary>
        /// 编辑
        /// </summary>
        Task UpdateAsync(@(entityNamePc)UpdateInput input);
        
        /// <summary>
        /// 删除
        /// </summary>
        Task<bool> DeleteAsync(long id);

    @if(gen.GenBatchDelete){
        @:/// <summary>
        @:/// 批量删除
        @:/// </summary>
        @:Task<bool> BatchDeleteAsync(long[] ids);
    }
    @if(gen.GenSoftDelete){
        @:/// <summary>
        @:/// 软删除
        @:/// </summary>
        @:Task<bool> SoftDeleteAsync(long id);
    }
    @if(gen.GenBatchSoftDelete){
        @:/// <summary>
        @:/// 批量软删除
        @:/// </summary>
        @:Task<bool> BatchSoftDeleteAsync(long[] ids);
    }
    }
}

namespace @(gen.Namespace).Services.@(entityNamePc).Dto
{
    
@if(gen.GenGetList){
    @:/// <summary>@(gen.BusName)列表查询结果输出</summary>
    @:public partial class @(entityNamePc)GetListOutput {
        @if (!String.IsNullOrWhiteSpace(gen.BaseEntity))
        {
        @:public long Id { get; set; }
        @:public DateTime CreatedTime { get; set; }
        @:public string CreatedUserName { get; set; }
        @:public string ModifiedUserName { get; set; }
        @:public DateTime? ModifiedTime { get; set; }
        }
        @foreach(var col in gen.Fields.Where(w=>w.WhetherList))
        {
            if (!col.IsIgnoreColumn())
            {
        @:/// <summary>@(col.Title)</summary>
        @:@col.PropCsByOutput()
            }

        ///    if (col.IsIncludeColumn())
        ///    {
        ///@:@col.PropIncludeCs()
        ///    }

            if (!string.IsNullOrWhiteSpace(col.DictTypeCode))
            {
        @:/// <summary>@(col.Title)名称</summary>
        @:public string? @(col.ColumnName)DictName { get; set; }
            }
        }
    @:}

    @:/// <summary>@(gen.BusName)列表查询条件输入</summary>
    @:public partial class @(entityNamePc)GetListInput : @(entityNamePc)GetPageInput {

    @:}
}

    /// <summary>@(gen.BusName)查询结果输出</summary>
    public partial class @(entityNamePc)GetOutput {
        @if (!String.IsNullOrWhiteSpace(gen.BaseEntity))
        {
        @:public long Id { get; set; }
        }
        @foreach (var col in gen.Fields)
        {

            if (!col.IsIgnoreColumn())
            {
        @:/// <summary>@(col.Title)</summary>
        @:@col.PropCsByOutput()
            }

        ///    if (col.IsIncludeColumn())
        ///    {
        ///@:@col.PropIncludeCs()
        ///    }
        }
    }

    /// <summary>@(gen.BusName)分页查询结果输出</summary>
    public partial class @(entityNamePc)GetPageOutput {
        @if (!String.IsNullOrWhiteSpace(gen.BaseEntity))
        {

        @:public long Id { get; set; }
        @:public DateTime CreatedTime { get; set; }
        @:public string CreatedUserName { get; set; }
        @:public string ModifiedUserName { get; set; }
        @:public DateTime? ModifiedTime { get; set; }

        }
        @foreach(var col in gen.Fields.Where(w=>w.WhetherTable))
        {

            if(!col.IsIgnoreColumn())
            {
        @:/// <summary>@(col.Title)</summary>
        @:@col.PropCsByOutput()
            }

        ///    if (col.IsIncludeColumn())
        ///    {
        ///@:@col.PropIncludeCs()
        ///    }

            if(!string.IsNullOrWhiteSpace(col.DictTypeCode))
            {
        @:/// <summary>@(col.Title)名称</summary>
        @:public string? @(col.ColumnName)DictName { get; set; }
            }
        }
    }

    /// <summary>@(gen.BusName)分页查询条件输入</summary>
    public partial class @(entityNamePc)GetPageInput {

        @foreach (var col in gen.Fields.Where(w=>w.WhetherQuery))
        {
            if(!col.IsIgnoreColumn())
            {
        @:/// <summary>@(col.Title)</summary>       
        @:@col.PropCsByInput(true)
            }
        }
    }
    
@if(gen.GenAdd){
@:    /// <summary>@(gen.BusName)新增输入</summary>
@:    public partial class @(entityNamePc)AddInput {
        @foreach (var col in gen.Fields.Where(w=>w.WhetherAdd))
        {
            if (!col.IsIgnoreColumn())
            {
@:        /// <summary>@(col.Title)</summary>
                if (!col.IsNullable)
                {
@:        [Required(ErrorMessage = "@((!String.IsNullOrEmpty(col.Title)?col.Title:col.ColumnName)+"不能为空")")]
                }
@:        @col.PropCsByInput()                                                    
             }
        }
@:    }
}


    /// <summary>@(gen.BusName)更新数据输入</summary>
    public partial class @(entityNamePc)UpdateInput {
    @if (!String.IsNullOrWhiteSpace(gen.BaseEntity))
    {
        @:public long Id { get; set; }
    }
    @foreach (var col in gen.Fields.Where(w => w.WhetherUpdate))
    {
        if (!col.IsIgnoreColumn())
        {
        @:/// <summary>@(col.Title)</summary>
        if (!col.IsNullable)
        {
        @:[Required(ErrorMessage = "@((!String.IsNullOrEmpty(col.Title) ? col.Title : col.ColumnName) + "不能为空")")]
        }
        @:@col.PropCsByInput()
        }
    }
    }


}