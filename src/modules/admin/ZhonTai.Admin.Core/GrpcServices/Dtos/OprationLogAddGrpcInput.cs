using ProtoBuf;

namespace ZhonTai.Admin.Core.GrpcServices.Dtos;

/// <summary>
/// 操作日志
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.None)]
public class OperationLogAddGrpcInput
{
    /// <summary>
    /// 姓名
    /// </summary>
    [ProtoMember(1)]
    public string Name { get; set; }

    /// <summary>
    /// 接口名称
    /// </summary>
    [ProtoMember(2)]
    public string ApiLabel { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    [ProtoMember(3)]
    public string ApiPath { get; set; }

    /// <summary>
    /// 接口提交方法
    /// </summary>
    [ProtoMember(4)]
    public string ApiMethod { get; set; }

    /// <summary>
    /// IP
    /// </summary>
    [ProtoMember(5)]
    public string IP { get; set; }

    /// <summary>
    /// 国家
    /// </summary>
    [ProtoMember(6)]
    public string Country { get; set; }

    /// <summary>
    /// 省份
    /// </summary>
    [ProtoMember(7)]
    public string Province { get; set; }

    /// <summary>
    /// 城市
    /// </summary>
    [ProtoMember(8)]
    public string City { get; set; }

    /// <summary>
    /// 网络服务商
    /// </summary>
    [ProtoMember(9)]
    public string Isp { get; init; }

    /// <summary>
    /// 浏览器
    /// </summary>
    [ProtoMember(10)]
    public string Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [ProtoMember(11)]
    public string Os { get; set; }

    /// <summary>
    /// 设备
    /// </summary>
    [ProtoMember(12)]
    public string Device { get; set; }

    /// <summary>
    /// 浏览器信息
    /// </summary>
    [ProtoMember(13)]
    public string BrowserInfo { get; set; }

    /// <summary>
    /// 耗时（毫秒）
    /// </summary>
    [ProtoMember(14)]
    public long ElapsedMilliseconds { get; set; }

    /// <summary>
    /// 操作状态
    /// </summary>
    [ProtoMember(15)]
    public bool? Status { get; set; }

    /// <summary>
    /// 操作消息
    /// </summary>
    [ProtoMember(16)]
    public string Msg { get; set; }

    /// <summary>
    /// 操作参数
    /// </summary>
    [ProtoMember(17)]
    public string Params { get; set; }

    /// <summary>
    /// 状态码
    /// </summary>
    [ProtoMember(18)]
    public int? StatusCode { get; set; }

    /// <summary>
    /// 操作结果
    /// </summary>
    [ProtoMember(19)]
    public string Result { get; set; }
}