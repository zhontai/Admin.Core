using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Core.Filters;
using Magicodes.ExporterAndImporter.Core.Models;
using Magicodes.ExporterAndImporter.Excel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.Dict.Dto;

/// <summary>
/// 字典导入列头筛选器
/// </summary>
public class DictImportHeaderFilter : IImportHeaderFilter
{
    public List<ImporterHeaderInfo> Filter(List<ImporterHeaderInfo> importerHeaderInfos)
    {
        foreach (var item in importerHeaderInfos)
        {
            if (item.PropertyName == "Enabled")
            {
                item.MappingValues = new Dictionary<string, dynamic>()
                    {
                        {"是", true },
                        {"否", false }
                    };
            }
        }
        return importerHeaderInfos;
    }
}

/// <summary>
/// 字典导入
/// </summary>
[ExcelImporter(IsLabelingError = true, ImportHeaderFilter = typeof(DictImportHeaderFilter))]
public class DictImport
{
    /// <summary>
    /// 字典Id
    /// </summary>
    [ImporterHeader(Name = "字典Id", IsIgnore = true)]
    public long Id { get; set; }

    /// <summary>
    /// 字典类型Id
    /// </summary>
    [ImporterHeader(Name = "字典类型Id", IsIgnore = true)]
    public long DictTypeId { get; set; }

    [ImporterHeader(Name = "字典分类")]
    [Required(ErrorMessage = "不能为空")]
    public string DictTypeName { get; set; }

    [ImporterHeader(Name = "字典名称", IsAllowRepeat = false)]
    [Required(ErrorMessage = "不能为空")]
    public string Name { get; set; }

    [ImporterHeader(Name = "字典编码", IsAllowRepeat = false)]
    public string Code { get; set; }

    [ImporterHeader(Name = "字典值", IsAllowRepeat = false)]
    public string Value { get; set; }

    [ImporterHeader(Name = "启用",IsInterValidation = false)]
    public bool Enabled { get; set; }

    [ImporterHeader(Name = "排序")]
    public int Sort { get; set; }

    [ImporterHeader(Name = "说明")]
    public string Description { get; set; }
}