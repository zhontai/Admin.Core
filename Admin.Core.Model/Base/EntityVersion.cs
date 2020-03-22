using FreeSql.DataAnnotations;

namespace Admin.Core.Model
{
    /// <summary>
    /// 实体版本
    /// </summary>
    public class EntityVersion<TKey> : Entity<TKey>, IEntityVersion
    {
        /// <summary>
        /// 版本
        /// </summary>
        [Column(Position = -1, IsVersion = true)]
        public long Version { get; set; }
    }

    public class EntityVersion : EntityVersion<long>
    {
    }
}
