using ProtoBuf;

namespace ZhonTai.Admin.Core.GrpcServices.Dtos;

/// <summary>
/// 接口
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.None)]
public class ApiGrpcOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [ProtoMember(1)]
    public long Id { get; set; }

    /// <summary>
    /// 所属模块
    /// </summary>
    [ProtoMember(2)]
    public long ParentId { get; set; }

    /// <summary>
    /// 接口名称
    /// </summary>
    [ProtoMember(3)]
    public string Label { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    [ProtoMember(4)]
    public string Path { get; set; }

    /// <summary>
    /// 启用接口日志
    /// </summary>
    [ProtoMember(5)]
    public bool EnabledLog { get; set; }

    /// <summary>
    /// 启用请求参数
    /// </summary>
    [ProtoMember(6)]
    public bool EnabledParams { get; set; }

    /// <summary>
    /// 启用响应结果
    /// </summary>
    [ProtoMember(7)]
    public bool EnabledResult { get; set; }
}