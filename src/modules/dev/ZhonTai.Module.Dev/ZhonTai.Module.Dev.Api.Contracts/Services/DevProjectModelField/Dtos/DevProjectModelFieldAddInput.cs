using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectModelField.Dtos;

/// <summary>项目模型字段新增输入</summary>
public partial class DevProjectModelFieldAddInput
{
    /// <summary>所属模型</summary>
    [ValidateRequired(ErrorMessage = "请选择所属模型")]
    public long? ModelId { get; set; }
    /// <summary>字段名称</summary>
    [Required(ErrorMessage = "字段名称不能为空")]
    public string Name { get; set; }
    /// <summary>字段编码</summary>
    [Required(ErrorMessage = "字段编码不能为空")]
    public string Code { get; set; }
    /// <summary>字段类型</summary>
    [ValidateRequired(ErrorMessage = "请选择字段类型")]
    public string? DataType { get; set; }
    /// <summary>是否必填</summary>
    public bool? IsRequired { get; set; }
    /// <summary>最大长度</summary>
    public int? MaxLength { get; set; }
    /// <summary>最小长度</summary>
    public int? MinLength { get; set; }
    /// <summary>字段顺序</summary>
    [Required(ErrorMessage = "字段顺序不能为空")]
    public int Sort { get; set; }
    /// <summary>字段描述</summary>
    public string? Description { get; set; }
    /// <summary>字段属性</summary>
    [Required(ErrorMessage = "请选择字段属性")]
    public string Properties { get; set; }
}