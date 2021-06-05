
namespace Admin.Core.Model.Admin
{
    /// <summary>
    /// 租户数据库类型
    /// </summary>
    public enum TenantDbType
    {
        /// <summary>
        /// 独立数据库
        /// </summary>
        OwnDb,
        /// <summary>
        /// 独立数据表
        /// </summary>
        OwnDt,
        /// <summary>
        /// 共享数据库，独立架构
        /// </summary>
        Schema,
        /// <summary>
        /// 共享数据库
        /// </summary>
        Share
    }
}
