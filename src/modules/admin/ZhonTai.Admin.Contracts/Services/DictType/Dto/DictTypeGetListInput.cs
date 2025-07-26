using ZhonTai.Admin.Core.Dto;
namespace ZhonTai.Admin.Services.DictType.Dto;

/// <summary>
/// 字典类型列表请求
/// </summary>
public partial class DictTypeGetListInput: QueryInput
{
    /// <summary>
    /// 字典名称
    /// </summary>
    public string Name { get; set; }
}