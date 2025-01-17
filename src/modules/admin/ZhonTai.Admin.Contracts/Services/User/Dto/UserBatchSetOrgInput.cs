namespace ZhonTai.Admin.Services.User.Dto;

/// <summary>
/// 批量设置部门
/// </summary>
public class UserBatchSetOrgInput
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