using ZhonTai.Admin.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.User.Dto;

/// <summary>
/// 更新基本信息
/// </summary>
public class UserUpdateBasicInput
{
    /// <summary>
    /// 姓名
    /// </summary>
    [Required(ErrorMessage = "请输入姓名")]
    public string Name { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }
}