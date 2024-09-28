using FreeSql.DataAnnotations;

namespace ZhonTai.Admin.Domain.OperationLog;

/// <summary>
/// 操作日志
/// </summary>
[Table(Name = "ad_operation_log", OldName = "ad_opration_log")]
public partial class OperationLogEntity : LogAbstract
{
    /// <summary>
    /// 接口名称
    /// </summary>
    [Column(Position = 2, StringLength = 50)]
    public string ApiLabel { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    [Column(Position = 3, StringLength = 500)]
    public string ApiPath { get; set; }

    /// <summary>
    /// 接口方法
    /// </summary>
    [Column(Position = 4, StringLength = 50)]
    public string ApiMethod { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    [Column(StringLength = -1)]
    public string Params { get; set; }

    /// <summary>
    /// 状态码
    /// </summary>
    public int? StatusCode { get; set; }

    /// <summary>
    /// 响应结果
    /// </summary>
    [Column(StringLength = -1)]
    public string Result { get; set; }
}