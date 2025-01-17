namespace ZhonTai.Admin.Core;

/// <summary>
/// 应用配置
/// </summary>
public class AppSettings
{
    /// <summary>
    /// 使用配置中心
    /// </summary>
    public bool UseConfigCenter { get; set; } = false;

    /// <summary>
    /// 配置中心路径
    /// </summary>
    public string ConfigCenterPath { get; set; } = "ConfigCenter";
}