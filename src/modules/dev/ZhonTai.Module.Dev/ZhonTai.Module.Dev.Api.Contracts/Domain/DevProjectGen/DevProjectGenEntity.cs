using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;

namespace ZhonTai.Module.Dev.Api.Contracts.Domain.DevProjectGen;

/// <summary>
/// 项目生成
/// </summary>
/// <remarks></remarks>
[Table(Name="dev_project_gen")]
public partial class DevProjectGenEntity: EntityBase
{
    /// <summary>
    /// 所属项目
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=1, Precision = 64)]
    public long ProjectId { get; set; }

    /// <summary>
    /// 模板组
    /// </summary>
    /// <remarks></remarks>
    [Column(Position=2, StringLength=64)]
    public string GroupIds { get; set; }
}

