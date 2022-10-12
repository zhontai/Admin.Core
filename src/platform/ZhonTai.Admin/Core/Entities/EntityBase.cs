using FreeSql.DataAnnotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 实体基类
/// </summary>
public class EntityBase<TKey> : Entity<TKey>, IVersion, IDelete, IEntityAdd<TKey>, IEntityUpdate<TKey> where TKey : struct
{
    /// <summary>
    /// 是否删除
    /// </summary>
    [Description("是否删除")]
    [Column(Position = -40)]
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// 版本
    /// </summary>
    [Description("版本")]
    [Column(Position = -30, IsVersion = true)]
    public long Version { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    [Description("创建者Id")]
    [Column(Position = -22, CanUpdate = false)]
    public long? CreatedUserId { get; set; }

    /// <summary>
    /// 创建者
    /// </summary>
    [Description("创建者")]
    [Column(Position = -21, CanUpdate = false), MaxLength(50)]
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Description("创建时间")]
    [Column(Position = -20, CanUpdate = false, ServerTime = DateTimeKind.Local)]
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 修改者Id
    /// </summary>
    [Description("修改者Id")]
    [Column(Position = -12, CanInsert = false)]
    public long? ModifiedUserId { get; set; }

    /// <summary>
    /// 修改者
    /// </summary>
    [Description("修改者")]
    [Column(Position = -11, CanInsert = false), MaxLength(50)]
    public string ModifiedUserName { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [Description("修改时间")]
    [Column(Position = -10, CanInsert = false, ServerTime = DateTimeKind.Local)]
    public DateTime? ModifiedTime { get; set; }
}

/// <summary>
/// 实体基类
/// </summary>
public class EntityBase : EntityBase<long>
{
}