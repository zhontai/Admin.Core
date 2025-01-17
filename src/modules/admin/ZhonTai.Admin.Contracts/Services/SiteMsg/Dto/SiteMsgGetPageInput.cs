namespace ZhonTai.Admin.Services.SiteMsg.Dto;

public partial class SiteMsgGetPageInput
{
    /// <summary>
    /// 是否已读
    /// </summary>
    public bool? IsRead { get; set; }

    /// <summary>
    /// 分类Id
    /// </summary>
    public long? TypeId { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }
}