using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ZhonTai.Admin.Services.Region;


public class RegionInfo
{
    private static readonly Regex ShengjiRegex = new Regex(
        @"^(?<Name>.+?)[（(](?<ShortName>[^）)]+)[）)]$",
        RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    [JsonPropertyName("children")]
    public List<object> Children { get; set; }

    [JsonPropertyName("diji")]
    public string DiJi { get; set; }

    [JsonPropertyName("quHuaDaiMa")]
    public string QuHuaDaiMa { get; set; }

    [JsonPropertyName("quhao")]
    public string QuHao { get; set; }

    [JsonPropertyName("shengji")]
    public string ShengJi { get; set; }

    [JsonPropertyName("xianji")]
    public string XianJi { get; set; }

    public (string Name, string ShortName) ParseShengJi()
    {
        var match = ShengjiRegex.Match(ShengJi ?? "");
        return match.Success
            ? (match.Groups["Name"].Value.Trim(),
               match.Groups["ShortName"].Value.Trim())
            : ("", "");
    }
}
