@{
    var gen = Model as ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    if (gen.Fields == null) return;
    if (gen.Fields.Count() == 0) return;

    var entityNamePc = gen.EntityName.NamingPascalCase();

    var entityClassName = entityNamePc
        .PadEndIfNot("Entity")
        .PadEndIfNotEmpty(gen.BaseEntity, ": " + gen.BaseEntity);

    var commonFields = new String[] { "id", "OwnerId", "OwnerOrgId", "IsDeleted", "TenantId"
    , "CreatedUserId","CreatedUserName","CreatedTime", "ModifiedUserId","ModifiedUserName","ModifiedTime"};
}
using System;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;
@foreach(var ns in gen.GetUsings())
{
@:using @(ns);    
}

#pragma warning disable CS8632

namespace @(gen.Namespace).Domain.@(entityNamePc)
{
    /// <summary>
    /// @gen.BusName @("实体类")
    /// </summary>
    /// <remarks>@(gen.Comment)</remarks>
    @(gen.GetTableIndexAttributes())
    public partial class @(entityClassName)
    {
@if (gen.Fields != null)
{
    foreach (var col in gen.Fields)
    {
        if (col == null) continue;
        if (!String.IsNullOrWhiteSpace(gen.BaseEntity))
            if (commonFields.Any(a => a.ToLower() == col.ColumnName.ToLower()))
                continue;

        if(!col.IsIgnoreColumn())
        {

        @:/// <summary>
        @:/// @(col.Title)
        @:/// </summary>
        @:/// <remarks>@(col.Comment)</remarks>
        @:@col.FreeSqlColumnAttribute()
        @:@col.PropCs()

        }

        ///if (col.IsIncludeColumn())
        ///{
        ///
        ///@:@col.FreeSqlNavigaetAttribute()
        ///@:@col.PropIncludeCs()
        ///
        ///}
    }

}
    }

}

#pragma warning restore CS8632

