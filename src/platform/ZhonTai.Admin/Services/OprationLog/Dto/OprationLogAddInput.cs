namespace ZhonTai.Admin.Services.OprationLog.Dto;

/// <summary>
/// 添加
/// </summary>
public class OprationLogAddInput
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 接口名称
    /// </summary>
    public string ApiLabel { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    public string ApiPath { get; set; }

    /// <summary>
    /// 接口提交方法
    /// </summary>
    public string ApiMethod { get; set; }

    /// <summary>
    /// IP
    /// </summary>
    public string IP { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    public string Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    public string Os { get; set; }

    /// <summary>
    /// 设备
    /// </summary>
    public string Device { get; set; }

    /// <summary>
    /// 浏览器信息
    /// </summary>
    public string BrowserInfo { get; set; }

    /// <summary>
    /// 耗时（毫秒）
    /// </summary>
    public long ElapsedMilliseconds { get; set; }

    /// <summary>
    /// 操作状态
    /// </summary>
    public bool? Status { get; set; }

    /// <summary>
    /// 操作消息
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// 操作参数
    /// </summary>
    public string Params { get; set; }

    /// <summary>
    /// 操作结果
    /// </summary>
    public string Result { get; set; }
}