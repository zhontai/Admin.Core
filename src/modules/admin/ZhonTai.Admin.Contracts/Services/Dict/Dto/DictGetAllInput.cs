using ZhonTai.Admin.Core.Dto;

namespace ZhonTai.Admin.Domain.Dict.Dto;

/// <summary>
/// 字典列表请求
/// </summary>
public partial class DictGetAllInput: QueryInput
{
    /// <summary>
    /// 字典类型Id
    /// </summary>
    public long DictTypeId { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    public string Name { get; set; }
}