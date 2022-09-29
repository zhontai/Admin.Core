using System.Collections.Generic;
using ZhonTai.Admin.Domain.Org;

namespace ZhonTai.Admin.Domain.User;

/// <summary>
/// 员工添加
/// </summary>
public class StaffAddInput
{
    /// <summary>
    /// 主属部门Id
    /// </summary>
    public long MainOrgId { get; set; }

    /// <summary>
    /// 所属部门
    /// </summary>
    public long[] OrgIds { get; set; }

    public ICollection<OrgEntity> Orgs { get; set; }
}