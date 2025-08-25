using System.Text.Json.Serialization;

namespace ZhonTai.Admin.Services.Dict.Dto;

/// <summary>
/// 字典列表
/// </summary>
public class DictGetListOutput
{
    /// <summary>
    /// 字典类型编码
    /// </summary>
    [JsonIgnore]
    public string DictTypeCode { get; set; }

    /// <summary>
    /// 字典类型名称
    /// </summary>
    [JsonIgnore]
    public string DictTypeName { get; set; }

    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 上级Id
    /// </summary>
    public long? ParentId { get; set; }

    public bool ShouldSerializeParentId()
    {
        return ParentId.HasValue && ParentId > 0;
    }

    /// <summary>
    /// 字典名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 字典编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 字典值
    /// </summary>
    public string Value { get; set; }
}