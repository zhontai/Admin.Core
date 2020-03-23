using FreeSql.DataAnnotations;

namespace Admin.Core.Model
{
    public interface IEntity
    {
    }

    public class Entity<TKey> : IEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Column(Position = 1)]
        public virtual TKey Id { get; set; }
    }

    public class Entity : Entity<long>
    {

    }
}
