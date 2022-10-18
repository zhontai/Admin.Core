
namespace ZhonTai.Admin.Domain.UserStaff.Input;

/// <summary>
/// 修改
/// </summary>
public class UserStaffUpdateInput: UserStaffAddInput
{
    /// <summary>
    /// 编号
    /// </summary>
    public long Id { get; set; }
}