namespace ZhonTai.Admin.Services.User.Dto;

/// <summary>
/// 设置主管
/// </summary>
public class UserSetManagerInput
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
	public long OrgId { get; set; }

    /// <summary>
    /// 是否主管
    /// </summary>
    public bool IsManager { get; set; }
}