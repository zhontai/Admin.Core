using System.Collections.Generic;
using ZhonTai.Admin.Domain.Role;

namespace ZhonTai.Admin.Domain.User.Dto;

public class DataPermissionDto
{
    /// <summary>
    /// 部门Id
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    public List<long> OrgIds { get; set; }

    /// <summary>
    /// 数据范围
    /// </summary>
    public DataScope DataScope { get; set; } = DataScope.Self;
}