using System;
using System.ComponentModel.DataAnnotations;
using FreeSql.DataAnnotations;

namespace Admin.Core.Common.BaseModel
{
    /// <summary>
    /// 实体修改审计
    /// </summary>
    public class EntityUpdate<TKey> : Entity<TKey>, IEntityUpdate<TKey> where TKey : struct
    {
        /// <summary>
        /// 修改者Id
        /// </summary>
        [Column(Position = -3, CanInsert = false)]
        public TKey? ModifiedUserId { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        [Column(Position = -2, CanInsert = false), MaxLength(50)]
        public string ModifiedUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column(Position = -1, CanInsert = false, ServerTime = DateTimeKind.Local)]
        public DateTime? ModifiedTime { get; set; }
    }

    public class EntityUpdate : EntityUpdate<long>
    {

    }
}
