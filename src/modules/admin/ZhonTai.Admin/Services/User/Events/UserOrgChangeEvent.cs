using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.User.Events;

/// <summary>
/// 部门转移
/// </summary>
public class UserOrgChangeEvent
{
    /// <summary>
    /// 用户Id列表
    /// </summary>
    public long[] UserIds { get; set; }

    /// <summary>
    /// 所属部门Ids
    /// </summary>
    public virtual long[] OrgIds { get; set; }

    /// <summary>
    /// 主属部门Id
    /// </summary>
    public long OrgId { get; set; }
}
