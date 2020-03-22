using FreeSql.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Admin.Core.Model
{
    /// <summary>
    /// 实体审计
    /// </summary>
    public class EntityBase<TKey> : Entity<TKey>, IEntityVersion, IEntitySoftDelete,IEntityAdd,IEntityUpdate
    {
        /// <summary>
        /// 版本
        /// </summary>
        [Column(Position = -9, IsVersion = true)]
        public long Version { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Column(Position = -8)]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 创建者Id
        /// </summary>
        [Column(Position = -7, CanUpdate = false)]
        public long? CreatedUserId { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [Column(Position = -6, CanUpdate = false),MaxLength(50)]
        public string CreatedUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(Position = -5, CanUpdate = false, ServerTime = DateTimeKind.Local)]
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// 修改者Id
        /// </summary>
        [Column(Position = -4, CanInsert = false)]
        public long? ModifiedUserId { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        [Column(Position = -2, CanInsert = false),MaxLength(50)]
        public string ModifiedUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column(Position = -1, CanInsert = false,ServerTime = DateTimeKind.Local)]
        public DateTime? ModifiedTime { get; set; }
    }

    public class EntityBase : EntityBase<long>
    {

    }
}
