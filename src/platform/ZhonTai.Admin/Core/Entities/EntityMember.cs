using FreeSql.DataAnnotations;
using System;
using System.ComponentModel;

namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 实体会员
/// </summary>
public class EntityMember<TKey> : Entity<TKey>, IMember, IDelete
{
    /// <summary>
    /// 会员Id
    /// </summary>
    [Description("会员Id")]
    [Column(Position = -23, CanUpdate = false)]
    public virtual long? MemberId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Description("创建时间")]
    [Column(Position = -20, CanUpdate = false, ServerTime = DateTimeKind.Local)]
    public virtual DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [Description("修改时间")]
    [Column(Position = -10, CanInsert = false, ServerTime = DateTimeKind.Local)]
    public virtual DateTime? ModifiedTime { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    [Description("是否删除")]
    [Column(Position = -9)]
    public virtual bool IsDeleted { get; set; } = false;
}

/// <summary>
/// 实体会员
/// </summary>
public class EntityMember : EntityMember<long>
{
}