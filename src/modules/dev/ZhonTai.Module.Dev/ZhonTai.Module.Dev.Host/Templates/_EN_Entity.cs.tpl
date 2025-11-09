@{
    var gen = Model as ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen.CodeGenEntity;
    if (gen?.Fields?.Any() != true) return;
    
    var entityNamePc = gen.EntityName.NamingPascalCase();
    var entityClassName = entityNamePc.PadEndIfNot("Entity").PadEndIfNotEmpty(gen.BaseEntity, ": " + gen.BaseEntity);

    // 定义需要排除的公共字段
    var commonFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "OwnerId", "OwnerOrgId", "IsDeleted", "TenantId",
        "CreatedUserId", "CreatedUserName", "CreatedUserRealName", "CreatedTime", 
        "ModifiedUserId", "ModifiedUserName", "ModifiedUserRealName", "ModifiedTime"
    };
    
    // 过滤有效字段
    var validFields = gen.Fields
        .Where(col => col != null)
        .Where(col => !commonFields.Contains(col.ColumnName))
        .Where(col => !col.IsIgnoreColumn())
        .ToList();
}
using System;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;
@foreach(var ns in gen.GetUsings())
{
@:using @ns;    
}

namespace @(gen.Namespace).Domain.@(entityNamePc);

/// <summary>
/// @(gen.BusName)实体
/// </summary>
/// <remarks>@gen.Comment</remarks>
@(gen.GetTableIndexAttributes())
public partial class @entityClassName
{
@foreach (var col in validFields)
{
    // 属性注释
    @:/// <summary>
    @:/// @col.Title
    @:/// </summary>
    @if (!string.IsNullOrWhiteSpace(col.Comment))
    {
    @:/// <remarks>@col.Comment</remarks>
    }
    
    // 列属性
    @:@col.FreeSqlColumnAttribute()
    
    // 属性定义
    @:@col.PropCs()

    // 如果需要包含导航属性（当前被注释）
    @*@if (col.IsIncludeColumn())
    {
        @:@col.FreeSqlNavigaetAttribute()
        @:@col.PropIncludeCs()
    }*@
    
    // 在属性之间添加空行（最后一个属性后不添加）
    @if (col != validFields.Last())
    {
@:
    }
}
}