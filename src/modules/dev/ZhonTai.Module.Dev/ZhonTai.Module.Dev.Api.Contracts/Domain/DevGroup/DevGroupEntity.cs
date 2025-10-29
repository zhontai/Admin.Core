using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;

namespace ZhonTai.Module.Dev.Api.Contracts.Domain.DevGroup;

/// <summary>
/// 模板组
/// </summary>
/// <remarks></remarks>
[Table(Name="dev_group")]
public partial class DevGroupEntity: EntityBase
{
    /// <summary>
    /// 模板组名称
    /// </summary>
    /// <remarks></remarks>
    [Column(StringLength=200)]
    public string Name { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks></remarks>
    [Column(StringLength=500)]
    public string? Remark { get; set; }
}
