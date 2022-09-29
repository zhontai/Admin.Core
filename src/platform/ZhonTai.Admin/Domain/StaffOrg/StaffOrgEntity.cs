using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Domain.Staff;
using ZhonTai.Admin.Domain.Org;

namespace ZhonTai.Admin.Domain;

/// <summary>
/// 员工所属部门
/// </summary>
[Table(Name = "ad_staff_org")]
[Index("idx_{tablename}_01", nameof(StaffId) + "," + nameof(OrgId), true)]
public partial class StaffOrgEntity : EntityAdd
{
    /// <summary>
    /// 员工Id
    /// </summary>
	public long StaffId { get; set; }

    /// <summary>
    /// 员工
    /// </summary>
    public StaffEntity Staff { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
	public long OrgId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    public OrgEntity Org { get; set; }
}