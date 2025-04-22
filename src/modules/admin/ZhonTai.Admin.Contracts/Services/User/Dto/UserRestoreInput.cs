namespace ZhonTai.Admin.Services.User.Dto;

/// <summary>
/// 恢复
/// </summary>
public class UserRestoreInput
{
    /// <summary>
    /// 用户Id列表
    /// </summary>
    public long[] UserIds { get; set; }
}