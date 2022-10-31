using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using System;

namespace ZhonTai.Admin.Domain.UserStaff;

/// <summary>
/// 用户员工
/// </summary>
[Table(Name = "ad_user_staff")]
public partial class UserStaffEntity : EntityTenant
{
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
    public Sex? Sex { get; set; }

    /// <summary>
    /// 入职时间
    /// </summary>
    public DateTime? EntryTime { get; set; }

    /// <summary>
    /// 个人简介
    /// </summary>
    [Column(StringLength = 500)]
    public string Introduce { get; set; }
}