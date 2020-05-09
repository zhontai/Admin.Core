using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace Admin.Core.Common.BaseModel
{
    /// <summary>
    /// 实体版本
    /// </summary>
    public class EntityVersion<TKey> : Entity<TKey>, IEntityVersion
    {
        /// <summary>
        /// 版本
        /// </summary>
        [Description("版本")]
        [Column(Position = -1, IsVersion = true)]
        public long Version { get; set; }
    }

    public class EntityVersion : EntityVersion<long>
    {
    }
}
