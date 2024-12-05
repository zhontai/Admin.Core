using System;
namespace ZhonTai.Admin.Services.Msg.Dto;

public class MsgGetPageOutput
{
    /// <summary>
    /// 消息Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 类型Id
    /// </summary>
    public long TypeId { get; set; }

    /// <summary>
    /// 类型名称
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public virtual DateTime? CreatedTime { get; set; }
}