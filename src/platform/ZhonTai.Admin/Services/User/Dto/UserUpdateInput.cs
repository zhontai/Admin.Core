using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.User.Dto;

/// <summary>
/// 修改
/// </summary>
public partial class UserUpdateInput: UserFormInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择用户")]
    public long Id { get; set; }
}