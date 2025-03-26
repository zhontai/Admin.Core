namespace ZhonTai.Admin.Services.PrintTemplate.Inputs;

/// <summary>
/// 设置启用
/// </summary>
public class PrintTemplateSetEnableInput
{
    /// <summary>
    /// 打印模板Id
    /// </summary>
    public long PrintTemplateId { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
}