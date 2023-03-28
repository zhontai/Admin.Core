using System.ComponentModel;

namespace ZhonTai.Admin.Core.Consts;

/// <summary>
/// 订阅命名
/// </summary>
public class SubscribeNames
{
    /// <summary>
    /// 短信单发
    /// </summary>
    [Description("短信单发")]
    public static string SmsSingleSend { get; set; } = "zhontai.admin.smsSingleSend";
}