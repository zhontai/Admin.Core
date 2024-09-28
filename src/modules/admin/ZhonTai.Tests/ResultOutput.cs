namespace ZhonTai.Tests;

/// <summary>
/// 返回结果
/// </summary>
/// <typeparam name="T"></typeparam>
public class ResultOutput<T>
{
    /// <summary>
    /// 是否成功标记
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 状态码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public T Data { get; set; }
}