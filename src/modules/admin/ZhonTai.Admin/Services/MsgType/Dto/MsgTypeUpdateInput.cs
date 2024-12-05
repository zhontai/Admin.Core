using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.MsgType.Dto;

/// <summary>
/// 修改
/// </summary>
public partial class MsgTypeUpdateInput : MsgTypeAddInput
{
    /// <summary>
    /// 消息分类Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择消息分类")]
    public long Id { get; set; }
}