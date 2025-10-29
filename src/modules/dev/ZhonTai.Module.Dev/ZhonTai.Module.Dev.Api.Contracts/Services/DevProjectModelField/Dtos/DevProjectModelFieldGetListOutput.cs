using System;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectModelField.Dtos;

/// <summary>项目模型字段列表查询结果输出</summary>
public partial class DevProjectModelFieldGetListOutput
{
    public long Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public string CreatedUserName { get; set; }
    public string ModifiedUserName { get; set; }
    public DateTime? ModifiedTime { get; set; }
    /// <summary>所属模型</summary>
    public long? ModelId { get; set; }
    ///<summary>所属模型显示文本</summary>
    public string? ModelId_Text { get; set; }
    /// <summary>字段名称</summary>
    public string Name { get; set; }
    /// <summary>字段编码</summary>
    public string Code { get; set; }
    /// <summary>字段类型</summary>
    public string? DataType { get; set; }
    /// <summary>字段类型名称</summary>
    public string? DataTypeDictName { get; set; }
    /// <summary>是否必填</summary>
    public bool? IsRequired { get; set; }
    /// <summary>最大长度</summary>
    public int? MaxLength { get; set; }
    /// <summary>最小长度</summary>
    public int? MinLength { get; set; }
    /// <summary>字段顺序</summary>
    public int Sort { get; set; }
    /// <summary>字段描述</summary>
    public string? Description { get; set; }
    /// <summary>字段属性</summary>
    public string Properties { get; set; }
    /// <summary>字段属性名称</summary>
    public string? PropertiesDictName { get; set; }
}