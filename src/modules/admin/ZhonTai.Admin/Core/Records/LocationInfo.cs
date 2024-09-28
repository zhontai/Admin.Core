namespace ZhonTai.Admin.Core.Records;

/// <summary>
/// 位置信息
/// </summary>
public record LocationInfo
{
    /// <summary>
    /// 国家
    /// </summary>
    public string Country { get; init; }
    /// <summary>
    /// 省份
    /// </summary>
    public string Province { get; init; }

    /// <summary>
    /// 城市
    /// </summary>
    public string City { get; init; }

    /// <summary>
    /// 网络服务商
    /// </summary>
    public string Isp { get; init; }

    /// <summary>
    /// 转换地址信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static LocationInfo Parse(string input)
    {
        var parts = input.Split('|');

        if (parts.Length >= 5)
        {
            return new LocationInfo
            {
                Country = parts[0] != "0" ? parts[0] : "",
                Province = parts[2] != "0" ? parts[2] : "",
                City = parts[3] != "0" ? parts[3] : "",
                Isp = parts[4] != "0" ? parts[4] : "",
            };
        }

        return new LocationInfo();
    }
}
