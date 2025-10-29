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
        if (String.IsNullOrWhiteSpace(config.Usings))
            return new string[] { };

        var ns = config.Usings.Split(';')
            .ConcatIfNotNull(config.Fields?
                .Where(w => !String.IsNullOrWhiteSpace(w.IncludeEntity))
                .Select(s => s.IncludeEntity)
                .Select(s => "" + Type.GetType(s)?.Namespace)
                .Where(w => !String.IsNullOrWhiteSpace(w))
            ).Distinct();

        return ns;
    }

    public struct PermInfo
    {
        public string Code;
        public string Label;
        public PermInfo(String code, String label)
        {
            Code = code;
            Label = label;
        }
    }

    public static IEnumerable<PermInfo> GetPermissionsToGen(this CodeGenEntity table, String spillter = ":")
    {
        return new List<PermInfo>
        {
            new PermInfo("add","新增"),new PermInfo("get","查询"),new PermInfo("update","更新"),
            new PermInfo("delete","删除"),new PermInfo("get-page","分页查询")
        }
        .AddIf(table.GenGetList, new PermInfo("get-list", "列表查询"))
        .AddIf(table.GenBatchDelete, new PermInfo("batch-delete", "批量删除"))
        .AddIf(table.GenSoftDelete, new PermInfo("soft-delete", "软删除"))
        .AddIf(table.GenBatchSoftDelete, new PermInfo("batch-soft-delete", "批量软删除"))
        .Select(s =>
             new PermInfo(
                String.Join(spillter, "api", table.ApiAreaName?.NamingKebabCase(),
                    table.EntityName.NamingKebabCase(), s.Code), s.Label)
        );
    }

    public static String GetTableIndexAttributes(this CodeGenEntity config)
    {
        var attrs = new List<String> { { "Table(Name=\"" + config.TableName + "\")" } };

        if (config.Fields != null)
        {
            var indexFields = config.Fields.Where(w => w.IsUnique || !String.IsNullOrWhiteSpace(w.IndexMode));

            if (indexFields != null && indexFields.Count() > 0)
            {
                attrs.AddRange(indexFields.Select(w => "Index(\"Index_{TableName}_" +
                    (String.IsNullOrWhiteSpace(w.ColumnRawName) ? w.ColumnName : w.ColumnRawName)
                    + "\", \"" + w.ColumnName + (!String.IsNullOrWhiteSpace(w.IndexMode) ? " " + w.IndexMode : "") + "\", "
                    + (w.IsUnique ? "true" : "false") + ")"));
            }
        }

        return "[" + String.Join(",", attrs) + "]";
    }
}