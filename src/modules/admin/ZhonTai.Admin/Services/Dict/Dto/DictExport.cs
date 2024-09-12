using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using OfficeOpenXml.Table;
using System;

namespace ZhonTai.Admin.Services.Dict.Dto;

[ExcelExporter(Name = "字典列表", TableStyle = TableStyles.Light10, AutoFitAllColumn = true, AutoFitMaxRows = 5000)]
public class DictExport
{
    [ExporterHeader(DisplayName = "字典编号", Format = "0")]
    public long Id { get; set; }

    [ExporterHeader(DisplayName = "字典分类")]
    public string DictTypeName { get; set; }

    [ExporterHeader(DisplayName = "字典名称")]
    public string Name { get; set; }

    [ExporterHeader(DisplayName = "字典编码")]
    public string Code { get; set; }

    [ExporterHeader(DisplayName = "字典值")]
    public string Value { get; set; }

    [ExporterHeader(DisplayName = "启用")]
    [ValueMapping(text: "是", true)]
    [ValueMapping(text: "否", false)]
    public bool Enabled { get; set; }

    [ExporterHeader(DisplayName = "排序")]
    public int Sort { get; set; }

    [ExporterHeader(DisplayName = "说明")]
    public string Description { get; set; }

    [ExporterHeader(DisplayName = "创建人员")]
    public string CreatedUserRealName { get; set; }

    [ExporterHeader(DisplayName = "创建时间", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
    public DateTime? CreatedTime { get; set; }

    [ExporterHeader(DisplayName = "修改人员")]
    public string ModifiedUserRealName { get; set; }

    [ExporterHeader(DisplayName = "修改时间", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
    public virtual DateTime? ModifiedTime { get; set; }
}