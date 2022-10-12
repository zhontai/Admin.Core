using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 实体版本
/// </summary>
public class EntityVersion<TKey> : Entity<TKey>, IVersion
{
    /// <summary>
    /// 版本
    /// </summary>
    [Description("版本")]
    [Column(Position = -30, IsVersion = true)]
    public long Version { get; set; }
}

public class EntityVersion : EntityVersion<long>
{
}