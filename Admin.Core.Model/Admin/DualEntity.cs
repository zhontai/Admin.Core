using FreeSql.DataAnnotations;
using System;

namespace Admin.Core.Model.Admin
{
    /// <summary>
    /// 系统表，用于查询系统函数
    /// </summary>
	[Table(Name = "ad_dual")]
    public class DualEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Column(Position = 1, IsPrimary = true, IsNullable = false)]
        public Guid Id { get; set; }
    }
}
