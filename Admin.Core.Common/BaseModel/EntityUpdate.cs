using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FreeSql.DataAnnotations;

namespace Admin.Core.Common.BaseModel
{
    /// <summary>
    /// 实体修改
    /// </summary>
    public class EntityUpdate<TKey> : Entity<TKey>, IEntityUpdate<TKey> where TKey : struct
    {
        /// <summary>
        /// 修改者Id
        /// </summary>
        [Description("修改者Id")]
        [Column(Position = -3, CanInsert = false)]
        public long? ModifiedUserId { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        [Description("修改者")]
        [Column(Position = -2, CanInsert = false), MaxLength(50)]
        public string ModifiedUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Description("修改时间")]
        [Column(Position = -1, CanInsert = false, ServerTime = DateTimeKind.Local)]
        public DateTime? ModifiedTime { get; set; }
    }

    public class EntityUpdate : EntityUpdate<long>
    {

    }
}
