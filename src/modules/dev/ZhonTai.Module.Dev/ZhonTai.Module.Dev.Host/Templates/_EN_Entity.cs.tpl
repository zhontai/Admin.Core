@{
    var gen = Model as ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen.CodeGenEntity;
    if (gen?.Fields?.Any() != true) return;
    
    var entityNamePc = gen.EntityName.NamingPascalCase();
    var entityClassName = entityNamePc.PadEndIfNot("Entity").PadEndIfNotEmpty(gen.BaseEntity, ": " + gen.BaseEntity);

    var commonFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "OwnerId", "OwnerOrgId", "IsDeleted", "TenantId",
        "CreatedUserId", "CreatedUserName", "CreatedUserRealName", "CreatedTime", 
        "ModifiedUserId", "ModifiedUserName", "ModifiedUserRealName", "ModifiedTime"
    };
    
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
/// @(gen.BusName)实体类
/// </summary>
/// <remarks>@gen.Comment</remarks>
@(gen.GetTableIndexAttributes())
public partial class @entityClassName
{
@for (int i = 0; i < validFields.Count; i++)
{
    var col = validFields[i];
    
    @if (i > 0)
    {
    // 在属性之间添加空行（第一个属性前不添加）
@:
    }

    // 生成属性注释
    @:/// <summary>
    @:/// @col.Title
    @:/// </summary>
    @:/// <remarks>@col.Comment</remarks>
    
    // 生成列属性和属性定义
    @:@col.FreeSqlColumnAttribute()
    @:@col.PropCs()

    // 如果需要包含导航属性（当前被注释）
    @*@if (col.IsIncludeColumn())
    {
        @:@col.FreeSqlNavigaetAttribute()
        @:@col.PropIncludeCs()
    }*@
}
}