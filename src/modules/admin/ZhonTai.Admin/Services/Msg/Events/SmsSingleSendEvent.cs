namespace ZhonTai.Admin.Services.Msg.Events;

/// <summary>
/// 短信单发事件
/// </summary>
public class SmsSingleSendEvent
{
    /// <summary>
    /// 手机号
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 短信内容
    /// </summary>
    public string Text { get; set; }
}
