using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using System;
using MyApp.Api.Core.Consts;

namespace MyApp.Api.Domain.Module;

/// <summary>
/// 模块
/// </summary>
[Table(Name = ApiConsts.AreaName + "_module")]
[Index("idx_{tablename}_01", nameof(TenantId) + "," + nameof(Name), true)]
public partial class ModuleEntity : EntityTenant
{
    /// <summary>
    /// 名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }
}