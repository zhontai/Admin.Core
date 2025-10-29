using System;
using System.Collections.Generic;
using System.Linq;
using ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen;
using ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectModelField.Dtos;

public static class CodeGenFieldEntityExtension
{
    static string[] numFields = [ "decimal", "double",
                "float", "int", "long","byte", "sbyte", "short", "uint", "ulong", "ushort","Single"
                ,"Int16","UInt16","Int32","UInt32","Int64","UInt64"];

    static string[] strFields = ["char", "string", "byte[]"];

    /// <summary>
    /// 是否文本列
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    public static bool IsTextColumn(this CodeGenFieldEntity col)
    {
        return strFields.Any(a => col.NetType.ToLower().StartsWith(a.ToLower()));
    }

    /// <summary>
    /// 是否包含
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    public static bool IsTextQueryContains(this CodeGenFieldEntity col)
    {
        return strFields.Any(a => col.QueryType == "0");
    }
    /// <summary>
    /// 是否数字列
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    public static bool IsNumColumn(this CodeGenFieldEntity col)
    {
        return numFields.Any(a => col.NetType.ToLower().StartsWith(a.ToLower()));
    }
    /// <summary>
    /// 返回是否在实现中忽略的字段
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    /// <remarks>当列名(ColumnName)为 "- _ >" 的任意一个字符时返回 true </remarks>
    public static bool IsIgnoreColumn(this CodeGenFieldEntity col)
    {
        return ("-_>".Contains(col.ColumnName));
    }
    /// <summary>
    /// 返回是否为引入字符
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    /// <remarks>当 IncludeEntity 不为空时返回 true</remarks>
    public static bool IsIncludeColumn(this CodeGenFieldEntity col)
    {
        return !string.IsNullOrWhiteSpace(col.IncludeEntity);
    }

    /// <summary>
    /// 按列的类型获取CSharp对应的默认值字符串表示形式
    /// </summary>
    /// <param name="col"></param>
    /// <remarks>
    /// 数字型：返回原始值
    /// 布尔型：1-true true-true false-false 其它值-false
    /// 日期型：new DateTime(Year, Month, Day, Hour, Minute, Second)
    /// 字符型：'默认值第一个字符'
    /// 字符串："默认值"
    /// </remarks>
    /// <returns> 默认值按字段类型的表示形式 </returns>
    public static String GetDefautlValueStringCS(this CodeGenFieldEntity col)
    {
        if (col.IsNumColumn())
        {
            if (String.IsNullOrWhiteSpace(col.DefaultValue)) return "0";
            return "" + col.DefaultValue;
        }
        if (col.NetType == "bool")
        {
            if (String.IsNullOrWhiteSpace(col.DefaultValue)) return "false";
            var val = col.DefaultValue.ToLower();
            if (val == "1") return "true";
            if (val != "true" && val != "false") { return "false"; }
            return val;
        }
        if (col.NetType == "DateTime")
        {
            if (String.IsNullOrWhiteSpace(col.DefaultValue)) return "DateTime.MinValue";
            if (DateTime.TryParse(col.DefaultValue, out DateTime res))
            {
                return "new DateTime(" + String.Join(", ", res.Year, res.Month, res.Day, res.Hour, res.Minute, res.Second) + ")";
            }
        }
        if (col.NetType == "char")
        {
            if (String.IsNullOrWhiteSpace(col.DefaultValue)) return "'\0'";
            return "'" + col.DefaultValue[0] + "'";
        }
        return "\"" + col.DefaultValue + "\"";
    }

    public static String GetDefaultValueStringScript(this CodeGenFieldEntity col)
    {
        if (col.IsNullable) return "null";
        if (col.NetType == "DateTime")
        {
            if (String.IsNullOrWhiteSpace(col.DefaultValue))
            {
                return "new Date()";
            }
            if (DateTime.TryParse(col.DefaultValue, out DateTime res))
            {
                return "new Date(" + String.Join(", ", res.Year, res.Month, res.Day, res.Hour, res.Minute, res.Second) + ")";
            }
            return "new Date()";
        }
        return GetDefautlValueStringCS(col);
    }
    /// <summary>
    /// 获取FreeSql列定义Attribute
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    public static string FreeSqlColumnAttribute(this CodeGenFieldEntity col)
    {
        if (col == null) return string.Empty;
        if ("-_>".Contains(col.ColumnName)) return string.Empty;

        var isNumField = col.IsNumColumn();
        var isStrField = col.IsTextColumn();
        var strLen = 255;
        var preLen = -1;
        var scaleLen = -1;

        if (!String.IsNullOrWhiteSpace(col.Length))
        {
            var lens = col.Length.Split(',');
            if (lens.Length > 0)
            {
                Int32.TryParse(lens[0], out preLen);
                Int32.TryParse(lens[0], out strLen);
            }
            if (lens.Length > 1)
                Int32.TryParse(lens[1], out scaleLen);
        }

        var attrs = new List<String>() { };

        if (!String.IsNullOrWhiteSpace(col.ColumnRawName) && col.ColumnRawName.Trim() == col.ColumnName)
            attrs.Add("Name=\"" + col.ColumnName + "\"");

        //if (col.IsPrimary)
        //    attrs.Add("IsPrimary = true");

        if (col.Position > 0)
            attrs.Add("Position=" + col.Position);

        if (!col.WhetherAdd)
            attrs.Add("CanInsert = false");

        if (!col.WhetherUpdate)
            attrs.Add("CanUpdate = false");

        if (!string.IsNullOrWhiteSpace(col.DbType))
            attrs.Add("DbType=\"" + col.DbType + "\"");

        if (isStrField && strLen > -1)
            attrs.Add("StringLength=" + strLen);

        if (isNumField && (preLen > -1 || scaleLen > 0))
        {
            if (preLen > -1)
                attrs.Add("Precision = " + preLen);
            if (scaleLen > 0)
                attrs.Add("Scale=" + scaleLen);
        }

        return attrs.Count > 0 ? string.Concat("[Column(", string.Join(", ", attrs), ")]") : string.Empty;
    }

    /// <summary>
    /// 获取FreeSql导航Attribute
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    public static string FreeSqlNavigaetAttribute(this CodeGenFieldEntity col)
    {
        if (col == null) return string.Empty;
        if (string.IsNullOrWhiteSpace(col.IncludeEntity) || string.IsNullOrWhiteSpace(col.IncludeEntityKey)) return "";
        return string.Concat("[Navigate(nameof(", col.IncludeEntity.NamingPascalCase().PadEndIfNot("Entity"), ".", col.IncludeEntityKey.NamingPascalCase(), "))]");

    }

    /// <summary>
    /// 获取列属性定义字符串 C#
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    public static string PropCs(this DevProjectModelFieldGetOutput col)
    {
        if (col == null) return string.Empty;

        var propName = col.Code.NamingPascalCase();
        var propType = col.DataType;
        return string.Join(" ", "public", propType, propName, "{ get; set; }");
    }

    /// <summary>
    /// 获取列属性定义字符串 C#
    /// </summary>
    /// <param name="col"></param>
    /// <param name="isNullable"></param>
    /// <returns></returns>
    public static string PropCs(this CodeGenFieldEntity col, bool isNullable = false, bool isInput = false, bool isOutput = false)
    {
        if (col == null) return string.Empty;

        if ("-_>".Contains(col.ColumnName)) return string.Empty;

        var propType = col.NetType.PadEndIfNot(col.IsNullable || isNullable, "?");
        var propName = col.ColumnName.NamingPascalCase();

        var defaultValueStr = (!String.IsNullOrWhiteSpace(col.DefaultValue) ? (" = " + col.GetDefautlValueStringCS() + ";") : "");
        if (isInput && col.Editor == "my-bussiness-select" && col.IncludeMode == 1)
        {
            //输入模型
            //下拉一对多，接受数组
            return $@"public {propType} {propName} {{ get {{ return string.Join(',', {propName}_Values ?? new List<string>()); }} }}
        ///<summary>页面提交的{col.Title}数组</summary>
        public List<string>? {propName}_Values {{ get; set; }}";
        }
        else if (isOutput && col.Editor == "my-bussiness-select")
        {
            //输出模型
            //下拉显示文本值
            var textFields = new List<string>();
            //默认模型
            textFields.Add($@"public {propType} {propName} {{ get; set; }}");
            //有关联字段
            if (!string.IsNullOrWhiteSpace(col.IncludeEntityKey))
            {
                textFields.Add($"///<summary>{col.Title}显示文本</summary>");
                //单选返回text,多选返回texts
                textFields.Add(col.IncludeMode == 0 ? $@"public string? {propName}_Text {{ get; set; }}" : $@"public List<string>? {propName}_Texts {{ get; set; }}");
            }
            if (col.IncludeMode == 1)
            {
                textFields.Add($"///<summary>页面使用的{col.Title}数组</summary>");
                textFields.Add($@"public List<string>? {propName}_Values {{ get {{ return {propName}?.Split("","", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(); }} }}");
            }
            var textField = string.Join("\n        ", textFields);
            //输出模型
            return textField;
        }
        return string.Join(" ", "public", propType, propName, "{ get; set; }") + defaultValueStr;
    }

    /// <summary>
    /// 获取列属性定义字符串 C# 输入模型
    /// </summary>
    /// <param name="col"></param>
    /// <param name="isNullable"></param>
    /// <returns></returns>
    public static string PropCsByInput(this CodeGenFieldEntity col, bool isNullable = false)
    {
        return PropCs(col, isNullable, true, false);
    }
    /// <summary>
    /// 获取列属性定义字符串 C# 输出模型
    /// </summary>
    /// <param name="col"></param>
    /// <param name="isNullable"></param>
    /// <returns></returns>
    public static string PropCsByOutput(this CodeGenFieldEntity col, bool isNullable = false)
    {
        return PropCs(col, isNullable, false, true);
    }

    /// <summary>
    /// 获取引用类型属性定义字符串 c#
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    public static string PropIncludeCs(this CodeGenFieldEntity col)
    {
        if (string.IsNullOrWhiteSpace(col.IncludeEntity)) return string.Empty;

        var colEntityName = col.IncludeEntity.Split('.').Last().PadEndIfNot("Entity").NamingPascalCase();
        var propName = colEntityName.Substring(0, colEntityName.Length - 6);//属性名去掉 Entity

        var propType = (col.IncludeMode == 1 ? string.Concat("IEnumerable<", colEntityName, ">") : colEntityName) + "?";
        if (col.IncludeMode == 1)
            propName += "_Texts";

        return string.Join(" ", "public", propType, propName, "{ get; set; }");
    }
}
