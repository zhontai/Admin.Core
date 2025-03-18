namespace ZhonTai.Admin.Services.PrintTemplate.Inputs;

/// <summary>
/// 设置启用
/// </summary>
public class PrintTemplateSetEnableInput
{
    /// <summary>
    /// 地区Id
    /// </summary>
    public long PrintTemplateId { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
}