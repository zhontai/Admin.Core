using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;

namespace ZhonTai.Module.Dev.Api.Contracts.Domain.DevTemplate;

/// <summary>
/// 模板
/// </summary>
/// <remarks></remarks>
[Table(Name="dev_template")]
public partial class DevTemplateEntity: EntityBase
{
    /// <summary>
    /// 模板名称
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=1, StringLength=200)]
    public string Name { get; set; }

    /// <summary>
    /// 模板分组
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=2, Precision = 64)]
    public long GroupId { get; set; }

    /// <summary>
    /// 生成路径
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=3, StringLength=500)]
    public string? OutTo { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=4)]
    public bool IsEnable { get; set; }

    /// <summary>
    /// 模板内容
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=5, StringLength = -1)]
    public string Content { get; set; }
}
