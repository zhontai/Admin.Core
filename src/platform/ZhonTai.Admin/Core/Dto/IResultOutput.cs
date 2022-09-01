namespace ZhonTai.Admin.Core.Dto;

/// <summary>
/// 结果输出接口
/// </summary>
public interface IResultOutput
{
    /// <summary>
    /// 是否成功
    /// </summary>
    bool Success { get; }

    /// <summary>
    /// 消息
    /// </summary>
    string Msg { get; }

    /// <summary>
    /// 编码
    /// </summary>
    string Code { get; set; }
}

/// <summary>
/// 泛型结果输出接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResultOutput<T> : IResultOutput
{
    /// <summary>
    /// 返回数据
    /// </summary>
    T Data { get; }
}