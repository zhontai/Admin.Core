using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.User.Dto;

/// <summary>
/// 修改会员
/// </summary>
public class UserUpdateMemberInput: UserMemberFormInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择会员")]
    public override long Id { get; set; }
}