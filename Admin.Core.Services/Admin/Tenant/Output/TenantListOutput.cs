using Newtonsoft.Json;
using System;

namespace Admin.Core.Service.Admin.Tenant.Output
{
    public class TenantListOutput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        [JsonIgnore]
        public FreeSql.DataType? DbType { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DbTypeName => DbType.ToDescriptionOrString();

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

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedTime { get; set; }
    }
}
