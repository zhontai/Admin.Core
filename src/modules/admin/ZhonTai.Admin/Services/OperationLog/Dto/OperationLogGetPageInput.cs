using System;

namespace ZhonTai.Admin.Services.OperationLog.Dto;

public class OperationLogGetPageInput
{
    /// <summary>
    /// 创建者
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 操作状态
    /// </summary>
    public bool? Status { get; set; }

    /// <summary>
    /// 操作接口
    /// </summary>
    public string Api { get; set; }

    /// <summary>
    /// IP
    /// </summary>
    public string IP { get; set; }

    /// <summary>
    /// 创建开始时间
    /// </summary>
    public DateTime? AddStartTime { get; set; }

    /// <summary>
    /// 创建结束时间
    /// </summary>
    public DateTime? AddEndTime { get; set; }
}