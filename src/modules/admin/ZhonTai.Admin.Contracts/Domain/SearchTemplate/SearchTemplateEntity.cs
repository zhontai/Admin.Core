using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;
namespace ZhonTai.Admin.Domain.PrintTemplate;

/// <summary>
///  查询模板
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "search_template")]
[Index("idx_{tablename}_01", $"{nameof(CreatedUserId)},{nameof(ModuleId)},{nameof(Name)}", true)]
public partial class SearchTemplateEntity : EntityVersion
{
    /// <summary>
    /// 模块Id
    /// </summary>
    public long ModuleId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }

    /// <summary>
    /// 模板
    /// </summary>
    [Column(StringLength = -1)]
    public string Template { get; set; }
}