using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using OfficeOpenXml.Table;
using System;

namespace ZhonTai.Admin.Services.Dict.Dto;

[ExcelExporter(Name = "字典列表", TableStyle = TableStyles.None, AutoFitAllColumn = true, AutoFitMaxRows = 5000)]
public class DictExport
{
    [ExporterHeader(DisplayName = "字典编号", Format = "0")]
    public long Id { get; set; }

    [ExporterHeader(DisplayName = "字典名称")]
    public string Name { get; set; }

    [ExporterHeader(DisplayName = "字典编码")]
    public string Code { get; set; }

    [ExporterHeader(DisplayName = "字典值")]
    public string Value { get; set; }

    [ExporterHeader(IsIgnore = true)]
    public bool Enabled { get; set; }

    [ExporterHeader(DisplayName = "启用")]
    public string IsEnabled { get => Enabled ? "是" : "否"; }

    [ExporterHeader(DisplayName = "排序")]
    public int Sort { get; set; }

    [ExporterHeader(DisplayName = "创建时间", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
    public DateTime? CreatedTime { get; set; }
}