using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.Document.Dto;

public class DocumentUpdateMenuInput : DocumentAddMenuInput
{
    /// <summary>
    /// 编号
    /// </summary>
    [Required]
    [ValidateRequired("请选择菜单")]
    public long Id { get; set; }
}