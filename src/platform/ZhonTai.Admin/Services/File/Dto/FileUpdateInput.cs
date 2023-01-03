using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.File.Dto;

/// <summary>
/// 修改
/// </summary>
public partial class FileUpdateInput : FileAddInput
{
    /// <summary>
    /// 文件Id
    /// </summary>
    [Required(ErrorMessage = "请选择文件")]
    public string Id { get; set; }
}