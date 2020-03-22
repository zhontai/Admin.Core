using FreeSql.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Admin.Core.Model
{
    /// <summary>
    /// 实体创建审计
    /// </summary>
    public class EntityAdd<TKey> : Entity<TKey>, IEntityAdd
    {
        /// <summary>
        /// 创建者Id
        /// </summary>
        [Column(Position = -3, CanUpdate = false)]
        public long? CreatedUserId { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [Column(Position = -2, CanUpdate = false), MaxLength(50)]
        public string CreatedUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(Position = -1, CanUpdate = false, ServerTime = DateTimeKind.Local)]
        public DateTime? CreatedTime { get; set; }
    }

    public class EntityAdd : EntityAdd<long>
    {

    }
}
