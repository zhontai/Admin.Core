using System;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;
using MyApp.Api.Contracts.Core.Consts;

namespace MyApp.Api.Contracts.Domain.Module;

/// <summary>
/// 模块
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "module")]
[Index("idx_{tablename}_01", nameof(TenantId) + "," + nameof(Name), true)]
public partial class ModuleEntity : EntityTenant
{
    /// <summary>
    /// 名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }
}