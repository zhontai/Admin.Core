using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;

namespace ZhonTai.Module.Dev.Api.Contracts.Domain.DevProjectModelField;

/// <summary>
/// 项目模型字段 实体类
/// </summary>
/// <remarks></remarks>
[Table(Name="dev_project_model_field")]
public partial class DevProjectModelFieldEntity: EntityBase
{
    /// <summary>
    /// 所属模型
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=1, Precision = 64)]
    public long? ModelId { get; set; }

    /// <summary>
    /// 字段名称
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=2, StringLength=200)]
    public string Name { get; set; }

    /// <summary>
    /// 字段编码
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=2, StringLength=200)]
    public string Code { get; set; }

    /// <summary>
    /// 字段类型
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=4, StringLength=64)]
    public string? DataType { get; set; }

    /// <summary>
    /// 是否必填
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=5)]
    public bool? IsRequired { get; set; }

    /// <summary>
    /// 最大长度
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=6)]
    public int? MaxLength { get; set; }

    /// <summary>
    /// 最小长度
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=7)]
    public int? MinLength { get; set; }


    /// <summary>
    /// 字段顺序
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=8, Precision = 64)]
    public int Sort { get; set; }

    /// <summary>
    /// 字段描述
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=9)]
    public string? Description { get; set; }

    /// <summary>
    /// 字段属性
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=10, StringLength=500)]
    public string Properties { get; set; }
}
