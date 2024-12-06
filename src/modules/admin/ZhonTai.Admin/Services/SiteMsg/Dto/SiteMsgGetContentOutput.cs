using System;

namespace ZhonTai.Admin.Services.SiteMsg.Dto;

public class SiteMsgGetContentOutput
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 类型名称
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 接收时间
    /// </summary>
    public virtual DateTime? ReceivedTime { get; set; }
}