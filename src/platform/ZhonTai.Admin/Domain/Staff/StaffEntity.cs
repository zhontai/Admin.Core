using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Domain.User;

namespace ZhonTai.Admin.Domain.Staff;

/// <summary>
/// 员工
/// </summary>
[Table(Name = "ad_staff")]
public partial class StaffEntity : EntityFull, ITenant
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [Column(Position = -10)]
    public long? TenantId { get; set; }

    /// <summary>
    /// 主属部门Id
    /// </summary>
    public long MainOrgId { get; set; }

    public OrgEntity MainOrg { get; set; }

    /// <summary>
    /// 直属主管Id
    /// </summary>
    public long? ManagerUserId { get; set; }

    public UserEntity ManagerUser { get; set; }

    /// <summary>
    /// 职位
    /// </summary>
    public string Position { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    [Column(StringLength = 20)]
    public string JobNumber { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    [Column(MapType = typeof(int))]
    public SexEnum? Sex { get; set; }

    /// <summary>
    /// 入职时间
    /// </summary>
    public DateTime? EntryTime { get; set; }

    [Navigate(ManyToMany = typeof(StaffOrgEntity))]
    public ICollection<OrgEntity> Orgs { get; set; }
}