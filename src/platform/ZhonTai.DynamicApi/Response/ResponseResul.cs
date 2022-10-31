namespace ZhonTai.DynamicApi;

public class ResponseResul<T>
{
    /// <summary>
    /// 是否成功标记
    /// </summary>
    public bool Success { get; private set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Msg { get; private set; }

    /// <summary>
    /// 数据
    /// </summary>
    public T Data { get; private set; }
}