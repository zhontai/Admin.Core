using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;

namespace ZhonTai.Module.Dev.Api.Contracts.Domain.DevProject;

/// <summary>
/// 项目
/// </summary>
/// <remarks></remarks>
[Table(Name="dev_project")]
public partial class DevProjectEntity: EntityBase
{
    /// <summary>
    /// 项目名称
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=1, StringLength=200)]
    public string Name { get; set; }

    /// <summary>
    /// 项目编码
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=2, StringLength=200)]
    public string Code { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=3)]
    public bool IsEnable { get; set; }

    /// <summary>
    /// 使用模板组
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=4, Precision = 64)]
    public long GroupId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=10)]
    public string? Remark { get; set; }
}
