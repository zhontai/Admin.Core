using Newtonsoft.Json;

namespace ZhonTai.Admin.Services.Dict.Dto;

public class DictGetListDto
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