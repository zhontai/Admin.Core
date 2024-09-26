using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Magicodes.IE.Core;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ZhonTai.Admin.Services.Dict.Dto;

/// <summary>
/// 布尔值映射
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class BoolValueMappingAttribute : ValueMappingsBaseAttribute
{
    public override Dictionary<string, object> GetMappings(PropertyInfo propertyInfo)
    {
        var res = new Dictionary<string, object>
        {
            { "是", true },
            { "否", false }
        };
        return res;
    }
}

[ExcelExporter(Name = "字典列表")]
public class DictExportHeader
{
    [ExporterHeader(DisplayName = "字典分类", ColumnIndex = 2)]
    public string DictTypeName { get; set; }

    [ExporterHeader(DisplayName = "字典名称", ColumnIndex = 3)]
    public string Name { get; set; }

    [ExporterHeader(DisplayName = "字典编码", ColumnIndex = 4)]
    public string Code { get; set; }

    [ExporterHeader(DisplayName = "字典值", ColumnIndex = 5)]
    public string Value { get; set; }

    //[BoolValueMapping]
    //[ValueMapping(text: "是", true)]
    //[ValueMapping(text: "否", false)]
    [ExporterHeader(DisplayName = "启用", ColumnIndex = 6)]
    public bool Enabled { get; set; }

    [ExporterHeader(DisplayName = "排序", ColumnIndex = 7)]
    public int Sort { get; set; }

    [ExporterHeader(DisplayName = "说明", ColumnIndex = 8)]
    public string Description { get; set; }
}

[ExcelExporter(Name = "字典列表", TableStyle = TableStyles.Light9, AutoFitAllColumn = true, AutoFitMaxRows = 5000)]
public class DictExport: DictExportHeader
{
    [ExporterHeader(DisplayName = "字典编号", Format = "0", ColumnIndex = 1)]
    public long Id { get; set; }

    [ExporterHeader(DisplayName = "创建人员", ColumnIndex = 10)]
    public string CreatedUserRealName { get; set; }

    [ExporterHeader(DisplayName = "创建时间", Format = "yyyy-MM-dd HH:mm:ss", Width = 20, ColumnIndex = 11)]
    public DateTime? CreatedTime { get; set; }

    [ExporterHeader(DisplayName = "修改人员", ColumnIndex = 12)]
    public string ModifiedUserRealName { get; set; }

    [ExporterHeader(DisplayName = "修改时间", Format = "yyyy-MM-dd HH:mm:ss", Width = 20, ColumnIndex = 13)]
    public virtual DateTime? ModifiedTime { get; set; }
}