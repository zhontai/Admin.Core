
namespace ZhonTai.Admin.Domain.Dictionary.Dto;

public partial class DictionaryGetPageDto
{
    /// <summary>
    /// 字典类型Id
    /// </summary>
    public long DictionaryTypeId { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    public string Name { get; set; }
}