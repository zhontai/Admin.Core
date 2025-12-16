using System;
using System.Collections.Generic;
using System.Linq;
using ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen;

public static class CodeGenEntityExtension
{
    public static IEnumerable<T> ConcatIfNotNull<T>(this IEnumerable<T> dst, IEnumerable<T>? src)
    {
        if (src != null) dst.Concat(src);
        return dst;
    }
    public static IEnumerable<string> GetUsings(this CodeGenEntity config)
    {
        if (string.IsNullOrWhiteSpace(config.Usings))
            return new string[] { };

        var ns = config.Usings.Split(';')
            .ConcatIfNotNull(config.Fields?
                .Where(w => !string.IsNullOrWhiteSpace(w.IncludeEntity))
                .Select(s => s.IncludeEntity)
                .Select(s => "" + Type.GetType(s)?.Namespace)
                .Where(w => !string.IsNullOrWhiteSpace(w))
            ).Distinct();

        return ns;
    }

    public struct PermInfo
    {
        public string Code;
        public string Label;
        public PermInfo(string code, string label)
        {
            Code = code;
            Label = label;
        }
    }

    public static IEnumerable<PermInfo> GetPermissionsToGen(this CodeGenEntity table, string spillter = ":")
    {
        return new List<PermInfo>()
        .AddIf(table.GenGet, new PermInfo("get", "查询"))
        .AddIf(table.GenGetPage, new PermInfo("get-page", "分页查询"))
        .AddIf(table.GenGetList, new PermInfo("get-list", "列表查询"))
        .AddIf(table.GenAdd, new PermInfo("add", "新增"))
        .AddIf(table.GenUpdate, new PermInfo("update", "更新"))
        .AddIf(table.GenDelete, new PermInfo("delete", "删除"))
        .AddIf(table.GenBatchDelete, new PermInfo("batch-delete", "批量删除"))
        .AddIf(table.GenSoftDelete, new PermInfo("soft-delete", "软删除"))
        .AddIf(table.GenBatchSoftDelete, new PermInfo("batch-soft-delete", "批量软删除"))
        .Select(s =>
             new PermInfo(
                string.Join(spillter, "api", table.ApiAreaName?.NamingKebabCase(),
                    table.EntityName.NamingKebabCase(), s.Code), s.Label)
        );
    }

    public static string GetTableIndexAttributes(this CodeGenEntity config)
    {
        var attrs = new List<string> { { "[Table(Name=\"" + config.TableName + "\")]" } };

        if (config.Fields != null)
        {
            var indexFields = config.Fields.Where(w => w.IsUnique || !string.IsNullOrWhiteSpace(w.IndexMode));

            if (indexFields != null && indexFields.Count() > 0)
            {
                attrs.AddRange(indexFields.Select(w => "[Index(\"Index_{TableName}_" +
                    (string.IsNullOrWhiteSpace(w.ColumnRawName) ? w.ColumnName : w.ColumnRawName)
                    + "\", \"" + w.ColumnName + (!string.IsNullOrWhiteSpace(w.IndexMode) ? " " + w.IndexMode : "") + "\", "
                    + (w.IsUnique ? "true" : "false") + ")]"));
            }
        }

        return string.Join(Environment.NewLine, attrs);
    }
}