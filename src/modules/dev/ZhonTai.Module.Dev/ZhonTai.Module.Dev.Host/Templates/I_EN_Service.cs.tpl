@{
    var gen = Model as ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen.CodeGenEntity;
    if (gen?.Fields?.Any() != true) return;
    
    var entityNamePc = gen.EntityName.NamingPascalCase();
    var hasBaseEntity = !string.IsNullOrWhiteSpace(gen.BaseEntity);
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Entities;
@foreach (var ns in gen.GetUsings())
{
@:using @ns;    
}
using @(gen.Namespace).Services.@(entityNamePc).Dtos;

namespace @(gen.Namespace).Services.@(entityNamePc)
{
    /// <summary>
    /// @(gen.BusName)服务接口
    /// </summary>
    public interface I@(entityNamePc)Service
    {
    @if (gen.GenGet)
    {
        <text>
        /// <summary>
        /// 查询单条记录
        /// </summary>
        Task<@(entityNamePc)GetOutput> GetAsync(long id);
        </text>
    }      
    @if (gen.GenGetPage)
    {
        <text>
        /// <summary>
        /// 分页查询
        /// </summary>
        Task<PageOutput<@(entityNamePc)GetPageOutput>> GetPageAsync(PageInput<@(entityNamePc)GetPageInput> input);
        </text>
    }        
    @if (gen.GenGetList)
    {
        <text>
        /// <summary>
        /// 列表查询
        /// </summary>
        Task<IEnumerable<@(entityNamePc)GetListOutput>> GetListAsync(@(entityNamePc)GetListInput input);
        </text>
    }
    @if (gen.GenAdd)
    {
        <text>
        /// <summary>
        /// 新增
        /// </summary>
        Task<long> AddAsync(@(entityNamePc)AddInput input);
        </text>
    }
    @if (gen.GenUpdate)
    {
        <text>
        /// <summary>
        /// 编辑
        /// </summary>
        Task UpdateAsync(@(entityNamePc)UpdateInput input);
        </text>
    }
    @if (gen.GenDelete)
    {
        <text>
        /// <summary>
        /// 删除
        /// </summary>
        Task<bool> DeleteAsync(long id);
        </text>
    }
    @if (gen.GenBatchDelete)
    {
        <text>
        /// <summary>
        /// 批量删除
        /// </summary>
        Task<bool> BatchDeleteAsync(long[] ids);
        </text>
    }
    @if (gen.GenSoftDelete)
    {
        <text>
        /// <summary>
        /// 软删除
        /// </summary>
        Task<bool> SoftDeleteAsync(long id);
        </text>
    }
    @if (gen.GenBatchSoftDelete)
    {
        <text>
        /// <summary>
        /// 批量软删除
        /// </summary>
        Task<bool> BatchSoftDeleteAsync(long[] ids);
        </text>
    }
    }
}

namespace @(gen.Namespace).Services.@(entityNamePc).Dtos
{
@if (gen.GenGetList)
{
    <text>
    /// <summary>
    /// @(gen.BusName)列表查询结果输出
    /// </summary>
    public partial class @(entityNamePc)GetListOutput 
    {
    @if (hasBaseEntity)
    {
        <text>
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        </text>
    }
    @foreach (var col in gen.Fields.Where(w => w.WhetherList))
    {
        if (!col.IsIgnoreColumn())
        {
        <text>
        /// <summary>
        /// @col.Title
        /// </summary>
        @col.PropCsByOutput()
        </text>
        }

        if (!string.IsNullOrWhiteSpace(col.DictTypeCode))
        {
        <text>
        /// <summary>
        /// @(col.Title)名称
        /// </summary>
        public string? @(col.ColumnName)DictName { get; set; }
        </text>
        }
    }
    }

    /// <summary>
    /// @(gen.BusName)列表查询条件输入
    /// </summary>
    public partial class @(entityNamePc)GetListInput : @(entityNamePc)GetPageInput 
    {
    }
    </text>
}

@if (gen.GenGet)
{
    <text>
    /// <summary>
    /// @(gen.BusName)查询结果输出
    /// </summary>
    public partial class @(entityNamePc)GetOutput 
    {
    @if (hasBaseEntity)
    {
        <text>
        public long Id { get; set; }
        </text>
    }
    @foreach (var col in gen.Fields)
    {
        if (!col.IsIgnoreColumn())
        {
        <text>
        /// <summary>
        /// @col.Title
        /// </summary>
        @col.PropCsByOutput()
        </text>
        }
    }
    }
    </text>
}

@if (gen.GenGetPage)
{
    <text>
    /// <summary>
    /// @(gen.BusName)分页查询结果输出
    /// </summary>
    public partial class @(entityNamePc)GetPageOutput 
    {
    @if (hasBaseEntity)
    {
        <text>
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        </text>
    }
    @foreach (var col in gen.Fields.Where(w => w.WhetherTable))
    {
        if (!col.IsIgnoreColumn())
        {
        <text>
        /// <summary>
        /// @col.Title
        /// </summary>
        @col.PropCsByOutput()
        </text>
        }

        if (!string.IsNullOrWhiteSpace(col.DictTypeCode))
        {
        <text>
        /// <summary>
        /// @(col.Title)名称
        /// </summary>
        public string? @(col.ColumnName)DictName { get; set; }
        </text>
        }
    }
    }

    /// <summary>
    /// @(gen.BusName)分页查询条件输入
    /// </summary>
    public partial class @(entityNamePc)GetPageInput 
    {
    @foreach (var col in gen.Fields.Where(w => w.WhetherQuery))
    {
        if (!col.IsIgnoreColumn())
        {
        <text>
        /// <summary>
        /// @col.Title
        /// </summary>       
        @col.PropCsByInput(true)
        </text>
        }
    }
    }
    </text>
}
    
@if (gen.GenAdd)
{
    <text>
    /// <summary>
    /// @(gen.BusName)新增输入
    /// </summary>
    public partial class @(entityNamePc)AddInput 
    {
    @foreach (var col in gen.Fields.Where(w => w.WhetherAdd))
    {
        if (!col.IsIgnoreColumn())
        {
        <text>
        /// <summary>
        /// @col.Title
        /// </summary>
        @if (!col.IsNullable)
        {
        @:[Required(ErrorMessage = "@((!string.IsNullOrEmpty(col.Title) ? col.Title : col.ColumnName))不能为空")]
        }
        @col.PropCsByInput()
        </text>
        }
    }
    }
    </text>
}

@if (gen.GenUpdate)
{
    <text>
    /// <summary>
    /// @(gen.BusName)更新数据输入
    /// </summary>
    public partial class @(entityNamePc)UpdateInput 
    {
    @if (hasBaseEntity)
    {
        <text>
        public long Id { get; set; }
        </text>
    }
    @foreach (var col in gen.Fields.Where(w => w.WhetherUpdate))
    {
        if (!col.IsIgnoreColumn())
        {
        <text>
        /// <summary>
        /// @col.Title
        /// </summary>
        @if (!col.IsNullable)
        {
        @:[Required(ErrorMessage = "@((!string.IsNullOrEmpty(col.Title) ? col.Title : col.ColumnName))不能为空")]
        }
        @col.PropCsByInput()
        </text>
        }
    }
    }
    </text>
}
}