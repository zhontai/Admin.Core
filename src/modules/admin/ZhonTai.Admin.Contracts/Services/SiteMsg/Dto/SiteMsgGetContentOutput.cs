using System;

namespace ZhonTai.Admin.Services.SiteMsg.Dto;

public class SiteMsgGetContentOutput
{
    /// <summary>
    /// 消息Id
    /// </summary>
    public long MsgId { get; set; }

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

    /// <summary>
    /// 是否已读
    /// </summary>
    public bool? IsRead { get; set; }
}