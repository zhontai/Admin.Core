using FreeSql.DataAnnotations;

namespace ZhonTai.Admin.Domain.OprationLog;

/// <summary>
/// 操作日志
/// </summary>
[Table(Name = "ad_opration_log")]
public partial class OprationLogEntity : LogAbstract
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
    /// 接口提交方法
    /// </summary>
    [Column(Position = 4, StringLength = 50)]
    public string ApiMethod { get; set; }

    /// <summary>
    /// 操作参数
    /// </summary>
    [Column(StringLength = -1)]
    public string Params { get; set; }
}