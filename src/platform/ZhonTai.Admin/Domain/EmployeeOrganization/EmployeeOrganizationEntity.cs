using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Domain.Employee;
using ZhonTai.Admin.Domain.Organization;

namespace ZhonTai.Admin.Domain;

/// <summary>
/// 员工所属部门
/// </summary>
[Table(Name = "ad_employee_organization")]
[Index("idx_{tablename}_01", nameof(EmployeeId) + "," + nameof(OrganizationId), true)]
public partial class EmployeeOrganizationEntity : EntityAdd
{
    /// <summary>
    /// 员工Id
    /// </summary>
	public long EmployeeId { get; set; }

    /// <summary>
    /// 员工
    /// </summary>
    public EmployeeEntity Employee { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
	public long OrganizationId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    public OrganizationEntity Organization { get; set; }
}