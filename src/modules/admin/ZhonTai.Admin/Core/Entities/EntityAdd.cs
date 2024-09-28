using FreeSql.DataAnnotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 实体创建
/// </summary>
public class EntityAdd<TKey> : Entity<TKey>, IEntityAdd<TKey> where TKey : struct
{
    /// <summary>
    /// 创建者用户Id
    /// </summary>
    [Description("创建者用户Id")]
    [Column(Position = -23, CanUpdate = false)]
    public virtual long? CreatedUserId { get; set; }

    /// <summary>
    /// 创建者用户名
    /// </summary>
    [Description("创建者用户名")]
    [Column(Position = -22, CanUpdate = false), MaxLength(50)]
    public virtual string CreatedUserName { get; set; }

    /// <summary>
    /// 创建者姓名
    /// </summary>
    [Description("创建者姓名")]
    [Column(Position = -21, CanUpdate = false), MaxLength(50)]
    public virtual string CreatedUserRealName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Description("创建时间")]
    [Column(Position = -20, CanUpdate = false)]
    [ServerTime]
    public virtual DateTime? CreatedTime { get; set; }
}

/// <summary>
/// 实体创建
/// </summary>
public class EntityAdd : EntityAdd<long>
{
}