using Admin.Core.Common.BaseModel;

namespace Admin.Core.Service.Admin.Tenant.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class TenantAddInput
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 数据隔离类型
        /// </summary>
        public DataIsolationType DataIsolationType { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        public FreeSql.DataType? DbType { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 空闲时间(分)
        /// </summary>
        public int? IdleTime { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
    }
}